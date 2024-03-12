using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMoveAbility : MonoBehaviour
{
    public static PlayerMoveAbility Instance { get; private set; }
    public SurvivalGauge survivalGauge;

    public Camera theCamera;
    private float lookSensitivity = 2f;          // ���콺�� �����ӿ� ���� ȸ�� �ΰ���
    public float cameraRotationLimit = 35f;
    private float currentCameraRotationX = 0f;

    private CharacterController _characterController;
    private Animator _animator;

    // wasd: �̵�
    public float MoveSpeed = 5;
    // spacebar: ����
    public float JumpPower = 10;
    public int JumpMaxCount = 1;
    public int JumpRemainCount;
    private bool _isJumping = false;
    // private float _gravity = -5;        // �߷� ��
    private float _yVelocity = 0f;         // ������ �߷� ����: y�� �ӵ�
    private const float GravityConstant = -20f; // �߷� ���
    public float jumpStaminaCost = 10f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        survivalGauge = GetComponent<SurvivalGauge>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
            Move();
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                CameraRotation();       // ���콺 ���Ʒ�(Y) ������
                CharacterRotation();   // ���콺 �¿�(X) ������
            }
        ClampPlayerPosition();

    }
    private void Move()
    {
        float h = Input.GetAxis("Horizontal"); // �¿�(����Ű ����/������) 
        float v = Input.GetAxis("Vertical"); // ����(����Ű ��/�Ʒ�) 

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0; // ���� �̵��� �����Ͽ� ������ ���� �̵����� ����մϴ�.
        cameraRight.y = 0;
        cameraForward.Normalize(); // ����ȭ�� ���� ���⸸�� �����ϸ�, ũ��� 1�� ����ϴ�.
        cameraRight.Normalize();

        Vector3 dir = transform.right * h + transform.forward * v;
        dir.Normalize();

       // dir = Camera.main.transform.TransformDirection(dir);

        // ���� Ȯ�� �Լ�
        bool isGrounded = Physics.Raycast(transform.position, -Vector3.up, _characterController.height / 2 + 0.1f);

        if (isGrounded && _yVelocity < 0)
        {
            _isJumping = false;
            _yVelocity = -0.5f; // ���� ���¿����� �ణ�� �߷��� �����Ͽ� �÷��̾ �ٴڿ� �����ǵ��� ��
            JumpRemainCount = JumpMaxCount;
        }
        else
        {
            _yVelocity += GravityConstant * Time.deltaTime; // �߷� ���ӵ�
        }

        // ���� ����
        if (Input.GetKeyDown(KeyCode.Space) && JumpRemainCount > 0 && SurvivalGauge.Instance.Stamina >= jumpStaminaCost) 
        {
            _yVelocity = JumpPower; // y�࿡ �����Ŀ� ����
            _isJumping = true;
            JumpRemainCount--;
            SurvivalGauge.Instance.Stamina -= jumpStaminaCost; //������ �� �Ҹ�Ǵ� ���¹̳��� ��
        }

        // ���� �̵� ���Ϳ� y�� �ӵ��� �߰�
        Vector3 velocity = dir * MoveSpeed + Vector3.up * _yVelocity;
        // �̵��ϱ�
        _characterController.Move(velocity * Time.deltaTime);

        // ĳ������ ȸ���� �̵� ���⿡ ����ϴ�.
        if (dir != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(dir, Vector3.up);
            _characterController.transform.rotation = Quaternion.RotateTowards(_characterController.transform.rotation, toRotation, lookSensitivity * Time.deltaTime);
        }
    }

    private void CameraRotation()
    {
        // ���콺�� ���� �����ӿ� ���� ī�޶� ȸ��
        float _xRotation = Input.GetAxisRaw("Mouse Y") * lookSensitivity;
        currentCameraRotationX -= _xRotation;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        // ī�޶��� x�� ȸ���� ����
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, theCamera.transform.localEulerAngles.y, 0f);
    }

    private void CharacterRotation()
    {
        // ���콺�� �¿� �����ӿ� ���� ĳ���� ȸ��
        float _yRotation = Input.GetAxisRaw("Mouse X") * lookSensitivity;
        Vector3 currentRotation = _characterController.transform.eulerAngles;
        float newYRotation = currentRotation.y + _yRotation;
        _characterController.transform.eulerAngles = new Vector3(0, newYRotation, 0);
        
    }

    // ������ ���ÿ�!
    void ClampPlayerPosition()
    {
        // �÷��̾��� ���� ��ġ�� �����ɴϴ�.
        Vector3 clampedPosition = transform.position;

        // ���� ��踦 �����մϴ�. �� ���� ���� ���� ũ�� �� ��ġ�� ���� �����ؾ� �մϴ�.
        float mapWidth = 300f;
        float mapHeight = 40f;

        // �÷��̾ �� ��踦 ����� �ʵ��� �����մϴ�.
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -mapWidth / 2f, mapWidth / 2f);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -mapHeight / 2f, mapHeight / 2f);

        // �÷��̾��� ��ġ�� ������Ʈ�մϴ�.
        transform.position = clampedPosition;
    }
}