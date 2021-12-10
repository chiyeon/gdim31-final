using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    void Start() {
        Global.instance.ClearData();
    }

    public void Quit() {
        Application.Quit(0);
    }

    public void MainMenu() {
        Global.instance.LoadScene(0);
    }
}
