using UnityEngine;

public class Destroy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            PoolingManager.instance.ReturnToPool(other.gameObject);
        }
       
    }

}
