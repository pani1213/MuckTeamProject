using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireAbility : MonoBehaviour
{
    private Animator _animator;

    //public Equipment CurrentEquipment; // ���� ����ִ� ����

    public GameObject EquipmentPrefab;
    public Transform SwingPosition;
    public float SwingPower = 20f;
    public int Damage = 10; // ���ݷ�

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SwingEquipment();
        }
    }

    void SwingEquipment()
    {
        if (EquipmentPrefab != null && SwingPosition != null)
        {
            //_animator.SetTrigger("Swing");
            GameObject equipmentInstance = Instantiate(EquipmentPrefab, SwingPosition.position, Quaternion.identity) as GameObject;

            // ������ �ν��Ͻ��� ī�޶� �ٶ󺸴� �������� �ֵη�
            Rigidbody rb = equipmentInstance.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero; // �̹� �����̰� ���� ��츦 ����Ͽ� �ӵ� �ʱ�ȭ
                rb.AddForce(Camera.main.transform.forward * SwingPower, ForceMode.Impulse);
            }

            // ����ĳ������ ����Ͽ� ���� ��� ����
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            // ���̸� �߻�
            // ���̰� �ε��� ����� ������ �޾ƿ�
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                // �������� ���Ϳ��� ���� �� ���� ü�� ��� ��� ����
                IHitable hitObject = hitInfo.collider.GetComponent<IHitable>();
                if (hitObject != null)
                {
                    DamageInfo damageInfo = new DamageInfo(DamageType.Normal, Damage);
                    damageInfo.Position = hitInfo.point;
                    damageInfo.Normal = hitInfo.normal;
                    hitObject.Hit(damageInfo);
                }

                // �ε��� ��ġ�� (�ֵθ���)����Ʈ�� ��ġ
                //HitEffect.gameObject.transform.position = hitInfo.point;
                // ����Ʈ�� �Ĵٺ��� ������ �ε��� ��ġ�� '���� ����'�� ��
                //HitEffect.gameObject.transform.forward = hitInfo.normal;
                //HitEffect.Play();
            }
        }
    }
}
