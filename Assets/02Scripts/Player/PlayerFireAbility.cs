using UnityEngine;

public class PlayerFireAbility : MonoBehaviour
{
    public Animator _animator;
    public GameObject EquipmentPrefab;
    public Transform SwingPosition;
    public int Damage = 10; // 공격력
    public float AttackRange = 2f; // 공격 범위
    private GameObject equipmentInstance; // 현재 활성화된 무기 인스턴스를 추적
    private bool isEquipped = false; // 무기 장착 상태를 판단하는 변수

    private void Update()
    {
        if (equipmentInstance != null)
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraDown = Camera.main.transform.up * -0.5f;
            Vector3 offset = cameraForward + cameraDown;
            SwingPosition.position = Camera.main.transform.position + offset;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) // 1번 키
        {
            // 무기가 장착된 경우, 제거하고 장착되지 않은 상태로 변경
            if (isEquipped)
            {
                Destroy(equipmentInstance);
                equipmentInstance = null;
                _animator = null;
                isEquipped = false;
            }

            // 무기가 장착되지 않은 경우, 장착하고 장착된 상태로 변경
            else
            {
                equipmentInstance = Instantiate(EquipmentPrefab, SwingPosition.position, SwingPosition.rotation, SwingPosition);
                _animator = equipmentInstance.GetComponent<Animator>();
                isEquipped = true;
            }
        }

        if (Input.GetMouseButton(0) && _animator != null)
        {
            _animator.SetTrigger("AxeHit");
            AttackIfInRange();
            // + 무기 휘두르는 effect에다가 콜라이더 입혀서 범위 넓게 타격 입히도록 구현(raycast 별개임 물체 검사 함수)
            //도끼 휘두를 때 타격 데미지 Raycast도 하고 + 상자 콜라이더 인식해서 E키 누르면 잡을 수 있도록
        }

    }

    void AttackIfInRange()
    {
        // Raycast를 사용하여 공격 범위 안에 있는 몬스터를 탐지
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, AttackRange))
        {
            // 탐지된 몬스터에게 데미지를 주는 코드
            Monster monster = hit.collider.GetComponent<Monster>();
            if (monster != null)
            {
                DamageInfo damageInfo = new DamageInfo(DamageType.Normal, Damage);
                monster.Hit(damageInfo);
                Debug.Log("hIT");
            }
        }
    }
}
