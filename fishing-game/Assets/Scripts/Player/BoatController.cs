using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    [Header("Boat Movement")]
    [SerializeField]
    private float acceleration = 0.5f;
    [SerializeField]
    private float maxForwardSpeed = 5.0f;
    [SerializeField]
    private float maxBackwardSpeed = 2.0f;
    private float speed = 0;
    [SerializeField]
    private bool isNavigating = false;
    [SerializeField]
    private float rotationSpeed = 15.0f;

    [Header("Player Search")]
    [SerializeField]
    private Transform playerSearch;
    [SerializeField]
    private float playerSearchRadius = 5.0f;
    [SerializeField]
    private LayerMask playerLayer;
    private bool isPlayerNear = false;

    [Header("References")]
    [SerializeField]
    private GameObject hitboxes;
    [SerializeField]
    private Transform playerNavPosition;
    private CharacterController controller;
    [SerializeField]
    private Transform engine;
    [SerializeField]
    private Transform boatModel;

    // controller statse
    private Vector3 controllerCenterDisabled = new Vector3(0, 12, 0);
    private Vector3 controllerCenterEnabled = new Vector3(0, 0, 0);


    void Start() {
        controller = GetComponent<CharacterController>();
        //controller.detectCollisions = false;
    }

    void Update() {
        boatModel.Translate(transform.up * Mathf.Sin(Time.time * 3) * Time.deltaTime * 0.35f);

        // determine if player is nearby
        isPlayerNear = Physics.CheckSphere(playerSearch.position, playerSearchRadius, playerLayer);

        // enable / disable navigating mode when interactign with
        if(Input.GetButtonDown("Interact") && isPlayerNear) {
            SetNavigating(!isNavigating);
        }

        // while interacting, allow player to move the boat
        if(isNavigating) {
            // inc / dec forward movement;
            Vector3 dir = Vector3.right * Input.GetAxis("Horizontal") + Vector3.forward * Input.GetAxis("Vertical");
            // brake when pressing space
            if(Input.GetButton("Jump")) {
                speed = Mathf.Lerp(speed, 0, Time.deltaTime * acceleration);
            } else {
                // otherwise inc / dec speed accordingly
                if(dir.z > 0) {
                    speed = Mathf.Lerp(speed, maxForwardSpeed, acceleration * Time.deltaTime);
                } else if(dir.z < 0) {
                    speed = Mathf.Lerp(speed, -maxBackwardSpeed, acceleration * Time.deltaTime);
                }
            }

            // rotate as long as player has enough speed
            float rotationStrength = Mathf.Clamp(speed/3, 0, 1);
            transform.Rotate(transform.up * dir.x * Time.deltaTime * rotationSpeed * rotationStrength);
            // rotate engine too
            //engine.localEulerAngles = Vector3.Lerp(engine.localEulerAngles, new Vector3(0, dir.x * 35 * rotationStrength, 0), Time.deltaTime);
            engine.localRotation = Quaternion.Lerp(engine.localRotation, Quaternion.Euler(new Vector3(0, rotationStrength * dir.x * 35, 0)), Time.deltaTime);
        } else {
            // set speed gradually to 0 when player stops navigating
            speed = Mathf.Lerp(speed, 0, Time.deltaTime);
            // reset engine pos
            engine.localRotation = Quaternion.Lerp(engine.localRotation, Quaternion.Euler(new Vector3(0, 0, 0)), Time.deltaTime);
        }

        // move according to correct speed
        controller.Move(transform.forward * speed * Time.deltaTime);
    }

    // enalbed / disables navigating
    void SetNavigating(bool _isNavigating) {
        isNavigating = _isNavigating;                                       // change bool
        PlayerController.instance.SetNavigating(isNavigating, playerNavPosition);        // alert player class
        // this is bugged. sad face.
        //controller.detectCollisions = _isNavigating;

    }
}
