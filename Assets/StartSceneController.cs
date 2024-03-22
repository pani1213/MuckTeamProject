using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneController : MonoBehaviour
{


    private void Start()
    {
       
        Application.targetFrameRate = 60;
        Screen.SetResolution(1600, 900, false);
    }

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

        if (Input.anyKey)
        {
            SceneManager.LoadScene(1);
        }
    }

}
