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
                            float targetRot = fullPowerAngle * fishingPower / maxFishingPower;
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
                            }
                        }
                    }
                } else {

                    // poll see if player will get random item

                    if(Input.GetButton("Fire1")) {
                        // reel in
                        // real animation
                        bobberInstance.pulling = true;
                        FishingRod.transform.localRotation = Quaternion.Lerp(FishingRod.transform.localRotation, Quaternion.Euler(new Vector3(reelingAngle, 0, 0)), Time.deltaTime * 3);
                        spinningThing.transform.Rotate(Vector3.right, -1000 * Time.deltaTime, Space.Self);
                    } else {
                        FishingRod.transform.localRotation = Quaternion.Lerp(FishingRod.transform.localRotation, Quaternion.Euler(new Vector3(castedAngle, 0, 0)), Time.deltaTime * 15);
                        if(bobberInstance)
                            bobberInstance.pulling = false;
                    }
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
            Destroy(bobberInstance.gameObject);
            bobberInstance = null;
        }
        CutLine();
    }

    // called from bobber when it enters the finish fishing radius
    // gives catch and releases
    public void Catch(bool caught) {
        // enable this bool to make it so player can keep holdin gleft click and it wont cast again
        justCaught = true;
        Release();

        if(caught) {
            // determine what kind of fish it is
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
        fishModelInstance = Instantiate(fishModels[Random.Range(0, fishModels.Count)], CaughtObject);
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
