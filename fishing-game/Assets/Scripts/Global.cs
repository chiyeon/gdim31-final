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
    private float MouseSensitivity = 130;

    void Awake() {
        if(instance == null) {
            DontDestroyOnLoad(this);
            instance = this;
        } else {
            Destroy(gameObject);
        }

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
            if(SceneManager.GetActiveScene().buildIndex == 5)  {
                Time.timeScale = paused ? 0 : 1;
                PausedScreen.SetActive(paused);
            }
        }
    }

    public void Unpause() {
        paused = false;
        SetFPSMouse(!paused);
        Time.timeScale = 1;
        PausedScreen.SetActive(paused);
    }

    public void LoadScene(int sceneID) {
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
        PlayerController.instance.SetMouseSensitivity(_MouseSensitivity);
    }
}
