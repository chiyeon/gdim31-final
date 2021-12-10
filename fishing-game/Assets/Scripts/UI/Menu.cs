using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject MenuButtons;
    [SerializeField] private GameObject AboutPage;
    [SerializeField] private GameObject QuitButton;
    [SerializeField] private GameObject PlayButton;
    [SerializeField] private GameObject ResumeButton;
    [SerializeField] private GameObject RestartButton;

    void Start() {
        //QuitButton.SetActive(Application.platform != RuntimePlatform.WebGLPlayer);

        ResumeButton.SetActive(SaveManager.LoadGame() != null);
        RestartButton.SetActive(SaveManager.LoadGame() != null);
        PlayButton.SetActive(SaveManager.LoadGame() == null);
    }

    public void LoadScene(int sceneID) {
        Global.instance.LoadScene(sceneID);
    }

    public void Quit() {
        Application.Quit(0);
    }

    public void LoadMenu() {
        MenuButtons.SetActive(true);
        AboutPage.SetActive(false);
    }

    public void LoadAbout() {
        MenuButtons.SetActive(false);
        AboutPage.SetActive(true);
    }

    public void Play() {
        Global.instance.PlayTextScene("Its a great day to go fishing.", 3, 2);
    }

    public void Resume() {
        Global.instance.PlayTextScene("The water turns red.", 1, 5);
    }

    public void Restart() {
        Global.instance.ClearData();
        Play();
    }
}
