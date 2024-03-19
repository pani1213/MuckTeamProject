using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sound  // 컴포넌트 추가 불가능.  MonoBehaviour 상속 안 받아서. 그냥 C# 클래스.
{
    public string name;  // 곡 이름
    public AudioClip clip;  // 곡
}

public class SoundPooling : MonoBehaviour
{
    static public SoundPooling instance; // 자기 자신을 공유 자원으로. static은 씬이 바뀌어도 유지된다.

    private void Awake()  // 객체 생성시 최초 실행 (그래서 싱글톤을 여기서 생성)
    {
        if (instance == null)   // 최초 생성
        {
            instance = this;  // 현재의 자기 자신(인스턴스)를 할당
        }
        else  // 단 하나만 존재하게끔 새로 생긴 Sound Manager 오브젝트 인스턴스일 경우엔 파괴
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
