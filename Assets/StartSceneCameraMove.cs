using Unity.Burst.CompilerServices;
using UnityEngine;

public class StartSceneCameraMove : MonoBehaviour
{
    public float radius = 2f; // ���� ������
    public float speed = 1f; // �̵� �ӵ�

    public GameObject center; // ���� �߽� ��ǥ
    private float angle = 0f; // ���� ����

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
        // ���� ������ �ð��� ���� ������Ŵ
        if (turnDir)
        angle += speed * Time.deltaTime;
        else
        angle -= speed * Time.deltaTime;


        // ���ο� ��ġ ���
        Vector3 newPosition = center.transform.position + new Vector3(Mathf.Cos(angle), center.transform.position.y - hight, Mathf.Sin(angle)) * radius;

        transform.LookAt(center.transform.position);
        // ���ο� ��ġ�� �̵�
        transform.position = newPosition;
    }
}
