using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//remember too put player on player tag
public class playercontroller : MonoBehaviour
{

    [SerializeField] Transform PlayerCamera = null;
    [SerializeField] float Mousesensetivity = 3.5f;
    [SerializeField] float Walkspeed = 6.0f;
    [SerializeField] float JumpHeight = 3f;
    [SerializeField] float gravity = -13.0f;
    [SerializeField][Range(0.0f, 0.5f)] float MoveSmoothTime = 0.3f;
    [SerializeField][Range(0.0f, 0.5f)] float MouseSmoothTime = 0.03f;

    void OnCollisionStay()
    {
        isGrounded = true;
    }

    public float speed = 10f;

    public float jumpHeight = 5f;

    [SerializeField] bool Lockedcursor = true;

    float cameraPitch = 0.0f;
    float velocityY = 0.0f;
    CharacterController controller = null;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;


    Vector3 velocity;
    bool isGrounded;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirvelocity = Vector2.zero;

    Vector2 currentmousedelta = Vector2.zero;
    Vector2 currentmousedeltavelocity = Vector2.zero;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (Lockedcursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


    void Update()
    {
        UpdateMouseLook();
        updatemovement();
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButton("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);


    }

    void GroundCheck()
    {
        RaycastHit hit;
        float distance = 1f;
        Vector3 dir = new Vector3(0, -1);

        if (Physics.Raycast(transform.position, dir, out hit, distance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    void UpdateMouseLook()
    {
        Vector2 targetmouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentmousedelta = Vector2.SmoothDamp(currentmousedelta, targetmouseDelta, ref currentmousedeltavelocity, MouseSmoothTime);


        cameraPitch -= currentmousedelta.y * Mousesensetivity;

        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        PlayerCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * currentmousedelta.x * Mousesensetivity);
    }


    void updatemovement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirvelocity, MoveSmoothTime);

        if (controller.isGrounded)
            velocityY = 0.0f;

        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * Walkspeed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);
    }
}