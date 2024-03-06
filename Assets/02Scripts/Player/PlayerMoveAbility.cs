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

    // wasd: �̵�
    public float MoveSpeed = 5;

    // shift: ���¹̳�

    public float RunSpeed = 10;
    public float Stamina = 100;             // ���¹̳�
    public const float MaxStamina = 100;    // �ִ뷮
    public float StaminaConsumeSpeed = 33f; // �ʴ� ���¹̳� �Ҹ�
    public float StaminaChargeSpeed = 50;  // �ʴ� ���¹̳� ������

    // spacebar: ����

    public float JumpPower = 10;
    public int JumpMaxCount = 2;
    public int JumpRemainCount;
    private bool _isJumping = false;
    // private float _gravity = -5;        // �߷� ��
    private float _yVelocity = 0f;         // ������ �߷� ����: y�� �ӵ�
    private const float GravityConstant = -9.81f; // �߷� ���


    void Start()
    {
        _characterController = GetComponent<CharacterController>();  
        Cursor.visible = false;
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

        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();

        float speed = MoveSpeed;

        // Shift ������ ���� �ٱ�
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Stamina -= StaminaConsumeSpeed * Time.deltaTime;

            if (Stamina > 0)
            {
                speed = RunSpeed;
            }
        }
        else
        {
            Stamina += StaminaChargeSpeed * Time.deltaTime; // �ʴ� 50�� ����
        }

        Stamina = Mathf.Clamp(Stamina, 0, 100); // ���� �Ѿ�� �ʵ���

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

        // ���� ����
        if (Input.GetKeyDown(KeyCode.Space) && (_characterController.isGrounded || JumpRemainCount > 0))
        {
            _isJumping = true;
            JumpRemainCount--;

            _yVelocity = JumpPower; // y�࿡ �����Ŀ� ����
        }

        // �߷� ����
        if (!_characterController.isGrounded)
        {
            _yVelocity += GravityConstant * Time.deltaTime; // �߷� ���ӵ�
            dir.y = _yVelocity;
        }


        // �̵��ϱ�
        //transform.position += speed * dir * Time.deltaTime;
        _characterController.Move(dir * Time.deltaTime);
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
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    private void CharacterRotation()
    {
        // ���콺 �¿�(X) �������� �޾� ĳ������ ȸ���� ����
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;

        // ĳ������ ȸ���� ����
        _characterController.transform.Rotate(_characterRotationY);
    }
}