using UnityEngine;

public class PlayerFireAbility : MonoBehaviour
{
    public Animator _animator;
    public GameObject EquipmentPrefab;
    public Transform SwingPosition;
    public int Damage = 10; // ���ݷ�
    public float AttackRange = 2f; // ���� ����
    private GameObject equipmentInstance; // ���� Ȱ��ȭ�� ���� �ν��Ͻ��� ����
    private bool isEquipped = false; // ���� ���� ���¸� �Ǵ��ϴ� ����

    private void Update()
    {
        if (equipmentInstance != null)
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraDown = Camera.main.transform.up * -0.5f;
            Vector3 offset = cameraForward + cameraDown;
            SwingPosition.position = Camera.main.transform.position + offset;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) // 1�� Ű
        {
            // ���Ⱑ ������ ���, �����ϰ� �������� ���� ���·� ����
            if (isEquipped)
            {
                Destroy(equipmentInstance);
                equipmentInstance = null;
                _animator = null;
                isEquipped = false;
            }

            // ���Ⱑ �������� ���� ���, �����ϰ� ������ ���·� ����
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
            // + ���� �ֵθ��� effect���ٰ� �ݶ��̴� ������ ���� �а� Ÿ�� �������� ����(raycast ������ ��ü �˻� �Լ�)
            //���� �ֵθ� �� Ÿ�� ������ Raycast�� �ϰ� + ���� �ݶ��̴� �ν��ؼ� EŰ ������ ���� �� �ֵ���
        }

    }

    void AttackIfInRange()
    {
        // Raycast�� ����Ͽ� ���� ���� �ȿ� �ִ� ���͸� Ž��
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, AttackRange))
        {
            // Ž���� ���Ϳ��� �������� �ִ� �ڵ�
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
