using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Global : MonoBehaviour
{
    public static Global instance;
    private bool paused = false;

    [SerializeField] private int TextScene = 1;
    [SerializeField] private GameObject PausedScreen;
    [SerializeField] private AudioMixer MasterAudio;

    [SerializeField] private Slider VolumeSlider;
    [SerializeField] private Slider MouseSensitivitySlider;
    [SerializeField] private GameObject FocusPanel;
    private float MouseSensitivity = 130;

    void Awake() {
        if(instance == null) {
            DontDestroyOnLoad(this);
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        AdjustMouseSensitivity(PlayerPrefs.GetFloat("MouseSensitivity", 130));
        AdjustAudio(PlayerPrefs.GetFloat("Volume", 0));

        VolumeSlider.value = PlayerPrefs.GetFloat("Volume", 0);
        MouseSensitivitySlider.value = MouseSensitivity;
    }

    public void SetFPSMouse(bool status) {
        Cursor.visible = !status;
        Cursor.lockState = status ? CursorLockMode.Locked : CursorLockMode.None;
    }

    void Update() {
        if(Input.GetButtonDown("Cancel")) {
            SetFPSMouse(paused);
            paused = !paused;
            if(SceneManager.GetActiveScene().buildIndex != 0 || SceneManager.GetActiveScene().buildIndex != 1 || SceneManager.GetActiveScene().buildIndex != 6 || SceneManager.GetActiveScene().buildIndex != 7)  {
                Time.timeScale = paused ? 0 : 1;
                PausedScreen.SetActive(paused);
            }
        }

        if(Input.GetKeyDown(KeyCode.U)) {
            SaveGame();
        }
        
        if(Input.GetKeyDown(KeyCode.Y)) {
            ClearData();
        }

        FocusPanel.SetActive(!Application.isFocused && Application.platform == RuntimePlatform.WebGLPlayer);
    }

    public void Unpause() {
        paused = false;
        SetFPSMouse(!paused);
        Time.timeScale = 1;
        PausedScreen.SetActive(paused);
    }

    public void LoadScene(int sceneID) {
        SetFPSMouse(false);
        SceneManager.LoadScene(sceneID);
    }

    public void PlayTextScene(string _text, float _duration, int _nextScene) {
        StartCoroutine(IPlayTextScene(_text, _duration, _nextScene));
    }

    IEnumerator IPlayTextScene(string _text, float _duration, int _nextScene) {
        LoadScene(TextScene);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        StoryTextScript.instance.ShowText(_text, _duration, _nextScene);
    }

    public void AdjustAudio(float audio) {
        MasterAudio.SetFloat("Volume", audio);
        PlayerPrefs.SetFloat("Volume", audio);
    }

    public float GetMouseSensitivity() {
        return MouseSensitivity;
    }

    public void AdjustMouseSensitivity(float _MouseSensitivity) {
        MouseSensitivity = _MouseSensitivity;
        PlayerPrefs.SetFloat("MouseSensitivity", _MouseSensitivity);
        if(PlayerController.instance)
            PlayerController.instance.SetMouseSensitivity(_MouseSensitivity);
    }

    public void SaveGame() {
        SaveData data = new SaveData(
            BoatController.instance.transform.position,
            BoatController.instance.transform.rotation,
            InventoryManager.instance.GetHasCursedRod(),
            InventoryManager.instance.GetHasCursedBait(),
            InventoryManager.instance.GetItems().ToArray(),
            InventoryManager.instance.GetPages().ToArray(),
            PlayerController.instance.GetZones()
        );

        if(UINotification.instance)
            UINotification.instance.ShowSaveIcon();

        SaveManager.SaveGame(data);
    }

    public void LoadGame() {
        SaveData data = SaveManager.LoadGame();

        if(data == null)
            return;

        BoatController.instance.transform.position = data.boatPosition;
        BoatController.instance.transform.rotation = data.boatRotation; 
        PlayerController.instance.transform.position = new Vector3(data.boatPosition.x, data.boatPosition.y + 2, data.boatPosition.z);

        InventoryManager.instance.SetHasCursedRod(data.hasCursedRod);
        InventoryManager.instance.SetHasCursedBait(data.hasCursedBait);
        InventoryManager.instance.SetItems(data.items);
        InventoryManager.instance.SetPages(data.pages);

        if(data.pages.Length > 0) {
            Destroy(GameObject.FindGameObjectWithTag("Crate"));
            // destroy crate at the start if we already have the page
        }

        PlayerController.instance.SetZones(data.zones);
    }

    public void ClearData() {
        SaveManager.ClearData();
    }
}
