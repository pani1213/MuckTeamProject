using UnityEngine;

public class PlayerMoveLimit : MonoBehaviour
{
    // ���� ��� ����
    public float mapXMin = 0;
    public float mapXMax = 100;
    public float mapZMin = 0;
    public float mapZMax = 100;

    // �ʱ� ��ġ ����
    private Vector3 initialPosition;

    void Start()
    {
        // �ʱ� ��ġ ����
        initialPosition = transform.position;
    }

    void Update()
    {
        // ���� �Է� �ޱ�
        float deltaX = Input.GetAxis("Horizontal");
        float deltaZ = Input.GetAxis("Vertical");

        // ���ο� ��ġ ���
        Vector3 newPosition = transform.position + new Vector3(deltaX, 0, deltaZ);

        // �� ��� ���� ��ġ ����
        newPosition.x = Mathf.Clamp(newPosition.x, mapXMin, mapXMax);
        newPosition.z = Mathf.Clamp(newPosition.z, mapZMin, mapZMax);

        // ĳ���� �̵�
        transform.position = newPosition;
    }
}