using Unity.Burst.CompilerServices;
using UnityEngine;

public class StartSceneCameraMove : MonoBehaviour
{
    public float radius = 2f; // 원의 반지름
    public float speed = 1f; // 이동 속도

    public GameObject center; // 원의 중심 좌표
    private float angle = 0f; // 현재 각도

    public float hight;

    private float coolTime =0;
    private int randomNum = 10;

    private bool turnDir = true;

    void Update()
    {
        coolTime += Time.deltaTime;

        if (coolTime > randomNum)
        {
            coolTime = 0;
            turnDir = !turnDir;
        }
        // 현재 각도를 시간에 따라 증가시킴
        if (turnDir)
        angle += speed * Time.deltaTime;
        else
        angle -= speed * Time.deltaTime;


        // 새로운 위치 계산
        Vector3 newPosition = center.transform.position + new Vector3(Mathf.Cos(angle), center.transform.position.y - hight, Mathf.Sin(angle)) * radius;

        transform.LookAt(center.transform.position);
        // 새로운 위치로 이동
        transform.position = newPosition;
    }
}
