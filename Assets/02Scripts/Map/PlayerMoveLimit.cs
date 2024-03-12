using UnityEngine;

public class PlayerMoveLimit : MonoBehaviour
{
    // 맵의 경계 설정
    public float mapXMin = 0;
    public float mapXMax = 100;
    public float mapZMin = 0;
    public float mapZMax = 100;

    // 초기 위치 설정
    private Vector3 initialPosition;

    void Start()
    {
        // 초기 위치 저장
        initialPosition = transform.position;
    }

    void Update()
    {
        // 변위 입력 받기
        float deltaX = Input.GetAxis("Horizontal");
        float deltaZ = Input.GetAxis("Vertical");

        // 새로운 위치 계산
        Vector3 newPosition = transform.position + new Vector3(deltaX, 0, deltaZ);

        // 맵 경계 내에 위치 조정
        newPosition.x = Mathf.Clamp(newPosition.x, mapXMin, mapXMax);
        newPosition.z = Mathf.Clamp(newPosition.z, mapZMin, mapZMax);

        // 캐릭터 이동
        transform.position = newPosition;
    }
}