using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CatchType {
    FISH,
    JUMPSCARE,
    ITEM
}
[System.Serializable]
public class Catch
{
    public Mesh mesh;
    public CatchType catchType;
}
