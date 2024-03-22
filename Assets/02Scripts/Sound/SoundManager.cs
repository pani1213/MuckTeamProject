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
        
        // BGM AudioSource 설정
        GameObject bgmObject = new GameObject("BGM");
        bgmSource = bgmObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.clip = bgmClip;
        bgmSource.Play();

        // 풀링을 위한 부모 GameObject 생성
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
        GameObject soundObject = GetPooledSoundObject(); // 풀에서 오디오 소스를 가져옴
        AudioSource audioSource = soundObject.GetComponent<AudioSource>(); // AudioSource 가져옴
        AudioClip newClip = GetAudioClip(clipName);
        audioSource.clip = newClip;

        audioSource.Play();

        StartCoroutine(ReturnToPoolWithDelay(soundObject, newClip.length)); // 재생 후 풀에 반환

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