using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using System.Linq;

public class CheckClock : MonoBehaviour {

    [SerializeField]
    private GameObject MicrowaveGage;

    private MicrowaveGage microwaveGage_cs;

    private Microwave microwave_cs;

    private GameObject[] hitObj = Enumerable.Repeat<GameObject>(null, 2).ToArray();

    private void Awake()
    {
        microwave_cs= GameObject.Find("microwave1").GetComponent<Microwave>();
        microwaveGage_cs = MicrowaveGage.GetComponent<MicrowaveGage>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // グレートエリア
        if (collision.tag == "MicrowaveGreatSuccessArea")
        {
            hitObj[0] = collision.gameObject;
        }
        // 通常エリア
        if (collision.tag == "MicrowaveSuccessArea")
        {
            hitObj[1] = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // グレートエリア
        if (collision.tag == "MicrowaveGreatSuccessArea")
        {
            hitObj[0] = null;
        }
        // 通常エリア
        if (collision.tag == "MicrowaveSuccessArea")
        {
            hitObj[1] = null;
        }
    }

    public void DecisionArea()
    {
        for (int i = 0; i < hitObj.Length; i++)
        {
            if (hitObj[i] != null)
            {
                if (i == 0)
                {
                    microwave_cs.AddEatoyPoint(0,hitObj[i].GetComponent<MicrowaveMiniGameSuccessArea>().GetAreaPoint(MicrowaveMiniGameSuccessArea.EArea.GreatSuccessArea));
                    microwaveGage_cs.DisplayPoint(hitObj[i].GetComponent<MicrowaveMiniGameSuccessArea>().GetAreaPoint(MicrowaveMiniGameSuccessArea.EArea.GreatSuccessArea));
                }
                else if (i == 1)
                {
                    microwave_cs.AddEatoyPoint(1,hitObj[i].GetComponent<MicrowaveMiniGameSuccessArea>().GetAreaPoint(MicrowaveMiniGameSuccessArea.EArea.SuccessArea));
                    microwaveGage_cs.DisplayPoint(hitObj[i].GetComponent<MicrowaveMiniGameSuccessArea>().GetAreaPoint(MicrowaveMiniGameSuccessArea.EArea.SuccessArea));

                }
                microwave_cs.AddChain();
                microwaveGage_cs.DecisionSuceesAreaPosition();
                return;
            }
        }

        // エリア外
        microwaveGage_cs.ChinMiss();
        microwave_cs.ResetChain();
    }
}
