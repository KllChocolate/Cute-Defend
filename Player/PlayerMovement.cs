using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public AudioClip SwordAttackSound;
    public AudioClip grass;
    public AudioClip winSound; 
    public Transform RayStart;
    public LayerMask LayerMark;
    private AudioSource audioSource;
    public GameObject areaAttack;

    [Header("Attack")]
    public bool canAttack = true;
    public float AttackCooldown = 0.5f;
    public bool isAttacking = false;
    public bool isDefend = false;

    [Header("Jump")]
    public bool isJumping = false;
    public bool isGrounded = true;

    [Header("Item")]
    public GameObject axe;
    public GameObject sword1;
    public GameObject sword2;
    public GameObject sword3;
    public GameObject sword4;
    public GameObject sword5;
    public GameObject sword6;
    public GameObject sword7;
    public GameObject sword8;
    public GameObject sword9;

    public static PlayerMovement instance;

    [SerializeField]private float rotationSpeed;
    [SerializeField]private float jumpSpeed;
    [SerializeField]private float jumpButtonGracePeriod;
    [SerializeField]private float jumpHorizontalSpeed;
    [SerializeField]private Transform cameraTransform;
    [SerializeField]private bool CursorLockedVar;
    private float ySpeed;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;

    private Animator animator;
    private CharacterController character;
    public GameObject completeUI;
    private bool Win = false;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        areaAttack.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        character = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = (false);
        CursorLockedVar = (true);
    }

    //Å§¾×é¹
    private void OnAnimatorMove()
    {
        if (isGrounded)
        {
            Vector3 velocity = animator.deltaPosition;
            velocity.y = ySpeed * Time.deltaTime * 2;
            character.Move(velocity);
        }
    }
    private void Update()
    {
        //Walk
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(x, 0, z);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude) * speed;

        //Run
        if (Input.GetKey(KeyCode.LeftShift))
        {
            inputMagnitude *= 3;
        }

        animator.SetFloat("Speed", inputMagnitude, 0.05f, Time.deltaTime);

        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        ySpeed += Physics.gravity.y * Time.deltaTime * 2f;

        if (character.isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
        }
        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            ySpeed = -0.5f;
            animator.SetBool("IsGrounded", true);
            isGrounded = true;
            animator.SetBool("IsJumping", false);
            isJumping = false;
            animator.SetBool("IsFalling", false);

            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = jumpSpeed;
                animator.SetBool("IsJumping", true);
                isJumping = true;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
        }
        else
        {
            character.stepOffset = 0;
            animator.SetBool("IsGrounded", false);
            isGrounded = false;

            if ((isJumping && ySpeed < 0) || ySpeed < -2)
            {
                animator.SetBool("IsFalling", true);
            }
        }

        if (movementDirection != Vector3.zero)
        {
            animator.SetBool("IsMoving", true);

            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        else
        {
            animator.SetBool("IsMoving", false);
        }

        if (isGrounded == false)
        {
            Vector3 velocity = movementDirection * inputMagnitude * jumpHorizontalSpeed;
            velocity.y = ySpeed;

            character.Move(velocity * Time.deltaTime);
        }

        //¤ÅÔê¡«éÒÂ
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (canAttack && Time.timeScale == 1)
            {
                Attack();
            }
        }
        //¤ÅÔê¡¢ÇÒ
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (isAttacking == false)
            {
                canAttack = false;
                isDefend = true;
                animator.SetBool("Defend", true);
            }
        }
        //àÅÔ¡¤ÅÔê¡¢ÇÒ
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            if (isDefend == true)
            {
                canAttack = true;
                isDefend = false;
                animator.SetBool("Defend", false);
            }
        }
        //Inventory
        if (Input.GetKeyDown(KeyCode.I) && !CursorLockedVar)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = (false);
            CursorLockedVar = (true);
            canAttack = true;
            Time.timeScale = 1;
        }
        else if (Input.GetKeyDown(KeyCode.I) && CursorLockedVar)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = (true);
            CursorLockedVar = (false);
            canAttack = false;
            Time.timeScale = 0;
        }
        if(WaveSpawner.instance.complete == true)
        { 
            animator.SetTrigger("Victory");
            StartCoroutine(complete());
            Win = true;
        }
        if(Win)
        {
            audioSource.PlayOneShot(winSound);
            Win = false;
        }
        
    }

    public IEnumerator complete()
    {
        completeUI.SetActive(true);
        yield return new WaitForSeconds(3);
        Time.timeScale = 0;
    }

    //â¨ÁµÕ
    private void Attack()
    {

        isAttacking = true;
        canAttack = false;
        animator.SetTrigger("Attack");
        animator.SetInteger("AttackInDex", Random.Range(0,3));
        StartCoroutine(CooldownAttack());
        audioSource.PlayOneShot(SwordAttackSound);
    }
    private IEnumerator CooldownAttack()
    {
        yield return new WaitForSeconds(0.2f);
        areaAttack.SetActive(true);
        isAttacking = false;
        yield return new WaitForSeconds(0.1f);
        areaAttack.SetActive(false);
        yield return new WaitForSeconds(AttackCooldown);
        canAttack = true;
    }

    public void ShowAxe()
    {
        axe.SetActive(!axe.activeSelf);
        sword1.SetActive(false);sword2.SetActive(false);sword3.SetActive(false);sword4.SetActive(false);sword5.SetActive(false);sword6.SetActive(false); sword7.SetActive(false); sword8.SetActive(false); sword9.SetActive(false);

    }
    public void ShowSword1()
    {
        sword1.SetActive(!sword1.activeSelf);
        axe.SetActive(false); sword2.SetActive(false); sword3.SetActive(false); sword4.SetActive(false); sword5.SetActive(false); sword6.SetActive(false); sword7.SetActive(false); sword8.SetActive(false); sword9.SetActive(false);
    }
    public void ShowSword2()
    {
        sword2.SetActive(!sword2.activeSelf);
        sword1.SetActive(false); axe.SetActive(false); sword3.SetActive(false); sword4.SetActive(false); sword5.SetActive(false); sword6.SetActive(false); sword7.SetActive(false); sword8.SetActive(false); sword9.SetActive(false);
    }
    public void ShowSword3()
    {
        sword3.SetActive(!sword3.activeSelf);
        sword1.SetActive(false); sword2.SetActive(false); axe.SetActive(false); sword4.SetActive(false); sword5.SetActive(false); sword6.SetActive(false); sword7.SetActive(false); sword8.SetActive(false); sword9.SetActive(false);
    }
    public void ShowSword4()
    {
        sword4.SetActive(!sword4.activeSelf);
        sword1.SetActive(false); sword2.SetActive(false); sword3.SetActive(false); axe.SetActive(false); sword5.SetActive(false); sword6.SetActive(false); sword7.SetActive(false); sword8.SetActive(false); sword9.SetActive(false);
    }
    public void ShowSword5()
    {
        sword5.SetActive(!sword5.activeSelf);
        sword1.SetActive(false); sword2.SetActive(false); sword3.SetActive(false); sword4.SetActive(false); axe.SetActive(false); sword6.SetActive(false); sword7.SetActive(false); sword8.SetActive(false); sword9.SetActive(false);
    }
    public void ShowSword6()
    {
        sword6.SetActive(!sword6.activeSelf);
        sword1.SetActive(false); sword2.SetActive(false); sword3.SetActive(false); sword4.SetActive(false); sword5.SetActive(false); axe.SetActive(false); sword7.SetActive(false); sword8.SetActive(false); sword9.SetActive(false);
    }
    public void ShowSword7()
    {
        sword7.SetActive(!sword7.activeSelf);
        sword1.SetActive(false); sword2.SetActive(false); sword3.SetActive(false); sword4.SetActive(false); sword5.SetActive(false); sword6.SetActive(false); axe.SetActive(false); sword8.SetActive(false); sword9.SetActive(false);
    }
    public void ShowSword8()
    {
        sword8.SetActive(!sword8.activeSelf);
        sword1.SetActive(false); sword2.SetActive(false); sword3.SetActive(false); sword4.SetActive(false); sword5.SetActive(false); sword6.SetActive(false); sword7.SetActive(false); axe.SetActive(false); sword9.SetActive(false);
    }
    public void ShowSword9()
    {
        sword9.SetActive(!sword9.activeSelf);
        sword1.SetActive(false); sword2.SetActive(false); sword3.SetActive(false); sword4.SetActive(false); sword5.SetActive(false); sword6.SetActive(false); sword7.SetActive(false); sword8.SetActive(false); axe.SetActive(false);
    }

}

