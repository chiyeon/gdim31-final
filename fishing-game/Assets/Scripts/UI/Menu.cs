using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject MenuButtons;
    [SerializeField] private GameObject AboutPage;
    [SerializeField] private GameObject QuitButton;
    [SerializeField] private GameObject PlayButton;
    [SerializeField] private GameObject ResumeButton;
    [SerializeField] private GameObject RestartButton;
    [SerializeField] private Dropdown dropdown;

    List<Vector2> resolutions = new List<Vector2>();
    bool isFullscreen = false;

    void Start() {
        //QuitButton.SetActive(Application.platform != RuntimePlatform.WebGLPlayer);

        ResumeButton.SetActive(SaveManager.LoadGame() != null);
        RestartButton.SetActive(SaveManager.LoadGame() != null);
        PlayButton.SetActive(SaveManager.LoadGame() == null);

         dropdown.ClearOptions();

         List<string> options = new List<string>();
         
         // im rolling rn HAHA
         options.Add("640 x 480");
         resolutions.Add(new Vector2(640, 480));
         options.Add("800 x 600");
         resolutions.Add(new Vector2(800, 600));
         options.Add("1024 x 768");
         resolutions.Add(new Vector2(1024, 768));
         options.Add("1280 x 960");
         resolutions.Add(new Vector2(1280, 960));
         options.Add("1400 x 1050");
         resolutions.Add(new Vector2(1400, 1050));
         options.Add("1440 x 1080");
         resolutions.Add(new Vector2(1440, 1080));
         options.Add("1920 x 1440");
         resolutions.Add(new Vector2(1920, 1440));

         dropdown.AddOptions(options);
         dropdown.value = 1;
         dropdown.RefreshShownValue();
    }

    public void SetFullscreen(bool full) {
      Screen.fullScreen = full;
      isFullscreen = full;
    }

    public void SetResolution(int i)
    {
      Vector2 res = resolutions[i];
      Screen.SetResolution((int)res.x, (int)res.y, isFullscreen);
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
