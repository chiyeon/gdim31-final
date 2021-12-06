using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventShowText : Event
{
    [SerializeField] private string text;
    [SerializeField] private float duration;
    [SerializeField] private int nextScene;

    public override void OnEvent() {
        Global.instance.PlayTextScene(text, duration, nextScene);
    }
}
