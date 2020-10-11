using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class PlayerController : MonoBehaviour
{
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
    public int extraJumpvalue = 2;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        count = 0;

        SetCountText();
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
        
        foreach(Collider collider in Physics.OverlapSphere(transform.position, 1.1f))
        {
            if (collider.tag == "Ground")
            {
                checker++;
            }
        }

        if(checker > 0)
        {
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }
       
    }

    private void Update()
    {
        GroundCheker();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space");
            Vector3 JumpVector = this.transform.position + Vector3.up;
            rb.AddForce(JumpVector * jumpForce);
        }
    }
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);

        
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
}
