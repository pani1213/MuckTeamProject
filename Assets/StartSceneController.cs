using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneController : MonoBehaviour
{

    public Text press_Text_UI;
    string pressText = "Press Ani Key";
    int type = 0;
    
    private float cooltime;
    private void Update()
    {
        cooltime += Time.deltaTime;

        if (cooltime > 1)
        {
            press_Text_UI.text += ".";
            type++;
            if (type > 3)
            { 
                press_Text_UI.text = pressText;
                type = 0;
            }
            Debug.Log(0);
            cooltime = 0;
        }


    }

}
