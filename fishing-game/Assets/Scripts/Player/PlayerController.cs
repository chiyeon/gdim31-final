using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // singleton
    public static PlayerController instance;

    [Header("Player Controls")]
    [SerializeField]
    private float mouseSensitivity = 30.0f;
    private float xRotation = 0f;
    private float yRotation = 0f;
    [SerializeField]
    private float speed = 10.0f;
    [SerializeField]
    private float jumpForce = 10.0f;
    //[SerializeField]
    //private float gravity = -10f;
    private bool isNavigating = false;
    private bool isGrounded = false;
    private Vector3 velocity;
    private bool jump = false;

    [Header("Ground Check")]
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundCheckDistance = 0.3f;
    [SerializeField]
    private LayerMask groundLayer;

    [Header("References")]
    [SerializeField]
    private Transform mainCamera;
    //private CharacterController controller;
    private Rigidbody rb;

    private Vector3 dir;
    private Collider col;
    void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        Global.instance.SetFPSMouse(true);
    }

    // Update is called once per frame
    void Update()
    {
        // mouse look
        Vector2 mouseLook = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseLook.y;
        xRotation = Mathf.Clamp(xRotation, -80, 80);
        yRotation += mouseLook.x;

        //transform.Rotate(Vector3.up * mouseLook.x);
        mainCamera.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    
        // check if touching ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);
        if(Input.GetButtonDown("Jump") && isGrounded) {
            //velocity.y = jumpForce;
            jump = true;
        }
        /*
        if(isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }*/

        if(isNavigating) {
            transform.position = transform.parent.position;
            return;
        }
        // move player & jump
        dir = mainCamera.right * Input.GetAxis("Horizontal") * speed + mainCamera.forward * Input.GetAxis("Vertical") * speed;

        //controller.Move(dir * speed * Time.deltaTime);

        //velocity.y += gravity * Time.deltaTime;

        //controller.Move(velocity * Time.deltaTime);
    }
    
    void FixedUpdate() {
        dir.y = rb.velocity.y;
        rb.velocity = dir;

        if(jump) {
            jump = false;
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void SetNavigating(bool _isNavigating, Transform parent) {
        isNavigating = _isNavigating;
        
        if(isNavigating) {
            transform.SetParent(parent);
        } else {
            transform.SetParent(null);
        }
    }
}
