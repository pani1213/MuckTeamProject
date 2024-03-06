using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DamageType ������: �������� ������ �����մϴ�. �Ϲݰ� ũ��Ƽ�÷� �����˴ϴ�.
public enum DamageType
{
    Normal,     // �Ϲ� ������
    Critical    // ũ��Ƽ�� ������
}

// DamageInfo ����ü: ������ ������ �����մϴ�.
public struct DamageInfo
{
    public DamageType DamageType;   // �������� ���� (�Ϲ� �Ǵ� ũ��Ƽ��)
    public int Amount;              // ������ ��
    public Vector3 Position;        // �������� ���� ��ġ
    public Vector3 Normal;          // �������� ���� ������Ʈ�� ǥ�� ���� ����

    // DamageInfo ������: ������ ������ �ʱ�ȭ�մϴ�.
    public DamageInfo(DamageType damageType, int amount)
    {
        this.Amount = amount;                     // ������ �� ����
        this.DamageType = DamageType.Normal;      // ������ ������ �Ϲ����� ���� (�⺻��)
        this.Position = Vector3.zero;             // ��ġ�� �������� �ʱ�ȭ
        this.Normal = Vector3.zero;               // ���� ���͸� �������� �ʱ�ȭ
    }
}
