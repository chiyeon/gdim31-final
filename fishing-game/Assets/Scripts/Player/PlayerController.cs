using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    [Header("Player Controls")]
    [SerializeField]
    private float mouseSensitivity = 30.0f;
    private float xRotation = 0f;
    [SerializeField]
    private float speed = 10.0f;
    [SerializeField]
    private float jumpForce = 10.0f;
    [SerializeField]
    private float gravity = -10f;
    private bool isGrounded = false;
    private Vector3 velocity;

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
    private CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        Global.instance.SetFPSMouse(true);
    }

    // Update is called once per frame
    void Update()
    {
        // mouse look
        Vector2 mouseLook = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseLook.y;
        xRotation = Mathf.Clamp(xRotation, -80, 80);

        transform.Rotate(Vector3.up * mouseLook.x);
        mainCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    
        // check if touching ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);
        if(isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        // move player & jump
        Vector3 dir = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");

        controller.Move(dir * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump")) {
            velocity.y = jumpForce;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
