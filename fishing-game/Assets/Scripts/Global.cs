using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Global : MonoBehaviour
{
    public static Global instance;
    private bool paused = false;

    void Awake() {
        if(instance == null) {
            DontDestroyOnLoad(this);
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void SetFPSMouse(bool status) {
        Cursor.visible = !status;
        Cursor.lockState = status ? CursorLockMode.Locked : CursorLockMode.None;
    }

    void Update() {
        if(Input.GetButtonDown("Cancel")) {
            SetFPSMouse(paused);
            paused = !paused;
        }
    }

    public void LoadScene(int sceneID) {
        SceneManager.LoadScene(sceneID);
    }
}
