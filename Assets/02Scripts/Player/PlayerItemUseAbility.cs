using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemUseAbility : MonoBehaviour
{
    private Animator _animator;

    //public Equipment CurrentEquipment; // 현재 들고있는 도구

    public GameObject EquipmentPrefab;
    public Transform SwingPosition;
    public int Damage = 10; // 공격력
    private GameObject equipmentInstance; // 현재 활성화된 무기 인스턴스를 추적

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator component not found in children.");
        }
    }

    private void Update()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraDown = Camera.main.transform.up * -0.5f;
        Vector3 offset = cameraForward + cameraDown;

        SwingPosition.position = Camera.main.transform.position + offset;

        if (Input.GetMouseButtonDown(0) && equipmentInstance == null)
        {
            SwingEquipment();
        }
    }

    // + 무기 휘두르는 effect에다가 콜라이더 입혀서 범위 넓게 타격 입히도록 구현(raycast 별개임 물체 검사 함수)
    //도끼 휘두를 때 타격 데미지 Raycast도 하고 + 상자 콜라이더 인식해서 E키 누르면 잡을 수 있도록
    void SwingEquipment()
    {
        if (_animator != null)
        {
            equipmentInstance = Instantiate(EquipmentPrefab, SwingPosition.position, Quaternion.identity);
            _animator.SetTrigger("AxeHitLeftTop");
            // -> AddTorque말고 _animator 써서 움직이도록 
            // 방향 대각선 4방향 애니메이터 구현하여 각 케이스대로 사용

            AttackWithRaycast();
        }

        else
        {
            Debug.LogError("Animator is null. Cannot set trigger.");
        }
    }

    void AttackWithRaycast()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            IHitable hitObject = hitInfo.collider.GetComponent<IHitable>();
            if (hitObject != null)
            {
                DamageInfo damageInfo = new DamageInfo(DamageType.Normal, Damage);
                hitObject.Hit(damageInfo);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(equipmentInstance);
        equipmentInstance = null;
    }
}
