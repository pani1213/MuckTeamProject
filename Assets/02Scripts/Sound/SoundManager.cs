using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioClip bgmClip;

    public static SoundManager instance;

    public AudioSource bgmSource;
    private GameObject soundPoolParent;
    private Queue<GameObject> soundObjectPool = new Queue<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);
        
        // BGM AudioSource ����
        GameObject bgmObject = new GameObject("BGM");
        bgmSource = bgmObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.clip = bgmClip;
        bgmSource.Play();

        // Ǯ���� ���� �θ� GameObject ����
        soundPoolParent = new GameObject("SoundPool");
    }

    public AudioClip GetAudioClip(string clipname)
    {
        for (int i = 0; i < audioClips.Length; i++)
        {
            if (audioClips[i].name == clipname)
                return audioClips[i];
        }
        return null;
    }
    public void PlayAudio(string clipName)
    {
        GameObject soundObject = GetPooledSoundObject(); // Ǯ���� ����� �ҽ��� ������
        AudioSource audioSource = soundObject.GetComponent<AudioSource>(); // AudioSource ������
        AudioClip newClip = GetAudioClip(clipName);
        audioSource.clip = newClip;

        audioSource.Play();

        StartCoroutine(ReturnToPoolWithDelay(soundObject, newClip.length)); // ��� �� Ǯ�� ��ȯ

    }

    private GameObject GetPooledSoundObject()
    {
        if (soundObjectPool.Count > 0)
        {
            GameObject pooledSoundObject = soundObjectPool.Dequeue();
            pooledSoundObject.SetActive(true);
            return pooledSoundObject;
        }
        else
        {
            GameObject soundObject = new GameObject("Sound");
            soundObject.transform.SetParent(soundPoolParent.transform);
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            return soundObject;
        }
    }

    private IEnumerator ReturnToPoolWithDelay(GameObject soundObject, float audioLength)
    {
        yield return new WaitForSeconds(audioLength);
        soundObject.SetActive(false);
        soundObjectPool.Enqueue(soundObject);
    }
}