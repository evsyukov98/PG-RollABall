using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class PlayerController : MonoBehaviour
{
    public Transform WorldPoint;
    
    public float speed;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;

    public int jumpForce = 5;
    public bool IsGrounded;
    public int extraJumps;
    public int extraJumpValue = 2;

    public InputAction a;

    void Start()
    {
        a.performed += _ => Jump();
        
        rb = GetComponent<Rigidbody>();

        count = 0;

        SetCountText();
    }

    private void Update()
    {
        GroundCheker();
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);

    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;

        movementY = movementVector.y;
    }

    void GroundCheker()
    {
        int checker = 0;
        
        foreach(Collider collider in Physics.OverlapSphere(transform.position, 1f))
        {
            if (collider.tag == "Ground")
            {
                checker++;
            }
        }

        if(checker > 0)
        {
            IsGrounded = true;
            extraJumps = extraJumpValue;
        }
        else
        {
            IsGrounded = false;
        }
       
    }

    private void Jump()
    {
        Debug.Log("try jump!!");

        if (IsGrounded || extraJumps > 0)
        {
            rb.AddForce(Vector3.up * jumpForce);

            extraJumps--;
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        if (count >= 12)
        {
            winTextObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);

            count = count + 1;

            SetCountText();
        }      
    }

    private void OnEnable()
    {
        a.Enable();
    }
}
