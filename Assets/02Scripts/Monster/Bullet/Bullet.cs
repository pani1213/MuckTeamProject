using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public DamageInfo info;
    public float BulletAttackTime = 0;
    public float BulletDestroy = 3;
    public BulletEffect _bulletEffect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IHitable>().Hit(info);
            _bulletEffect.CreateBulletEffect(transform.position);
            gameObject.SetActive(false);
            SoundManager.instance.PlayAudio("EffectDestroy");
        }
        else if (other.CompareTag("Ground") || other.CompareTag("MapResource"))
        {
            _bulletEffect.CreateBulletEffect(transform.position);
            gameObject.SetActive(false);
            SoundManager.instance.PlayAudio("EffectDestroy");
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
