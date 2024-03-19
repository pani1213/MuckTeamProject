using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sound  // ������Ʈ �߰� �Ұ���.  MonoBehaviour ��� �� �޾Ƽ�. �׳� C# Ŭ����.
{
    public string name;  // �� �̸�
    public AudioClip clip;  // ��
}

public class SoundPooling : MonoBehaviour
{
    static public SoundPooling instance; // �ڱ� �ڽ��� ���� �ڿ�����. static�� ���� �ٲ� �����ȴ�.

    private void Awake()  // ��ü ������ ���� ���� (�׷��� �̱����� ���⼭ ����)
    {
        if (instance == null)   // ���� ����
        {
            instance = this;  // ������ �ڱ� �ڽ�(�ν��Ͻ�)�� �Ҵ�
        }
        else  // �� �ϳ��� �����ϰԲ� ���� ���� Sound Manager ������Ʈ �ν��Ͻ��� ��쿣 �ı�
            Destroy(this.gameObject);
    }
    public AudioSource AudioSourceBFM;
    public AudioSource[] AudioSourceEffects;

    public string[] PlaySoundName;

    private void Start()
    {
        PlaySoundName = new string[AudioSourceEffects.Length];
    }

}
