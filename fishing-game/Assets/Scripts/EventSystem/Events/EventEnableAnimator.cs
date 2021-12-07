using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventEnableAnimator : Event
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool isEnabled;

    public override void OnEvent() {
        animator.enabled = isEnabled;
    }
}
