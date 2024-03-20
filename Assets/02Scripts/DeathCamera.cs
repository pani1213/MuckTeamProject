using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathCamera : MonoBehaviour
{
    public Transform target; // 무덤 위치
    public float orbitDistance = 5f; // 카메라가 대상으로부터 떨어질 거리
    public float orbitHeight = 7f; // 카메라의 높이
    public float orbitSpeed = 5f; // 원형 경로를 따라 이동하는 속도

    private float orbitAngle = 0f; // 현재 각도

    void LateUpdate()
    {
        if (target != null && SurvivalGauge.IsPlayerDead)
        {
            // 카메라를 대상 주위로 회전시키기
            OrbitAroundTarget();

            if (Input.anyKey)
            {
                SceneManager.LoadScene("StartScene");
            }
        }
    }

    public void OrbitAroundTarget()
    {
        // 시간에 따라 각도 업데이트
        orbitAngle += orbitSpeed * Time.deltaTime;
        orbitAngle %= 360; // 360도를 넘어가면 0으로 리셋

        // 원형 경로를 계산
        float radian = orbitAngle * Mathf.Deg2Rad; // 각도를 라디안으로 변환
        float x = Mathf.Sin(radian) * orbitDistance;
        float z = Mathf.Cos(radian) * orbitDistance;
        Vector3 orbitPosition = new Vector3(x, orbitHeight, z) + target.position;

        // 카메라 위치 업데이트
        transform.position = orbitPosition;

        // 카메라가 무덤을 바라보도록 설정
        transform.LookAt(target.position);
    }
}
