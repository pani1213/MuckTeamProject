using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemUseAbility : MonoBehaviour
{
    private Animator _animator;

    //public Equipment CurrentEquipment; // ���� ����ִ� ����

    public GameObject EquipmentPrefab;
    public Transform SwingPosition;
    public int Damage = 10; // ���ݷ�
    private GameObject equipmentInstance; // ���� Ȱ��ȭ�� ���� �ν��Ͻ��� ����

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

    // + ���� �ֵθ��� effect���ٰ� �ݶ��̴� ������ ���� �а� Ÿ�� �������� ����(raycast ������ ��ü �˻� �Լ�)
    //���� �ֵθ� �� Ÿ�� ������ Raycast�� �ϰ� + ���� �ݶ��̴� �ν��ؼ� EŰ ������ ���� �� �ֵ���
    void SwingEquipment()
    {
        if (_animator != null)
        {
            equipmentInstance = Instantiate(EquipmentPrefab, SwingPosition.position, Quaternion.identity);
            _animator.SetTrigger("AxeHitLeftTop");
            // -> AddTorque���� _animator �Ἥ �����̵��� 
            // ���� �밢�� 4���� �ִϸ����� �����Ͽ� �� ���̽���� ���

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
