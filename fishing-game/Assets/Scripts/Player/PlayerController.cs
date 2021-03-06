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
    [SerializeField] private List<bool> zones;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;     // point to check grounded
    [SerializeField] private float groundCheckDistance = 0.3f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Sounds")]
    [SerializeField] private AudioClip SitUp;
    [SerializeField] private AudioClip SitDown;
    [SerializeField] private AudioClip Scare;

    [Header("References")]
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Camera subCamera;
    private Rigidbody rb;
    private Animator animator;
    [SerializeField] private AudioSource PlayerAudioSource;

    private Vector3 dir;
    private Vector2 mouseLook;
    private Collider col;
    private int currentZone;
    private GameObject currentZoneObject;
    private Transform fishermanHead;

    private bool dead = false;

    void Awake() {
        instance = this;            // set up singleton
    }

    void Start()
    {
        mouseSensitivity = Global.instance.GetMouseSensitivity();

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        animator = GetComponent<Animator>();

        Global.instance.SetFPSMouse(true);      // disable mouse cursor

        for(int i = 0; i < 6; i++) {
            zones.Add(true);
        }
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
        if(!dead)
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
        
        if(dead) {
            Vector3 lookDir = (fishermanHead.position - mainCamera.position).normalized;
            Quaternion targetRot = Quaternion.LookRotation(lookDir);
            mainCamera.rotation = Quaternion.Lerp(mainCamera.rotation, targetRot, Time.deltaTime * 2);
            subCamera.fieldOfView = Mathf.Lerp(subCamera.fieldOfView, 10, Time.deltaTime);
        }
    }

    public void PlaySound(AudioClip clip) {
        PlayerAudioSource.pitch = 1;
        PlayerAudioSource.PlayOneShot(clip);
    }

    public void PlaySoundRandPitch(AudioClip clip) {
        PlayerAudioSource.pitch = Random.Range(0.75f, 1.25f);
        PlayerAudioSource.PlayOneShot(clip);
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
            PlayerAudioSource.PlayOneShot(SitUp);
        } else {
            transform.SetParent(null);
            PlayerAudioSource.PlayOneShot(SitDown);

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
        if(!zones[_zoneID])
            return;

        Debug.Log("Player zone is now " + _zoneID);
        currentZone = _zoneID;
        currentZoneObject = _currentZoneObject;

        if(_zoneID == 0 || _zoneID == 5) {
            BoatController.instance.GetComponent<RippleManager>().enabled = false;
        } else {
            BoatController.instance.GetComponent<RippleManager>().enabled = true;
        }
    }

    public int GetZone() {
        return currentZone;
    }

    public void DisableCurrentZoneObject() {
        if(currentZone != 5)
            zones[currentZone] = false;
    }

    public void SetMouseSensitivity(float _mouseSensitivity) {
        mouseSensitivity = _mouseSensitivity;
    }

    public List<bool> GetZones() {
        return zones;
    }

    public void SetZones(List<bool> _zones) {
        zones = _zones;

        GameObject[] z = GameObject.FindGameObjectsWithTag("Zone");

        for(int i = 0; i < z.Length; i++) {
            if(!zones[i+1]) {
                z[i].GetComponent<Collider>().enabled = false;
            }
        }
    }

    public void KillPlayer(Transform _fishermanHead) {
        if(dead)
            return;
        animator.SetTrigger("Dead");
        StartCoroutine(KillPlayerCoroutine(_fishermanHead));
    }

    IEnumerator KillPlayerCoroutine(Transform _fishermanHead) {
        SetDisableControls(true);

        PlaySound(Scare);

        fishermanHead = _fishermanHead;
        dead = true;

        yield return new WaitForSeconds(3f);

        Global.instance.PlayTextScene("A chill goes down your spine.", 1, 6);
    }
}
