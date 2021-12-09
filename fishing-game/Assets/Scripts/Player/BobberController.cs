using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobberController : MonoBehaviour
{
    [Header("Bobber data")]
    [SerializeField] private float pullSpeed = 3f;
    [SerializeField] private float minWaitTime = 2f;
    [SerializeField] private float maxWaitTime = 6f;
    private bool touchedRipple = false;     // ensure we only touch one ripple!
    private bool pulling = false;           // is player actively pulling bobber
    
    [SerializeField] private float waterHeight = 87;     // y level of the water object, using collision isn't consistent so ill use math!
    [Header("Catch Data")]
    [SerializeField] private Vector3 bobberCatchPosition = new Vector3(0, -1, 0);
    private bool caught = false;

    [Header("Bobbing Data")]
    [SerializeField] private float baseBobRate = 7.0f;
    [SerializeField] private float pullingBobRate = 12.0f;
    private float bobRate = 7.0f;

    [SerializeField] private float baseBobDistance = 0.25f;
    [SerializeField] private float pullingBobDistance = 0.39f;
    private float bobDistance = 0.25f;

    [Header("References")]
    [SerializeField] private Transform BobberModel;
    [SerializeField] private GameObject BobberRipples;
    [SerializeField] private GameObject BobberImpactParticles;
    private Transform player;
    private Rigidbody rb;
    private Coroutine waitCatchCoroutine;
    private Transform crate;        // any connected crate

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void Start() {
        player = FishingController.instance.transform;  // nab a player reference
    }
    
    // launch bobber in direction, used when initially casting
    public void Launch(float force) {
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
    }

    void Update() {
        // only if we arent on top of a crate
        if(!crate) {
            // if we havent caught anything bob up and down idly
            if(!caught) {
                BobberModel.Translate(BobberModel.up * Time.deltaTime * Mathf.Sin(Time.time * bobRate) * bobDistance);
            } else {
                // otherwise go to the down caught position underwater
                BobberModel.localPosition = Vector3.Lerp(BobberModel.localPosition, bobberCatchPosition, Time.deltaTime * 3);
            }
        }

        // if we have dropped below the water height (we hit the water), freeze position and ensure it is consistent
        if(transform.position.y <= waterHeight && rb.isKinematic == false) {
            Freeze();
            transform.position = new Vector3(transform.position.x, waterHeight, transform.position.z);
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

    // when we hit something
    void Freeze() {
        rb.isKinematic = true;
        FishingController.instance.BobberLanded();
        Destroy(Instantiate(BobberImpactParticles, transform.position, Quaternion.identity), 1);
    }

    public void Catch() {
        caught = true;
        BobberRipples.SetActive(false);
    }

    void OnTriggerEnter(Collider collider) {
        // bumps into radius around player, close enough to catch
        if(collider.gameObject.CompareTag("FinishFishing")) {
            if(crate) {
                crate.GetComponent<CrateController>().Open();
            }
            FishingController.instance.Catch(caught);
        } else if(collider.gameObject.CompareTag("Ripple") && !touchedRipple) {
            // we landed in a ripple, random wait time to bite
            touchedRipple = true;   // ensure we only touch a single ripple!
            waitCatchCoroutine = StartCoroutine(WaitThenCatch(collider.gameObject));
        } else if(collider.gameObject.CompareTag("Crate")) {
            Freeze();
            collider.gameObject.GetComponent<CrateController>().PlayHitSound();
            crate = collider.transform;
            crate.SetParent(transform);
            // play sound
        }
    }

    // waits a random amount of time, then delete the ripple and catch as long as we are still fishing
    IEnumerator WaitThenCatch(GameObject ripple) {
        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        Catch();
        Destroy(ripple);
    }

    public void Remove() {
        if(crate)
            crate.SetParent(null);
        if(waitCatchCoroutine != null)
            StopCoroutine(waitCatchCoroutine);
        Destroy(gameObject);
    }

    public Transform GetBobberModel() {
        return BobberModel;
    }

    public void SetPulling(bool _pulling) {
        pulling = _pulling;
    }
}
