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
    private Animator _animator;

    // wasd: 이동
    public float MoveSpeed = 5;

    // spacebar: 점프
    public float JumpPower = 10;
    public int JumpMaxCount = 2;
    public int JumpRemainCount;
    private bool _isJumping = false;
    // private float _gravity = -5;        // 중력 값
    private float _yVelocity = 0f;         // 누적할 중력 변수: y축 속도
    private const float GravityConstant = -9.81f; // 중력 상수

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }

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

    // 구현필요) 앞으로 키 + 마우스를 누르고 있으면 카메라가 보는 방향 앞으로 가야한다.
    private void Move()
    {
        float h = Input.GetAxis("Horizontal"); // 좌우(방향키 왼쪽/오른쪽) 
        float v = Input.GetAxis("Vertical"); // 수직(방향키 위/아래) 

        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();

        dir = Camera.main.transform.TransformDirection(dir);


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