using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveAbility : MonoBehaviour
{
    public Camera theCamera;
    private float lookSensitivity = 2f;          // 마우스의 움직임에 따른 회전 민감도
    private float cameraRotationLimit = 35f;
    private float currentCameraRotationX = 0f;

    private CharacterController _characterController;
    private Animator _animator;
    // wasd: 이동
    public float MoveSpeed = 5;
    // spacebar: 점프
    public float JumpPower = 10;
    public int JumpMaxCount = 1;
    public int JumpRemainCount;
    private bool _isJumping = false;
    // private float _gravity = -5;        // 중력 값
    private float _yVelocity = 0f;         // 누적할 중력 변수: y축 속도
    private const float GravityConstant = -15.81f; // 중력 상수

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        Move();
        if (Cursor.lockState == CursorLockMode.Locked)
        { 
            CameraRotation();       // 마우스 위아래(Y) 움직임
            CharacterRotation();   // 마우스 좌우(X) 움직임
        }
    }
    private void Move()
    {
        float h = Input.GetAxis("Horizontal"); // 좌우(방향키 왼쪽/오른쪽) 
        float v = Input.GetAxis("Vertical"); // 수직(방향키 위/아래) 

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0; // 수직 이동을 제거하여 순수한 수평 이동만을 고려합니다.
        cameraRight.y = 0;
        cameraForward.Normalize(); // 정규화를 통해 방향만을 유지하며, 크기는 1로 만듭니다.
        cameraRight.Normalize();

        Vector3 dir = transform.right * h + transform.forward * v;
        dir.Normalize();

       // dir = Camera.main.transform.TransformDirection(dir);

        // 접지 확인 함수
        bool isGrounded = Physics.Raycast(transform.position, -Vector3.up, _characterController.height / 2 + 0.1f);

        if (isGrounded && _yVelocity < 0)
        {
            _isJumping = false;
            _yVelocity = -0.5f; // 접지 상태에서는 약간의 중력을 적용하여 플레이어가 바닥에 밀착되도록 함
            JumpRemainCount = JumpMaxCount;
        }
        else
        {
            _yVelocity += GravityConstant * Time.deltaTime; // 중력 가속도
        }

        // 점프 구현
        if (Input.GetKeyDown(KeyCode.Space) && JumpRemainCount > 0) 
        {
            _yVelocity = JumpPower; // y축에 점프파워 적용
            _isJumping = true;
            JumpRemainCount--;
        }

        // 최종 이동 벡터에 y축 속도를 추가
        Vector3 velocity = dir * MoveSpeed + Vector3.up * _yVelocity;
        // 이동하기
        _characterController.Move(velocity * Time.deltaTime);

        // 캐릭터의 회전을 이동 방향에 맞춥니다.
        if (dir != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(dir, Vector3.up);
            _characterController.transform.rotation = Quaternion.RotateTowards(_characterController.transform.rotation, toRotation, lookSensitivity * Time.deltaTime);
        }
    }

    private void CameraRotation()
    {
        // 마우스의 상하 움직임에 따라 카메라 회전
        float _xRotation = Input.GetAxisRaw("Mouse Y") * lookSensitivity;
        currentCameraRotationX -= _xRotation;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        // 카메라의 x축 회전을 적용
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, theCamera.transform.localEulerAngles.y, 0f);
    }

    private void CharacterRotation()
    {
        // 마우스의 좌우 움직임에 따라 캐릭터 회전
        float _yRotation = Input.GetAxisRaw("Mouse X") * lookSensitivity;
        Vector3 currentRotation = _characterController.transform.eulerAngles;
        float newYRotation = currentRotation.y + _yRotation;
        _characterController.transform.eulerAngles = new Vector3(0, newYRotation, 0);
        
    }
}