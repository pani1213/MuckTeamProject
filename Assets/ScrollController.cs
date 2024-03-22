using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour
{
    public Text[] texts;
    public float speed;
    public GameObject startPos,endPos, endText  = null;

    public List<string> textString;
    
    private int stringIndex;
    private bool isEnd = false;
    private void Start()
    {
        for(int i = 0; i < texts.Length; i++)
            texts[i].text = textString[stringIndex++];
    }
    public void Update()
    {
        scroll();
    }
    private void scroll()
    {
        if (endText != null && endText.transform.localPosition.y >= 30)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif


            Debug.Log(0);
            return;
        }
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].transform.position += Vector3.up * speed * Time.deltaTime;

            if (texts[i].transform.position.y >= endPos.transform.position.y)
            {
                texts[i].transform.position = startPos.transform.position;

               if (textString.Count - 1 >= stringIndex)
               {
                   texts[i].text = textString[stringIndex];
                   stringIndex++;
               
                   if (stringIndex == 15)
                   {
                       endText = texts[i].gameObject;
                       isEnd = true;
                   }
               }
               
            }
           
        }
    }

}