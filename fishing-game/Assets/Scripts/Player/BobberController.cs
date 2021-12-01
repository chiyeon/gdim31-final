using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobberController : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float pullSpeed = 3f;
    public bool pulling = false;
    
    [SerializeField]
    private float waterHeight = 87;     // y level of the water object, using collision isn't consistent so ill use math!
    [Header("Catch Data")]
    [SerializeField]
    private Vector3 bobberCatchPosition = new Vector3(0, -1, 0);
    private bool caught = false;

    [Header("Bobbing Data")]
    [SerializeField]
    private float baseBobRate = 7.0f;
    [SerializeField]
    private float pullingBobRate = 12.0f;
    private float bobRate = 7.0f;
    [SerializeField]
    private float baseBobDistance = 0.25f;
    [SerializeField]
    private float pullingBobDistance = 0.39f;
    private float bobDistance = 0.25f;

    [Header("References")]
    [SerializeField]
    private Transform BobberModel;
    private Transform player;
    private Rigidbody rb;

    private Coroutine waitCatchCoroutine;
    private bool touchedRipple = false;     // ensure we only touch one ripple!

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void Start() {
        player = FishingController.instance.transform;
    }
    
    // launch bobber in direction
    public void Launch(float force) {
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
    }

    void Update() {

        if(Input.GetKeyDown(KeyCode.C))
            Catch();

        // if we havent caught anything bob up and down idly
        if(!caught) {
            BobberModel.Translate(BobberModel.up * Time.deltaTime * Mathf.Sin(Time.time * bobRate) * bobDistance);
        } else {
            // otherwise go to the down caught position underwater
            BobberModel.localPosition = Vector3.Lerp(BobberModel.localPosition, bobberCatchPosition, Time.deltaTime * 3);
        }

        // if we have dropped below the water height (we hit the water), freeze position and ensure it is consistent
        if(transform.position.y <= waterHeight && rb.isKinematic == false) {
            rb.isKinematic = true;
            transform.position = new Vector3(transform.position.x, waterHeight, transform.position.z);
            FishingController.instance.BobberLanded();
        } else {
            // at that point always face the player
            transform.LookAt(player);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }

        // this is set from the fishing class when holding left click
        // go towards the player
        if(pulling) {
            transform.position += transform.forward * pullSpeed * Time.deltaTime;
            bobRate = pullingBobRate;
            bobDistance = pullingBobDistance;
        } else {
            bobRate = baseBobRate;
            bobDistance = baseBobDistance;
        }
    }

    public void Catch() {
        caught = true;
    }

    void OnTriggerEnter(Collider collider) {
        /*
        if((layerMask.value & (1 << collider.gameObject.layer)) > 0) {
            rb.isKinematic = true;
            transform.LookAt(player);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            FishingController.instance.BobberLanded();
        } else*/
        if(collider.gameObject.CompareTag("FinishFishing")) {
            FishingController.instance.Catch(caught);
        } else if(collider.gameObject.CompareTag("Ripple") && !touchedRipple) {
            // we landed in a ripple, random chance to wait to catch a fish
            touchedRipple = true;
            waitCatchCoroutine = StartCoroutine(WaitThenCatch(collider.gameObject));
        }
    }

    public Transform GetBobberModel() {
        return BobberModel;
    }

    // waits a random amount of time, then delete the ripple and catch as long as we are still fishing
    IEnumerator WaitThenCatch(GameObject ripple) {
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        Catch();
        Destroy(ripple);
    }

    public void Remove() {
        if(waitCatchCoroutine != null)
            StopCoroutine(waitCatchCoroutine);
        Destroy(gameObject);
    }
}
