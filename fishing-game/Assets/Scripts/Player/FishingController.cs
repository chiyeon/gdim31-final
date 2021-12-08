using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingController : MonoBehaviour
{
    public static FishingController instance;

    [Header("Fishing Variables")]
    private float fishingPower = 0;                 // actively used to determine force to throw bobber at
    [SerializeField] private float maxFishingPower = 5;     // max force
    [SerializeField] private float fishingPowerAcceleration = 1.25f;    // speed at which power inc when mouse held down
    private BobberController bobberInstance;        // reference to the active bobber, null when not casted
    private bool casted = false;                    // whether or not we have casted a bobber (starts when left button let go)
    private bool renderLine = false;                // whether or not to render the fishing line
    private Vector3 linePosition;                   // the second, target position (bobber); seperate var required to smoothly transition in fish catch animation

    [Header("Fishing Rod Angles")]                  // lerp between these for procedural animations
    [SerializeField] private float idleAngle = 0;
    [SerializeField] private float fullPowerAngle = -70;
    [SerializeField] private float castedAngle = 35;
    [SerializeField] private float reelingAngle = 15;
    private bool justCaught = false;                // used to ensure we dont accidentally cast again when we keep holding left click after another ends
    private bool transferringLine = false;          // active when we are switching to fish catching animation

    [Header("Catch Stuffs")]
    private GameObject fishModelInstance;           // fish mode for catch animation
    [SerializeField] private List<GameObject> fishModels;   // all possible fishie models
    [SerializeField] private List<GameObject> itemModels;   // all possible item models
    [SerializeField] private List<GameObject> crates;       // spawns after we catch
    [SerializeField] private List<Item> items;              // all possible items to give;
    [SerializeField] private bool isTutorial = false;
    private int currentCrate = 0;

    [Header("Pulling Stuff")]                       // used to determine whether or not line will break when pulling
    [SerializeField] private float pullDecreaseAmount = 0.35f;
    [SerializeField] private float pullIncreaseAmount = 0.5f;
    [SerializeField] private float pullTimer = 0;
    private int fishInstance = -1;       // make sure we dont get the same fish over and over !

    [Header("References")]
    [SerializeField] private Transform bobberRelease;
    [SerializeField] private GameObject Bobber;
    [SerializeField] private GameObject FishingRod;
    [SerializeField] private GameObject spinningThing;
    [SerializeField] private Collider finishFishingRadius;
    [SerializeField] private Transform CaughtObject;
    [SerializeField] private LineRenderer lineRenderer;
    private Animator animator;
    private PlayerController controller;
    private Coroutine swingAnimationCoroutine;
    private int catchCounter = 0;       // after 6 unsuccessful catches, just give it to da player for free
    
    
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
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            InventoryManager.instance.AddItem(items[0]);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)) {
            InventoryManager.instance.AddItem(items[1]);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)) {
            InventoryManager.instance.AddItem(items[2]);
        }

        // render fishing line when necessary
        if(renderLine) {
            lineRenderer.enabled = true;
            // 0 -> connect to end of rod, 1 -> connect to bobber
            lineRenderer.SetPosition(0, lineRenderer.transform.position);
            lineRenderer.SetPosition(1, linePosition);

            // if we are actively fishing, connect it to bobber, otherwise connect it to the caught fish (for the animation)
            if(fishModelInstance != null) {
                if(transferringLine) {
                    // smoothly move when needed
                    linePosition = Vector3.Lerp(linePosition, fishModelInstance.transform.position, Time.deltaTime * 3);
                } else {
                    // otherwise set it directly
                    linePosition = fishModelInstance.transform.position;
                }
            } else {
                // when bobber is out use that position
                if(bobberInstance) {
                    linePosition = bobberInstance.GetBobberModel().position;
                }
            }
        } else {
            lineRenderer.enabled = false;
        }

        if(!controller.GetNavigating()) {
            if(!controller.GetDisableControls()) {
                if(!casted) {
                    if(!justCaught) {
                        // charging fishing rod to throw again
                        if(Input.GetButton("Fire1")) {
                            fishingPower = Mathf.Lerp(fishingPower, maxFishingPower, Time.deltaTime * fishingPowerAcceleration);
                            float targetRot = (fullPowerAngle - idleAngle) * (fishingPower / maxFishingPower) + idleAngle;
                            FishingRod.transform.localRotation = Quaternion.Lerp(FishingRod.transform.localRotation, Quaternion.Euler(new Vector3(targetRot, 0, 0)), Time.deltaTime * 5);
                        } else {
                            FishingRod.transform.localRotation = Quaternion.Lerp(FishingRod.transform.localRotation, Quaternion.Euler(new Vector3(idleAngle, 0, 0)), Time.deltaTime * 2);
                        }
                        
                        // cast bobber when letting go of left click
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
                    // pulling
                    if(Input.GetButton("Fire1")) {
                        // make bobber move towards player
                        bobberInstance.SetPulling(true);
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
                            bobberInstance.SetPulling(false);
                    }
                    pullTimer = Mathf.Clamp(pullTimer, 0f, 1f);
                }

                // release fishing rod when pressing right click
                if(Input.GetButtonDown("Fire2")) {
                    if(casted)
                        Release();
                }
                // reset just caught
                if(Input.GetButtonUp("Fire1")) {
                    justCaught = false;
                }
            }
        }
    }

    void FixedUpdate() {
        // shake in fixed update to ensure consistency
        if(casted) {
            float shakeAmount = pullTimer - 0.5f;
            shakeAmount = Mathf.Clamp(shakeAmount, 0f, 1f);
            CameraShake.instance.Shake(0.1f, shakeAmount/15f);
        }
    }

    // run only when we switch to navigating mode
    public void OnNavigating() {
        Release();
        justCaught = false;
    }

    // cuts line and resets fishing vars
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

            if(isTutorial) {
                animator.SetTrigger("Fish");
                StartCoroutine(TutorialIncrement());
                return;
            }

            // determine what kind of catch it is
            float det = Random.Range(0f, 1f);

            // debug p7urposes, turn catch coutner < 6!
            if(catchCounter < 6) {
                catchCounter++;
            } else {
                det = 1;    // catch item!
            }

            if(Input.GetKey(KeyCode.R))
                det = 1;

            // start animation, then rest handled by animation events
            if(det <= 0.30f) {                          // 30% chance jumpscare
                animator.SetTrigger("Jumpscare");
            } else if(det <= 0.80) {                    // 50% chance fish
                animator.SetTrigger("Fish");
            } else {                                    // 20% chance item
                // make sure we are in a valid zone to recieve item first.
                // if not, just play jumpscare hehe
                if(controller.GetZone() != 0) {
                    animator.SetTrigger("Item");            // play anim
                    Debug.Log(controller.GetZone()-1);
                    InventoryManager.instance.AddItem(items[controller.GetZone()-1]);       //add item
                    catchCounter = 0;                       // reset catch counter
                    controller.DisableCurrentZoneObject();  // disable zone so we cant repeat
                    if(currentCrate < crates.Count) {
                        Instantiate(crates[currentCrate], BoatController.instance.GetCrateSpawnPosition().position, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));     // create next crate
                        currentCrate++;
                    }
                    // player zone is still set at this point. we must reset it AFTER the model isntance is created
                } else {
                    animator.SetTrigger("Fish");
                }
            }
        } // otherwise do nothing because no catch !
    }

    // run when bobber lands on valid water and sticks
    public void BobberLanded() {
        finishFishingRadius.enabled = true;
    }

    // === CALLED DURING ANIMATION EVENTS ===

    // start of animation, disable controls
    public void StartCatching() {
        controller.SetDisableControls(true);
        renderLine = true;      // reenable, turned off in Release()
        transferringLine = true;
    }

    // run start of idle animation, catch anim over
    public void EndCatching() {
        controller.SetDisableControls(false);
        justCaught = false;
    }

    // spawn temp fish model for catch animation
    public void CreateFishInstance() {
        int newFish = -1;
        do {
            newFish = Random.Range(0, fishModels.Count);
        } while(newFish == fishInstance);
        fishInstance = newFish;
        fishModelInstance = Instantiate(fishModels[fishInstance], CaughtObject);
    }

    // fix vars
    public void StopLineTransfer() {
        transferringLine = false;
    }

    // remove instance
    public void DestroyFishInstance() {
        if(fishModelInstance)
            Destroy(fishModelInstance);
    }

    IEnumerator TutorialIncrement() {
        yield return new WaitForSeconds(7.5f);
        if(isTutorial) {
            GameObject.FindGameObjectWithTag("CatchCounter").GetComponent<TriggerNumberOfCatches>().IncreaseCatch();
        }
    }

    public void CreateItemInstance() {
        fishModelInstance = Instantiate(itemModels[controller.GetZone() - 1], CaughtObject);
        controller.SetZone(0, null);
    }

    public void JumpScare() {
        CameraShake.instance.Shake(0.35f, 0.05f);
        CutLine();
    }

    public void CutLine() {
        renderLine = false;
    }

    public void ResetCatchCounter() {
        catchCounter = 0;
    }
}
