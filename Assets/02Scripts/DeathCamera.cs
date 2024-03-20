using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathCamera : MonoBehaviour
{
    public Transform target; // ���� ��ġ
    public float orbitDistance = 5f; // ī�޶� ������κ��� ������ �Ÿ�
    public float orbitHeight = 7f; // ī�޶��� ����
    public float orbitSpeed = 5f; // ���� ��θ� ���� �̵��ϴ� �ӵ�

    private float orbitAngle = 0f; // ���� ����

    void LateUpdate()
    {
        if (target != null && SurvivalGauge.IsPlayerDead)
        {
            // ī�޶� ��� ������ ȸ����Ű��
            OrbitAroundTarget();

            if (Input.anyKey)
            {
                SceneManager.LoadScene("StartScene");
            }
        }
    }

    public void OrbitAroundTarget()
    {
        // �ð��� ���� ���� ������Ʈ
        orbitAngle += orbitSpeed * Time.deltaTime;
        orbitAngle %= 360; // 360���� �Ѿ�� 0���� ����

        // ���� ��θ� ���
        float radian = orbitAngle * Mathf.Deg2Rad; // ������ �������� ��ȯ
        float x = Mathf.Sin(radian) * orbitDistance;
        float z = Mathf.Cos(radian) * orbitDistance;
        Vector3 orbitPosition = new Vector3(x, orbitHeight, z) + target.position;

        // ī�޶� ��ġ ������Ʈ
        transform.position = orbitPosition;

        // ī�޶� ������ �ٶ󺸵��� ����
        transform.LookAt(target.position);
    }
}
