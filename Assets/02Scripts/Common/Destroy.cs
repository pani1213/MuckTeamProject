using UnityEngine;

public class Destroy : MonoBehaviour
{
    public GameObject[] DestroyGround; // 게임오브젝트 그라운드

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            PoolingManager.instance.ReturnToPool(other.gameObject);
        }
    }

}
