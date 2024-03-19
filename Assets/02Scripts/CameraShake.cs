using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f; // ��鸲 ���� �ð�
    public float shakeMagnitude = 0.1f; // ��鸲�� ����

    private Vector3 originalPos; // ��鸲 ���� ���� ���� ��ġ

    private void Awake()
    {
        // ī�޶��� ���� ���� ��ġ�� �����մϴ�.
        originalPos = transform.localPosition;
    }

    public void Shake()
    {
        // ��鸲 ȿ���� �����մϴ�.
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {

            // ������ �������� ī�޶� ���� �����Դϴ�.
            Vector3 randomPoint = originalPos + Random.insideUnitSphere * shakeMagnitude;

            // Z���� �������� �ʽ��ϴ�(�Ϲ������� ī�޶��� �յ� �̵��� �����ϱ� ����).
            randomPoint.z = originalPos.z;

            transform.localPosition = randomPoint;

            elapsed += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ��ٸ��ϴ�.
        }

        // ��鸲�� ������ ī�޶��� ��ġ�� ������� �����մϴ�.
        transform.localPosition = originalPos;
    }
}
