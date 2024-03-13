using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class GameTimeManager : Singleton<GameTimeManager>
{
    public SkyboxBlender Blender;
    public Material[] skyBox;
    bool isBlanderAction = false;
    int skyBoxType = 0;

    private void Update()
    {
        if (!isBlanderAction)
        {
            isBlanderAction = true;
            StartCoroutine(BlanderChange_Coroutine(skyBox[skyBoxType+0], skyBox[skyBoxType+1],5));
        }
    }
    IEnumerator BlanderChange_Coroutine(Material _one, Material _two, float _second)
    {
        Blender.skyBox1 = _one;
        Blender.skyBox2 = _two;
        Blender.BindTextures();
        Blender.blend = 0;
        while(Blender.blend <= 1)
        {
            Blender.blend += Time.deltaTime * 0.3f;
            yield return null;
        }
        yield return new WaitForSeconds(_second);
        if (skyBoxType == 2) skyBoxType = 0;
        else skyBoxType++;
        isBlanderAction = false;

    }


}
