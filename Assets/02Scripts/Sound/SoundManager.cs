using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] audioClips;

    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlayAudio(int index)
    {
        if (index < audioClips.Length)
        {
            GameObject soundObject = new GameObject("Sound"); // 새로운 GameObject 생성
            AudioSource audioSource = soundObject.AddComponent<AudioSource>(); // AudioSource 추가
            audioSource.clip = audioClips[index]; // AudioClip 설정
            audioSource.Play(); // 소리 재생

            // 소리가 종료되면 GameObject 파괴
            StartCoroutine(DestroySoundObjectWithDelay(soundObject, audioClips[index].length));
        }
    }

    private IEnumerator DestroySoundObjectWithDelay(GameObject soundObject, float delay)
    {
        yield return new WaitForSeconds(2f);
        AudioSource audioSource = soundObject.GetComponent<AudioSource>();
        audioSource.Stop(); // 소리 중지
        Destroy(soundObject);
    }
}
