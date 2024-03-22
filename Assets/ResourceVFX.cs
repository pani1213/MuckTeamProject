using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceVFX : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("off", 1f);   
    }

    void off()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        VFX_PoolManager.instance.ObjectInPool(gameObject);
    }

}
