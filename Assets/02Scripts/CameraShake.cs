using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f; // 흔들림 지속 시간
    public float shakeMagnitude = 0.1f; // 흔들림의 강도

    private Vector3 originalPos; // 흔들림 시작 전의 원래 위치

    private void Awake()
    {
        // 카메라의 원래 로컬 위치를 저장합니다.
        originalPos = transform.localPosition;
    }

    public void Shake()
    {
        // 흔들림 효과를 시작합니다.
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {

            // 임의의 방향으로 카메라를 조금 움직입니다.
            Vector3 randomPoint = originalPos + Random.insideUnitSphere * shakeMagnitude;

            // Z축은 변경하지 않습니다(일반적으로 카메라의 앞뒤 이동을 방지하기 위함).
            randomPoint.z = originalPos.z;

            transform.localPosition = randomPoint;

            elapsed += Time.deltaTime;
            yield return null; // 다음 프레임까지 기다립니다.
        }

        // 흔들림이 끝나면 카메라의 위치를 원래대로 복원합니다.
        transform.localPosition = originalPos;
    }
}
