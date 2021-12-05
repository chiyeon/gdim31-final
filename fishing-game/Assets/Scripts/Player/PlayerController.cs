using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // singleton
    public static PlayerController instance;

    [Header("Player Controls")]
    // mouse stuff
    [SerializeField] private float mouseSensitivity = 30.0f;
    private float xRotation = 0f;
    private float yRotation = 0f;
    // movement stuff
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float jumpForce = 10.0f;
    private bool isNavigating = false;      // whether we are controlling the player or the boat
    private bool disableControls = false;   // enable to stop taking input
    private bool isGrounded = false;        // for jumping
    private Vector3 velocity;               // used w movement
    private bool jump = false;              // pass jump to fixed update

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;     // point to check grounded
    [SerializeField] private float groundCheckDistance = 0.3f;
    [SerializeField] private LayerMask groundLayer;

    [Header("References")]
    [SerializeField] private Transform mainCamera;
    private Rigidbody rb;
    private Animator animator;

    private Vector3 dir;
    private Vector2 mouseLook;
    private Collider col;
    private int currentZone;
    private GameObject currentZoneObject;

    void Awake() {
        instance = this;            // set up singleton
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        animator = GetComponent<Animator>();

        Global.instance.SetFPSMouse(true);      // disable mouse cursor
    }

    void Update()
    {
        // mouse look
        if(!disableControls)
            mouseLook = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * mouseSensitivity * Time.deltaTime;
        else
            mouseLook = Vector2.zero;

        xRotation -= mouseLook.y;
        xRotation = Mathf.Clamp(xRotation, -80, 80);
        yRotation += mouseLook.x;

        mainCamera.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    
        // check if touching ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);
        if(Input.GetButtonDown("Jump") && isGrounded && !disableControls) {
            jump = true;
        }

        // lock position while navigating boat
        if(isNavigating) {
            transform.position = transform.parent.position;
            return;
        }

        // move player & jump
        if(!disableControls)
            dir = mainCamera.right * Input.GetAxis("Horizontal") * speed + mainCamera.forward * Input.GetAxis("Vertical") * speed;
        else    // dont allow movement when disabled
            dir = Vector3.zero;
    }
    
    void FixedUpdate() {
        dir.y = rb.velocity.y;
        rb.velocity = dir;

        if(jump) {
            jump = false;
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    // set whether we are in control of boat or player
    // parent Transform is the base position to which the player will be when navigating
    public void SetNavigating(bool _isNavigating, Transform parent) {
        isNavigating = _isNavigating;
        animator.SetBool("isNavigating", _isNavigating);        // lower/raise fishing rod animation

        if(isNavigating) {
            transform.SetParent(parent);
            FishingController.instance.OnNavigating();
        } else {
            transform.SetParent(null);
        }
    }

    public bool GetNavigating() {
        return isNavigating;
    }

    public void SetDisableControls(bool _disableControls) {
        disableControls = _disableControls;
    }

    public bool GetDisableControls() {
        return disableControls;
    }

    public void SetZone(int _zoneID, GameObject _currentZoneObject) {
        Debug.Log("Player zone is now " + _zoneID);
        currentZone = _zoneID;
        currentZoneObject = _currentZoneObject;
        FishingController.instance.ResetCatchCounter();
    }

    public int GetZone() {
        return currentZone;
    }

    public void DisableCurrentZoneObject() {
        if(currentZoneObject) {
            currentZoneObject.SetActive(false);
        }
    }
}
