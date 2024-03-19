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
            GameObject soundObject = new GameObject("Sound"); // ���ο� GameObject ����
            AudioSource audioSource = soundObject.AddComponent<AudioSource>(); // AudioSource �߰�
            audioSource.clip = audioClips[index]; // AudioClip ����
            audioSource.Play(); // �Ҹ� ���

            // �Ҹ��� ����Ǹ� GameObject �ı�
            StartCoroutine(DestroySoundObjectWithDelay(soundObject, audioClips[index].length));
        }
    }

    private IEnumerator DestroySoundObjectWithDelay(GameObject soundObject, float delay)
    {
        yield return new WaitForSeconds(2f);
        AudioSource audioSource = soundObject.GetComponent<AudioSource>();
        audioSource.Stop(); // �Ҹ� ����
        Destroy(soundObject);
    }
}
