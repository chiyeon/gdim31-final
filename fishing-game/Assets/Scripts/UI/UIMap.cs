using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMap : MonoBehaviour
{
    public static UIMap instance;

    [Header("References")]
    [SerializeField] private GameObject MapPanel;
    [SerializeField] private RectTransform PlayerDot;
    [SerializeField] private RectTransform ZoneDot;
    [SerializeField] private AudioClip[] OpenMapSounds;
    private Transform player;
    private InventoryManager inventoryManager;
    [SerializeField] List<Transform> zones;

    void Awake() {
        instance = this;
    }
    

    void Start() {
        MapPanel.SetActive(false);            // off by default
        player = PlayerController.instance.transform;
        inventoryManager = InventoryManager.instance;
    }

    void Update() {
        if(Input.GetButtonDown("Map") && !PlayerController.instance.GetDisableControls()) OpenMap();
        if(Input.GetButtonUp("Map")) CloseMap();

        if(player)
            PlayerDot.localPosition = new Vector2(player.position.x * 0.847f, player.position.z * 0.86f);
         
         if(inventoryManager.currentZone != 0) {
            ZoneDot.gameObject.SetActive(true);
            Transform zone = zones[inventoryManager.currentZone - 1];
            ZoneDot.localPosition = new Vector2(zone.position.x * 0.847f, zone.position.z * 0.86f);
            float radius = zone.GetComponent<SphereCollider>().radius * 0.95f; // add buffer so we can be sure it is accurate
            ZoneDot.sizeDelta = new Vector2(radius * 2, radius * 2);
         } else {
            ZoneDot.gameObject.SetActive(false);
         }
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
