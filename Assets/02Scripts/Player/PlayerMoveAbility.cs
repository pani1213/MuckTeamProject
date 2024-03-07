using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveAbility : MonoBehaviour
{
    public Camera theCamera;
    private float lookSensitivity;          // ���콺�� �����ӿ� ���� ȸ�� �ΰ���
    private float cameraRotationLimit;
    private float currentCameraRotationX;

    private CharacterController _characterController;
    private Animator _animator;

    // wasd: �̵�
    public float MoveSpeed = 5;

    // spacebar: ����
    public float JumpPower = 10;
    public int JumpMaxCount = 2;
    public int JumpRemainCount;
    private bool _isJumping = false;
    // private float _gravity = -5;        // �߷� ��
    private float _yVelocity = 0f;         // ������ �߷� ����: y�� �ӵ�
    private const float GravityConstant = -9.81f; // �߷� ���

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        Cursor.visible = false;

        lookSensitivity = 2f; // ���콺 �ΰ����� �����ϴ� ��, ������ ������ ����
        cameraRotationLimit = 85f; // ī�޶� ���Ʒ��� ȸ���� �� �ִ� �ִ� ����, 85�� ������ ����
        currentCameraRotationX = 0f; // �ʱ� ī�޶� X�� ȸ������ 0���� ����

    }


    private void Update()
    {
        Move();
        CameraRotation();       // ���콺 ���Ʒ�(Y) ������
        CharacterRotation();    // ���콺 �¿�(X) ������

        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.Locked;
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal"); // �¿�(����Ű ����/������) 
        float v = Input.GetAxis("Vertical"); // ����(����Ű ��/�Ʒ�) 

        Vector3 dir = transform.right * h + transform.forward * v;
        dir.Normalize();

        dir = Camera.main.transform.TransformDirection(dir);


        // ���� Ȯ�� �Լ�
        bool isGrounded = Physics.Raycast(transform.position, -Vector3.up, _characterController.height / 2 + 0.1f);


        if (isGrounded)
        {
            _isJumping = false;
            _yVelocity = -0.5f; // ���� ���¿����� �ణ�� �߷��� �����Ͽ� �÷��̾ �ٴڿ� �����ǵ��� ��
            JumpRemainCount = JumpMaxCount;
        }

        // ���� ����
        if (Input.GetKeyDown(KeyCode.Space) && JumpRemainCount > 0)
        {
            _yVelocity = JumpPower; // y�࿡ �����Ŀ� ����
            _isJumping = true;
            JumpRemainCount--;
        }

        // �߷� ����
        if (!_characterController.isGrounded || _isJumping)
        {
            _yVelocity += GravityConstant * Time.deltaTime; // �߷� ���ӵ�
        }

        // ���� �̵� ���Ϳ� y�� �ӵ��� �߰�
        Vector3 velocity = dir * MoveSpeed + Vector3.up * _yVelocity;
        // �̵��ϱ�
        _characterController.Move(velocity * Time.deltaTime);
    }

    private void CameraRotation()
    {
        // ���콺�� ���Ʒ�(Y) �������� �޾� ī�޶��� ȸ���� ����
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;

        // ī�޶��� ȸ���� ������Ʈ�ϰ�, ���� ���� ���� �ִ��� Ȯ��
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        // ī�޶��� ȸ���� ����
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, theCamera.transform.localEulerAngles.y, 0f);
    }

    private void CharacterRotation()
    {
        // ���콺 �¿�(X) �������� �޾� ĳ������ ȸ���� ����
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = Vector3.up * _yRotation * lookSensitivity;

        // ĳ������ ȸ���� ����
        transform.Rotate(_characterRotationY);
    }
}