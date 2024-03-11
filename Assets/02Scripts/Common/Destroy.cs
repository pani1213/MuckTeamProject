using UnityEngine;

public class Destroy : MonoBehaviour
{
    public GameObject[] DestroyGround; // �� �ܰ��� ���̾�

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            PoolingManager.instance.ReturnToPool(other.gameObject);
        }
    }

}
