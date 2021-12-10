using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController : MonoBehaviour
{

    [Header("References")]
    private Trigger trigger;
    [SerializeField] private Transform ChestModel;
    [SerializeField] private Transform ChestPosition;
    [SerializeField] private AudioClip CrateHitSound;
    [SerializeField] private AudioClip CrateOpenSound;
    private AudioSource CrateAudioSource;
    // serialized reference to crate model

    void Start() {
        trigger = GetComponent<Trigger>();
        CrateAudioSource = GetComponent<AudioSource>();
    }

    void Update() {
        ChestModel.position = Vector3.Lerp(ChestModel.position, ChestPosition.position, Time.deltaTime * 2);
    }

    public void Open() {
        PlayerController.instance.PlaySoundRandPitch(CrateOpenSound);
        trigger.OnTrigger();
        Global.instance.SaveGame();
        Destroy(gameObject);
    }

    public void PlayHitSound() {
        CrateAudioSource.PlayOneShot(CrateHitSound);
    }
}
