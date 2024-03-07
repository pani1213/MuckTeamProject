using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireAbility : MonoBehaviour
{
    private Animator _animator;

    //public Equipment CurrentEquipment; // 현재 들고있는 도구

    public GameObject EquipmentPrefab;
    public Transform SwingPosition;
    public float SwingPower = 20f;
    public int Damage = 10; // 공격력

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

            // 생성된 인스턴스를 카메라가 바라보는 방향으로 휘두룸
            Rigidbody rb = equipmentInstance.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero; // 이미 움직이고 있을 경우를 대비하여 속도 초기화
                rb.AddForce(Camera.main.transform.forward * SwingPower, ForceMode.Impulse);
            }

            // 레이캐스팅을 사용하여 공격 대상 감지
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            // 레이를 발사
            // 레이가 부딪힌 대상의 정보를 받아옴
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                // 레이저를 몬스터에게 맞출 시 몬스터 체력 닳는 기능 구현
                IHitable hitObject = hitInfo.collider.GetComponent<IHitable>();
                if (hitObject != null)
                {
                    DamageInfo damageInfo = new DamageInfo(DamageType.Normal, Damage);
                    damageInfo.Position = hitInfo.point;
                    damageInfo.Normal = hitInfo.normal;
                    hitObject.Hit(damageInfo);
                }

                // 부딪힌 위치에 (휘두르는)이펙트를 위치
                //HitEffect.gameObject.transform.position = hitInfo.point;
                // 이펙트가 쳐다보는 방향을 부딪힌 위치의 '법선 벡터'로 함
                //HitEffect.gameObject.transform.forward = hitInfo.normal;
                //HitEffect.Play();
            }
        }
    }
}
