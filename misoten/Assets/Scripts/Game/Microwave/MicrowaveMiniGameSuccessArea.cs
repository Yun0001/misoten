using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrowaveMiniGameSuccessArea : MonoBehaviour {

    public enum EArea
    {
        SuccessArea,
        GreatSuccessArea
    }

    private Dictionary<EArea, int> areaPoint = new Dictionary<EArea, int> {
        { EArea.SuccessArea, 5},        //"Aseets/Scenes/Title.unity"
        { EArea.GreatSuccessArea,2 },  //"Aseets/Scenes/Tutorial.unity"
    };

    public int GetAreaPoint(EArea areaname) => areaPoint[areaname];
}
