using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Security.Principal;

public class PlayerController : MonoBehaviour
{
    public Transform WorldPoint;
    
    public float speed;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private Rigidbody _rigidBody;
    private int count = 0;

    public int jumpForce = 5;
    public bool IsGrounded;
    public int extraJumps;
    public int extraJumpValue = 2;

    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = new PlayerInput();

        _playerInput.Player.Jump.performed += _ => Jump();

    }

    void Start()
    {   
        _rigidBody = GetComponent<Rigidbody>();

        SetCountText();
    }

    private void Update()
    {
        Vector2 moveDirection = _playerInput.Player.Move.ReadValue<Vector2>();

        Move(moveDirection);

        GroundCheker();
    }

    void Move(Vector2 direction)
    {
        Vector3 dirVector3 = new Vector3(direction.x, 0f, direction.y);
        _rigidBody.AddForce(dirVector3 * speed );
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
            _rigidBody.AddForce(Vector3.up * jumpForce);

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
        _playerInput.Enable();
    }
}
