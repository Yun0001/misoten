﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitialize //: MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Screen.SetResolution(1920, 1080, true);

    }

}