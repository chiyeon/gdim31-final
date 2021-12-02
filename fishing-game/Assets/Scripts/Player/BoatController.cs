using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    [Header("Boat Movement")]
    // boat movement
    [SerializeField] private float acceleration = 0.5f;         // rate at which speed inc
    [SerializeField] private float maxForwardSpeed = 5.0f;
    [SerializeField] private float maxBackwardSpeed = 2.0f;
    private float speed = 0;
    [SerializeField] private float rotationSpeed = 15.0f;
    private bool isNavigating = false;

    [Header("Player Search")]       // determine whether player is close enough to interact
    [SerializeField] private Transform playerSearch;
    [SerializeField] private float playerSearchRadius = 5.0f;
    [SerializeField] private LayerMask playerLayer;
    private bool isPlayerNear = false;

    [Header("References")]
    [SerializeField] private GameObject hitboxes;
    [SerializeField] private Transform playerNavPosition; 
    private CharacterController controller;
    [SerializeField] private Transform engine;
    [SerializeField] private Transform boatModel;


    void Start() {
        controller = GetComponent<CharacterController>();
    }

    void Update() {
        // make boat bob up and down
        // todo increase bobbing speed based on boat speed
        boatModel.Translate(transform.up * Mathf.Sin(Time.time * 3) * Time.deltaTime * 0.35f);

        // determine if player is nearby
        isPlayerNear = Physics.CheckSphere(playerSearch.position, playerSearchRadius, playerLayer);

        // enable / disable navigating mode when interacted with
        if(Input.GetButtonDown("Interact") && isPlayerNear) {
            SetNavigating(!isNavigating);
        }

        // while interacting, allow player to move the boat
        if(isNavigating) {
            // poll input
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

            // rotate with strength based on how fast we are going
            float rotationStrength = Mathf.Clamp(speed/3, 0, 1);
            transform.Rotate(transform.up * dir.x * Time.deltaTime * rotationSpeed * rotationStrength);
            // procedurally rotate the engine (no need to base on strength)
            engine.localRotation = Quaternion.Lerp(engine.localRotation, Quaternion.Euler(new Vector3(0, dir.x * 35, 0)), Time.deltaTime);
        } else {
            // set speed gradually to 0 when player stops navigating
            speed = Mathf.Lerp(speed, 0, Time.deltaTime * 2);
            // reset engine position to normal
            engine.localRotation = Quaternion.Lerp(engine.localRotation, Quaternion.Euler(new Vector3(0, 0, 0)), Time.deltaTime);
        }

        // move according to correct speed
        controller.Move(transform.forward * speed * Time.deltaTime);
    }

    // enalbed / disables navigating
    void SetNavigating(bool _isNavigating) {
        isNavigating = _isNavigating;                                       // change bool
        PlayerController.instance.SetNavigating(isNavigating, playerNavPosition);        // alert player class

    }
}
