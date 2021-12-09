using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fisherman : MonoBehaviour
{
    public static Fisherman instance;

    [SerializeField] private Waypoint currentWaypoint;
    private float moveSpeed = 5.0f;
    private float turnSpeed;
    [SerializeField] private float PatrolMoveSpeed = 5.0f;
    [SerializeField] private float ChaseMoveSpeed = 8.0f;
    [SerializeField] private float PatrolTurnSpeed = 1f;
    [SerializeField] private float ChaseTurnSpeed = 5f;
    [SerializeField] private LookAtTarget headLookAt;
    [SerializeField] Transform BoatModel;
    [SerializeField] private float DetectionRadius = 10.0f;
    [SerializeField] private AudioSource HuntingAudioSource;
    private bool chasingPlayer = false;
    private Transform target;
    private PlayerController playerController;
    private Animator animator;
    private Coroutine GetNewWaypointCoroutine;
    private bool playMusic = false;

    void Awake() {
        instance = this;
    }

    void Start() {
        playerController = PlayerController.instance;
        animator = GetComponent<Animator>();
    }

    void Update() {
        if(playMusic) {
            if(!HuntingAudioSource.isPlaying) {
                HuntingAudioSource.Play();
            }
            if(chasingPlayer) {
                if(!HuntingAudioSource.isPlaying) {
                    HuntingAudioSource.volume = 1;
                } else {
                    HuntingAudioSource.volume = Mathf.Lerp(HuntingAudioSource.volume, 1, Time.deltaTime);
                }
            } else {
                HuntingAudioSource.volume = Mathf.Lerp(HuntingAudioSource.volume, 0.15f, Time.deltaTime);
            }
        } else {
            HuntingAudioSource.volume = Mathf.MoveTowards(HuntingAudioSource.volume, 0, Time.deltaTime * 0.1f);
            if(HuntingAudioSource.volume <= 0) {
                HuntingAudioSource.Stop();
            }
        }

        if(target) {
            float distance = Vector3.Distance(target.position, transform.position);
            if(distance > DetectionRadius) {
                Vector3 dir = (target.position - transform.position).normalized;
                Quaternion targetRot = Quaternion.LookRotation(dir);
                targetRot.z = 0;
                targetRot.x = 0;

                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * turnSpeed);
                transform.position += transform.forward * Time.deltaTime * moveSpeed;

                transform.position = new Vector3(transform.position.x, 87, transform.position.z);
            } else {
                if(chasingPlayer) {
                    Debug.Log("player died");
                } else {
                    Debug.Log("Reach current waypoint. Finding new one!");
                    GetNewWaypointCoroutine = StartCoroutine(GetNewWaypoint());
                }
            }
        }
    }

    public void SetPlayer(bool justLocation = false) {
        // tracking player

        target = playerController.transform;
        chasingPlayer = true;
        playMusic = true;
        if(justLocation) {
            GameObject empty = new GameObject();
            empty.transform.position = playerController.transform.position;
            target = empty.transform;
            chasingPlayer = false;
        }
        headLookAt.target = target;
        turnSpeed = ChaseTurnSpeed;
        moveSpeed = ChaseMoveSpeed;
        

        if(GetNewWaypointCoroutine != null) {
            transform.LookAt(playerController.transform);
            StopCoroutine(GetNewWaypointCoroutine);
            animator.SetBool("Rotate", false);
        }
    }
    
    public void ClearPlayer() {
        Debug.Log("clearing player");
        if(chasingPlayer) {
            Transform empty = new GameObject().transform;
            empty.position = target.position;
            target = empty;

            chasingPlayer = false;
            
            // will now automatically go where player last was
        }
    }

    public void SetTargetWaypoint(Waypoint waypoint) {
        playMusic = false;
        // only clear music when we start patrolling normally again

        moveSpeed = PatrolMoveSpeed;
        turnSpeed = PatrolTurnSpeed;

        currentWaypoint = waypoint;
        target = waypoint.transform;
        headLookAt.target = target;
    }

    IEnumerator TravelNewWaypointCoroutine(Waypoint waypoint) {
        BoatModel.GetComponent<Bob>().enabled = false;
        
        while(BoatModel.localPosition.y > -19) {
            BoatModel.localPosition = Vector3.Lerp(BoatModel.localPosition, new Vector3(0, -20, 0), Time.deltaTime * 0.75f);
            yield return null;
        }

        transform.position = waypoint.transform.position;

        while(BoatModel.localPosition.y < -0.1f) {
            BoatModel.localPosition = Vector3.Lerp(BoatModel.localPosition, Vector3.zero, Time.deltaTime * 0.75f);
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);

        BoatModel.GetComponent<Bob>().enabled = true;
        SetTargetWaypoint(waypoint.GetNextWaypoint());
    }

    IEnumerator GetNewWaypoint() {
        target = null;
        Debug.Log("getting new waypoint");
        
        yield return new WaitForSeconds(3f);
        animator.SetBool("Rotate", true);
        yield return new WaitForSeconds(6f);
        animator.SetBool("Rotate", false);
        SetTargetWaypoint(currentWaypoint.GetNextWaypoint());
    }

    public void TravelNewWaypoint(Waypoint waypoint) {
        StartCoroutine(TravelNewWaypointCoroutine(waypoint));
    }
}
