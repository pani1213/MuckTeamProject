using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveAbility : MonoBehaviour
{
    public Camera theCamera;
    private float lookSensitivity;          // 마우스의 움직임에 따른 회전 민감도
    private float cameraRotationLimit;
    private float currentCameraRotationX;

    private CharacterController _characterController;

    // wasd: 이동
    public float MoveSpeed = 5;

    // shift: 스태미나

    public float RunSpeed = 10;
    public float Stamina = 100;             // 스태미나
    public const float MaxStamina = 100;    // 최대량
    public float StaminaConsumeSpeed = 33f; // 초당 스태미나 소모량
    public float StaminaChargeSpeed = 50;  // 초당 스태미나 충전량

    // spacebar: 점프

    public float JumpPower = 10;
    public int JumpMaxCount = 2;
    public int JumpRemainCount;
    private bool _isJumping = false;
    // private float _gravity = -5;        // 중력 값
    private float _yVelocity = 0f;         // 누적할 중력 변수: y축 속도
    private const float GravityConstant = -9.81f; // 중력 상수


    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        Cursor.visible = false;

        lookSensitivity = 2f; // 마우스 민감도를 조절하는 값, 적절한 값으로 조정
        cameraRotationLimit = 85f; // 카메라가 위아래로 회전할 수 있는 최대 각도, 85도 정도가 적당
        currentCameraRotationX = 0f; // 초기 카메라 X축 회전값을 0으로 설정

    }


    private void Update()
    {
        Move();
        CameraRotation();       // 마우스 위아래(Y) 움직임
        CharacterRotation();    // 마우스 좌우(X) 움직임

        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.Locked;
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal"); // 좌우(방향키 왼쪽/오른쪽) 
        float v = Input.GetAxis("Vertical"); // 수직(방향키 위/아래) 

        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();

        float speed = MoveSpeed;

        // Shift 누르면 빨리 뛰기
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Stamina -= StaminaConsumeSpeed * Time.deltaTime;
            dir *= RunSpeed;

        }
        else
        {
            Stamina += StaminaChargeSpeed * Time.deltaTime; // 초당 50씩 충전
            dir *= MoveSpeed;
        }

        Stamina = Mathf.Clamp(Stamina, 0, 100); // 값이 넘어가지 않도록

        bool isGrounded()
        {
            float distanceToGround = _characterController.height / 2;
            return Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f);
        }

        if (isGrounded())
        {
            _isJumping = false;
            //_gravity = 0;
            _yVelocity = 0f;
            JumpRemainCount = JumpMaxCount;
        }
        else if (!_characterController.isGrounded && !_isJumping)
        {
            JumpRemainCount = 0;
        }

        // 점프 구현
        if (Input.GetKeyDown(KeyCode.Space) && (_characterController.isGrounded || JumpRemainCount > 0))
        {
            _isJumping = true;
            JumpRemainCount--;

            _yVelocity = JumpPower; // y축에 점프파워 적용
        }

        // 중력 적용
        if (!_characterController.isGrounded)
        {
            _yVelocity += GravityConstant * Time.deltaTime; // 중력 가속도
            dir.y = _yVelocity;
        }


        // 이동하기
        //transform.position += speed * dir * Time.deltaTime;
        _characterController.Move(dir * Time.deltaTime);
    }

    private void CameraRotation()
    {
        // 마우스를 위아래(Y) 움직임을 받아 카메라의 회전을 결정
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;

        // 카메라의 회전을 업데이트하고, 제한 범위 내에 있는지 확인
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        // 카메라의 회전을 적용
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, theCamera.transform.localEulerAngles.y, 0f);
    }

    private void CharacterRotation()
    {
        // 마우스 좌우(X) 움직임을 받아 캐릭터의 회전을 결정
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = Vector3.up * _yRotation * lookSensitivity;

        // 캐릭터의 회전을 적용
        transform.Rotate(_characterRotationY);
    }
}