using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

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
    // Update is called once per frame
    private void OnDisable()
    {
        VFX_PoolManager.instance.ObjectInPool(gameObject);
    }
}
