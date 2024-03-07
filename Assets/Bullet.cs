using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public DamageInfo info;
    public float BulletTime = 0;
    public float BulletDestroy = 3;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IHitable>().Hit(info);

            gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        
    }
}
