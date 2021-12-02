using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingController : MonoBehaviour
{
    public static FishingController instance;
    [Header("Fishing Variables")]
    private float fishingPower = 0;
    [SerializeField]
    private float maxFishingPower = 5;
    [SerializeField]
    private float fishingPowerAcceleration = 1.25f;
    private PlayerController controller;
    private BobberController bobberInstance;
    private bool casted = false;
    private bool renderLine = false;
    private Vector3 linePosition;

    [Header("Fishing Rod Angles")]
    [SerializeField]
    private float idleAngle = 0;
    [SerializeField]
    private float fullPowerAngle = -70;
    [SerializeField]
    private float castedAngle = 35;
    [SerializeField]
    private float reelingAngle = 15;
    private bool justCaught = false;
    private bool transferringLine = false; // active when we are switching to fish catching animation

    [Header("Catch Stuffs")]
    private GameObject fishModelInstance;

    [Header("Catch Database")]
    public List<GameObject> fishModels;
    [Header("Pulling Stuff")]
    [SerializeField]
    private float pullDecreaseAmount = 0.35f;
    [SerializeField]
    private float pullIncreaseAmount = 0.5f;
    [SerializeField]
    private float pullTimer = 0;
    private int fishInstance = -1;       // make sure we dont get the same fish over and over !

    [Header("References")]
    [SerializeField]
    private Transform bobberRelease;
    [SerializeField]
    private GameObject Bobber;
    [SerializeField]
    private GameObject FishingRod;
    private Coroutine swingAnimationCoroutine;
    [SerializeField]
    private GameObject spinningThing;
    [SerializeField]
    private Collider finishFishingRadius;
    private Animator animator;
    [SerializeField]
    private Transform CaughtObject;
    [SerializeField]
    private LineRenderer lineRenderer;
    
    
    void Awake() {
        instance = this;
    }

    void Start() {
        controller = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        finishFishingRadius.enabled = false;
        lineRenderer.enabled = false;
    }

    void Update() {
        if(renderLine) {
            lineRenderer.enabled = true;
            // make sure fishing line is always connected to the rod first
            lineRenderer.SetPosition(0, lineRenderer.transform.position);
            lineRenderer.SetPosition(1, linePosition);

            // if we are actively fishing, connect it to bobber, otherwise connect it to the caught fish (for the animation)
            if(fishModelInstance != null) {
                if(transferringLine) {
                    linePosition = Vector3.Lerp(linePosition, fishModelInstance.transform.position, Time.deltaTime * 3);
                } else {
                    linePosition = fishModelInstance.transform.position;
                }
            } else {
                if(bobberInstance) {
                    linePosition = bobberInstance.GetComponent<BobberController>().GetBobberModel().position;
                }
            }
        } else {
            lineRenderer.enabled = false;
        }

        if(!controller.GetNavigating()) {
            if(!controller.GetDisableControls()) {
                if(!casted) {
                    if(!justCaught) {
                        if(Input.GetButton("Fire1")) {
                            fishingPower = Mathf.Lerp(fishingPower, maxFishingPower, Time.deltaTime * fishingPowerAcceleration);
                            float targetRot = (fullPowerAngle - idleAngle) * (fishingPower / maxFishingPower) + idleAngle;
                            FishingRod.transform.localRotation = Quaternion.Lerp(FishingRod.transform.localRotation, Quaternion.Euler(new Vector3(targetRot, 0, 0)), Time.deltaTime * 5);
                        } else {
                            FishingRod.transform.localRotation = Quaternion.Lerp(FishingRod.transform.localRotation, Quaternion.Euler(new Vector3(idleAngle, 0, 0)), Time.deltaTime * 2);
                        }
                        
                        if(Input.GetButtonUp("Fire1")) {
                            if(bobberInstance == null) {
                                bobberInstance = Instantiate(Bobber, bobberRelease.position, bobberRelease.rotation).GetComponent<BobberController>();
                                bobberInstance.Launch(fishingPower * 60);
                                casted = true;
                                renderLine = true;
                                pullTimer = 0;
                            }
                        }
                    }
                } else {

                    // poll see if player will get random item

                    if(Input.GetButton("Fire1")) {
                        // make bobber move towards player
                        bobberInstance.pulling = true;
                        // make fishing rod move up to its reeling angle
                        FishingRod.transform.localRotation = Quaternion.Lerp(FishingRod.transform.localRotation, Quaternion.Euler(new Vector3(reelingAngle, 0, 0)), Time.deltaTime * 3);
                        // spin the spinny thing
                        spinningThing.transform.Rotate(Vector3.right, -1000 * Time.deltaTime, Space.Self);
                        // increase the pull timer !
                        pullTimer += Time.deltaTime * pullIncreaseAmount;
                        if(pullTimer >= 1) {
                            // we have pulled too fast! cut the line
                            Release();
                        }
                    } else {
                        // casted angle
                        FishingRod.transform.localRotation = Quaternion.Lerp(FishingRod.transform.localRotation, Quaternion.Euler(new Vector3(castedAngle, 0, 0)), Time.deltaTime * 15);
                        // let pull timer cool down
                        pullTimer -= Time.deltaTime * pullDecreaseAmount;
                        // stop bobber from moving
                        if(bobberInstance)
                            bobberInstance.pulling = false;
                    }
                    pullTimer = Mathf.Clamp(pullTimer, 0f, 1f);
                }

                if(Input.GetButtonDown("Fire2")) {
                    if(casted)
                        Release();
                }
                if(Input.GetButtonUp("Fire1")) {
                    justCaught = false;
                }
            }
        }
    }

    void FixedUpdate() {
        if(casted) {
            float shakeAmount = pullTimer - 0.5f;
            shakeAmount = Mathf.Clamp(shakeAmount, 0f, 1f);
            CameraShake.instance.Shake(0.1f, shakeAmount/15f);
        }
    }

    public void OnNavigating() {
        Release();
        justCaught = false;
    }
    // cuts line and resets fishing
    public void Release() {
        casted = false;
        finishFishingRadius.enabled = false;
        fishingPower = 0;
        if(bobberInstance) {
            bobberInstance.Remove();
            bobberInstance = null;
        }
        CutLine();
    }

    // called from bobber when it enters the finish fishing radius
    // gives catch and releases
    public void Catch(bool caught) {
        // enable this bool to make it so player can keep holding left click and it wont cast again
        justCaught = true;
        Release();

        if(caught) {
            // start animation, then rest handled by animation events
            // determine what kind of catch it is
            float det = Random.Range(0f, 1f);
            if(det <= 0.30f) {                          // 30% chance jumpscare
                animator.SetTrigger("Jumpscare");
            } else if(det <= 0.80) {                    // 50% chance fish
                animator.SetTrigger("Fish");
            } else {                                    // 20% chance item
                Debug.Log("item!");
            }
        } // otherwise do nothing because no catch !
    }

    // run when bobber lands on valid water and sticks
    public void BobberLanded() {
        finishFishingRadius.enabled = true;
    }

    public void StartCatching() {
        controller.SetDisableControls(true);
        renderLine = true;      // reenable, turned off in Release()
        transferringLine = true;
    }

    public void EndCatching() {
        controller.SetDisableControls(false);
        justCaught = false;
    }

    public void CreateFishInstance() {
        int newFish = -1;
        do {
            newFish = Random.Range(0, fishModels.Count);
        } while(newFish == fishInstance);
        fishInstance = newFish;
        fishModelInstance = Instantiate(fishModels[fishInstance], CaughtObject);
    }

    public void StopLineTransfer() {
        transferringLine = false;
    }

    public void DestroyFishInstance() {
        if(fishModelInstance)
            Destroy(fishModelInstance);
    }

    public void JumpScare() {
        CameraShake.instance.Shake(0.35f, 0.05f);
        CutLine();
    }

    public void CutLine() {
        renderLine = false;
    }
}
