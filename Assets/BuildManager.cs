using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildManager : Singleton<BuildManager>
{
    public GameObject[] buildGameobject;
    Dictionary<int, GameObject> buildDic = new Dictionary<int, GameObject>();
    public GameObject currentObj;
    public void InIt()
    {
        for (int i = 0; i < buildGameobject.Length; i++)
            buildDic.Add(int.Parse(buildGameobject[i].name.Substring(0, 4)), buildGameobject[i]);
    }
    public GameObject GetObject(int _id)
    {
        currentObj = buildDic[_id].gameObject;
        currentObj.SetActive(true);
        return currentObj;
    }
    public void ReturnObject()
    {
        if (currentObj != null)
        { 
            currentObj.SetActive(false);
            currentObj = null;
        }
    }

}
