using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpscareEnding : MonoBehaviour
{
    public GameObject menu;
    public void Scare() {
        GetComponent<AudioSource>().Play();
        CameraShake.instance.Shake(0.5f, 0.1f);
    }

    public void PlayAgain() {
        Global.instance.LoadScene(5);
    }

    public void Menu() {
        Global.instance.LoadScene(0);
    }

    public void ShowMenu() {
        menu.SetActive(true);
    }
}
