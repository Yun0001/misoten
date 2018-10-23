using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrilledPoint : MonoBehaviour
{

    public enum EType
    {
        Normal_Nice,
        Normal_Good,
        Hard_Nice,
        Hard_Good,
        Hell
    }

    [SerializeField]
    private EType type;

    private readonly int[] areaPoint = { 1, 2, 1, 4, 2 };

    public int GetPoint() => areaPoint[(int)type];
}
