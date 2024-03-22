using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour
{
    public Text[] texts;
    public float speed;
    public GameObject startPos,endPos, endText = null;

    public List<string> textString;
    private int stringIndex;

    private bool isEnd = false; 
    private void Start()
    {
        for (int i = 0; i < texts.Length; i++) 
            texts[i].text = textString[stringIndex++];   
    }
    private void Update()
    {
        Scroll();
    }

    private void Scroll()
    {
        if (endText != null && endText.transform.localPosition.y >= 30)
        {
            if (Input.anyKey)
                SceneManager.LoadScene(0);
            return;
        }
        for (int i = 0; i < texts.Length; i++)
        {


            texts[i].transform.position += Vector3.up * speed * Time.deltaTime;


            if (texts[i].transform.position.y >= endPos.transform.position.y)
            {
                if (textString.Count - 1 >= stringIndex)
                {

                    texts[i].text = textString[stringIndex];
                    stringIndex++;

                    if (stringIndex == 20)
                    {
                        endText = texts[i].gameObject;
                        isEnd = true;
                    }
                }
                else
                    Debug.Log("stringIndex �Ѿ");




                texts[i].transform.position = startPos.transform.position;

            }


        }
    }
}

