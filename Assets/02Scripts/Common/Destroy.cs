using UnityEngine;

public class Destroy : MonoBehaviour
{
    public GameObject[] DestroyGround; // ¸Ê ¿Ü°ûÀÇ ·¹ÀÌ¾î

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            PoolingManager.instance.ReturnToPool(other.gameObject);
        }
    }

}
