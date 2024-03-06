using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DamageType 열거형: 데미지의 종류를 정의합니다. 일반과 크리티컬로 구성됩니다.
public enum DamageType
{
    Normal,     // 일반 데미지
    Critical    // 크리티컬 데미지
}

// DamageInfo 구조체: 데미지 정보를 저장합니다.
public struct DamageInfo
{
    public DamageType DamageType;   // 데미지의 종류 (일반 또는 크리티컬)
    public int Amount;              // 데미지 양
    public Vector3 Position;        // 데미지를 입힌 위치
    public Vector3 Normal;          // 데미지를 입힌 오브젝트의 표면 법선 벡터

    // DamageInfo 생성자: 데미지 정보를 초기화합니다.
    public DamageInfo(DamageType damageType, int amount)
    {
        this.Amount = amount;                     // 데미지 양 설정
        this.DamageType = DamageType.Normal;      // 데미지 종류를 일반으로 설정 (기본값)
        this.Position = Vector3.zero;             // 위치를 원점으로 초기화
        this.Normal = Vector3.zero;               // 법선 벡터를 원점으로 초기화
    }
}
