using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationOnTurn : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject fisherman;
    [SerializeField] private AudioClip JumpscareSound;
    [SerializeField] private AudioClip ScreamSound;
    bool animate = false;

    void Update() {
        if(!animate) {
            if(fisherman.activeSelf) {
               /*
                RaycastHit hit;
                Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out hit)) {
                    Debug.Log(hit.transform);
                    if(hit.transform == transform) {
                        animate = true;
                        animator.enabled = true;
                        cam.GetComponent<MouseLook>().enabled = false;
                    }
                }*/
                float adj = Mathf.Abs(cam.localEulerAngles.y);
                Debug.Log(adj);
                if ((adj >= 90 && adj <= 270)) {
                  Debug.Log("gotem");
                  animate = true;
                  animator.enabled = true;
                  cam.GetComponent<MouseLook>().enabled = false;
                }
            }
        } else {
            cam.localRotation = Quaternion.Lerp(cam.localRotation, Quaternion.Euler(new Vector3(9, 180, 0)), Time.deltaTime * 10);
        }
    }

    public void Shake() {
        GetComponent<AudioSource>().PlayOneShot(JumpscareSound);
        CameraShake.instance.Shake(0.2f, 0.03f);
    }

    public void NextScene() {
        Global.instance.PlayTextScene("It fishes at night.", 3, 5);
    }
}
