using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public DamageInfo info;
    public float BulletAttackTime = 0;
    public float BulletDestroy = 3;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IHitable>().Hit(info);
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Ground") || other.CompareTag("MapResource"))
        {
            gameObject.SetActive(false);
        }
    }

    public void Start()
    {
        BulletAttackTime = 0;
    }
    public void Update()
    {
        BulletAttackTime += Time.deltaTime;
        if (BulletAttackTime >= BulletDestroy)
        {
            gameObject.SetActive(false);
        }
    }
}
