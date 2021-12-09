using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMap : MonoBehaviour
{
    public static UIMap instance;

    [Header("References")]
    [SerializeField] private GameObject MapPanel;
    [SerializeField] private RectTransform PlayerDot;
    [SerializeField] private AudioClip[] OpenMapSounds;
    private Transform player;

    void Awake() {
        instance = this;
    }
    

    void Start() {
        MapPanel.SetActive(false);            // off by default
        player = PlayerController.instance.transform;
    }

    void Update() {
        if(Input.GetButtonDown("Map") && !PlayerController.instance.GetDisableControls()) OpenMap();
        if(Input.GetButtonUp("Map")) CloseMap();

        if(player)
            PlayerDot.localPosition = new Vector2(player.position.x * 0.847f, player.position.z * 0.86f);
    }

    public void OpenMap() {
        PlayerController.instance.PlaySoundRandPitch(OpenMapSounds[Random.Range(0, OpenMapSounds.Length)]);
        MapPanel.SetActive(true);
        Global.instance.SetFPSMouse(false);        // remove mouse lock
        PlayerController.instance.SetDisableControls(true);      // freeze player
    }

    public void CloseMap() {
        MapPanel.SetActive(false);
        Global.instance.SetFPSMouse(true);        // remove mouse lock
        PlayerController.instance.SetDisableControls(false);      // freeze player
    }
}
