using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventChangeMaterial : Event
{
    [SerializeField] private SkinnedMeshRenderer target;
    [SerializeField] private Material material;

    public override void OnEvent() {
        target.material = material;
    }
}
