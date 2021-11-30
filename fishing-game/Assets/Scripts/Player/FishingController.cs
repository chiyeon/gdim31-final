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

    [Header("Catch Database")]
    public List<Catch> fishCatches;
    public List<Catch> jumpscareCatches;
    public List<Catch> itemCatches;


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
    
    
    void Awake() {
        instance = this;
    }

    void Start() {
        controller = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        finishFishingRadius.enabled = false;
    }

    void Update() {
        if(controller.GetDisableControls())
            return;
        if(!controller.GetNavigating()) {
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
    }

    // called from bobber when it enters the finish fishing radius
    // gives catch and releases
    public void Catch(bool caught) {
        // enable this bool to make it so player can keep holdin gleft click and it wont cast again
        justCaught = true;
        Release();

        if(caught) {
            Debug.Log("caught somethign!");
            // determine what kind of fish it is
            animator.SetTrigger("Jumpscare");
            // do stuff for us catching fish
        } // otherwise do nothing because no catch !
    }

    // run when bobber lands on valid water and sticks
    public void BobberLanded() {
        finishFishingRadius.enabled = true;
    }

    public void StartCatching() {
        controller.SetDisableControls(true);
    }

    public void EndCatching() {
        controller.SetDisableControls(false);
        justCaught = false;
    }

    public void JumpScare() {
        CameraShake.instance.Shake(0.35f, 0.05f);
    }
}
