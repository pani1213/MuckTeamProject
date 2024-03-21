using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public enum FoodPuduState
{
    Idle,       
    Damaged,   
    Die         
}

public class FoodPudu : MonoBehaviour, IHitable
{
    private CharacterController _characterController;
    private Animator _animator;
    private Vector3 _velocity; 
    public float gravity = -100f; 

    public int FoodPuduHealth;
    public int MaxHealth = 100;
    public Slider HealthSliderUI;
    private bool isDead = false;

    public Transform _mainCharacter;     
    public float MoveSpeed = 1f;         
    public Vector3 StartPosition;      
    public float MoveDistance = 1f;  
    public const float TOLERANCE = 0.1f; 

    private float changeDirectionTime = 2f; 
    private float directionChangeInterval = 5.0f; 
    private float smoothRotationTime = 1.0f;
    private float directionChangeTimer = 0.0f;
    private Quaternion targetRotation;

    private float damagedCooldownTimer = 0f; 
    private readonly float damagedCooldownDuration = 1f; 
    private Vector3 _knockbackStartPosition;   
    private Vector3 _knockbackEndPosition;
    private const float KNOCKBACK_DURATION = 0.2f;
    private float _knockbackProgress = 0f;
    public float KnockbackPower = 1.2f;

    private FoodPuduState _currentState = FoodPuduState.Idle;
    public GameObject bloodEffectPrefab;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        StartPosition = transform.position;
        FoodPuduHealth = MaxHealth;
        targetRotation = transform.rotation; 

    }


    private void Update()
    {
        HealthSliderUI.value = (float)FoodPuduHealth / (float)MaxHealth; // 0 ~ 1

        bool isGrounded = _characterController.isGrounded;
        if (isGrounded && _velocity.y < 0)
        {
            _velocity.y = 0f; 
        }
      
        _velocity.y += gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);

        switch (_currentState)
        {
            case FoodPuduState.Idle:
                Idle();
                break;

            case FoodPuduState.Damaged:
                Damaged();
                break;

            case FoodPuduState.Die:
                Die();
                break;
        }
    }

    private void Idle()
    {
        directionChangeTimer += Time.deltaTime;

        if (directionChangeTimer >= directionChangeInterval)
        {
            directionChangeTimer = 0;
            float angle = Random.Range(0f, 360f);
            targetRotation = Quaternion.Euler(0, angle, 0);
        }

        
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime / smoothRotationTime);

       
        Vector3 move = transform.forward * MoveSpeed * Time.deltaTime;

        _characterController.Move(move); 

       
        if (Vector3.Distance(transform.position, StartPosition) > MoveDistance)
        {
            
            Vector3 returnDirection = (StartPosition - transform.position).normalized;

            _characterController.Move(returnDirection * MoveSpeed * Time.deltaTime);

        }
    }

    private void Damaged()
    {
        if (_knockbackProgress == 0) 
        {
            _knockbackStartPosition = transform.position;
            Vector3 dir = transform.position - _mainCharacter.position; 
            dir.y = 0;
            dir.Normalize();

            _knockbackEndPosition = transform.position + dir * KnockbackPower; 
        }
        _knockbackProgress += Time.deltaTime / KNOCKBACK_DURATION;

        transform.position = Vector3.Lerp(_knockbackStartPosition, _knockbackEndPosition, _knockbackProgress);

        if (_knockbackProgress > 1)  
        {
            _knockbackProgress = 0f;
            _currentState = FoodPuduState.Idle;
            _animator.SetTrigger("DamagedToIdle");
            _animator.Play("Idle", 1);
            damagedCooldownTimer = 0f; 
        }
    }

    public void Hit(DamageInfo damage)
    {
        if (isDead || damagedCooldownTimer > 0)
        {
            return;
        }
        //SoundManager.instance.PlayAudio("Blood1");
       
        BloodFactory.Instance.Make(this.transform.position + Vector3.up, damage.Normal, this.gameObject);

        FoodPuduHealth -= damage.Amount;

        if (FoodPuduHealth <= 0)
        {
         
            _animator.SetTrigger("IdleToDie"); 
            _animator.Play("Die", 1);
            isDead = true;
            _currentState = FoodPuduState.Die;
        }
        else
        {
          
            _currentState = FoodPuduState.Damaged;
            _animator.SetTrigger("IdleToDamaged");
            _animator.Play("Damaged", 1);

        }

        damagedCooldownTimer = damagedCooldownDuration;
    }

    private Coroutine _dieCoroutine;

    private void Die()
    {

        _dieCoroutine = StartCoroutine(Die_Coroutine());

    }
    private IEnumerator Die_Coroutine()
    {
        HealthSliderUI.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);

        //ItemObjectFactory.Instance.Make(transform.position);

        gameObject.SetActive(false); 

        PuduGroup puduGroup = GetComponentInParent<PuduGroup>();
        if (puduGroup != null)
        {
            puduGroup.OnPuduDead();
        }


    }
    public void Reinitialize()
    {
        isDead = false;
        FoodPuduHealth = MaxHealth; 
        HealthSliderUI.value = 1; 
        _currentState = FoodPuduState.Idle; 
        damagedCooldownTimer = 0;
        gameObject.SetActive(true); 

        HealthSliderUI.gameObject.SetActive(true); 
    }

}