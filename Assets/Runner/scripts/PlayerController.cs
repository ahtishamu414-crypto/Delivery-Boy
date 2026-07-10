using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Slide")]
    public float slideDuration = 1f;
    private bool isSliding = false;
    private float originalHeight;
    private Vector3 originalCenter;

    [Header("Movement")]
    public float forwardSpeed = 10f;
    public float laneDistance = 2.5f;
    public float laneChangeSpeed = 12f;

    [Header("Jump")]
    public float jumpForce = 8f;
    public float gravity = -20f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundLayer;

    [Header("Health Score")]
    public int health = 100;

    private int currentLane = 1;
    private float targetX;
    private float originX;

    private float verticalVelocity = 0f;
    private bool isGrounded = true;
    private bool isDead = false;

    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private bool swipeDetected;
    public float swipeThreshold = 50f;

    private CharacterController cc;
    private Animator anim;
    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        originX = transform.position.x;
        targetX = originX;
        originalHeight = cc.height;
        originalCenter = cc.center;
        Physics.queriesHitTriggers = true;
    }

    void Update()
    {
        CheckGround();
        HandleInput();
        MovePlayer();
    }

    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
            anim.SetBool("isJumping", false);
        }
    }

    void MovePlayer()
    {
        if (isDead) return;

        verticalVelocity += gravity * Time.deltaTime;

        float targetXThisFrame = Mathf.Lerp(transform.position.x, targetX, laneChangeSpeed * Time.deltaTime);

        Vector3 move = new Vector3(
            targetXThisFrame - transform.position.x,
            verticalVelocity * Time.deltaTime,
            forwardSpeed * Time.deltaTime
        );

        cc.Move(move);
    }

    void HandleInput()
    {
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            MoveLeft();
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            MoveRight();
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            Jump();
        if (Input.GetKeyDown(KeyCode.DownArrow) && isGrounded)
            StartSlide();

        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                startTouchPos = t.position;
                swipeDetected = false;
            }
            else if (t.phase == TouchPhase.Ended && !swipeDetected)
            {
                endTouchPos = t.position;
                DetectSwipe();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            startTouchPos = Input.mousePosition;
            swipeDetected = false;
        }
        if (Input.GetMouseButtonUp(0) && !swipeDetected)
        {
            endTouchPos = Input.mousePosition;
            DetectSwipe();
        }
    }

    void DetectSwipe()
    {
        float deltaX = endTouchPos.x - startTouchPos.x;
        float deltaY = endTouchPos.y - startTouchPos.y;

        if (Mathf.Abs(deltaY) > Mathf.Abs(deltaX))
        {
            if (deltaY > swipeThreshold)
            {
                if (isGrounded) Jump();
            }
            else if (deltaY < -swipeThreshold)
            {
                if (isGrounded) StartSlide();
            }
            swipeDetected = true;
            return;
        }

        if (Mathf.Abs(deltaX) < swipeThreshold) return;

        if (deltaX > 0) MoveRight();
        else MoveLeft();

        swipeDetected = true;
    }

    void Jump()
    {
        if (isSliding) return;
        verticalVelocity = jumpForce;
        anim.SetBool("isJumping", true);
    }

    void StartSlide()
    {
        if (isSliding) return;
        StartCoroutine(SlideCoroutine());
    }

    IEnumerator SlideCoroutine()
    {
        isSliding = true;
        anim.SetTrigger("slide");

        cc.height = originalHeight * 0.5f;
        cc.center = new Vector3(
            originalCenter.x,
            originalCenter.y * 0.5f,
            originalCenter.z
        );

        yield return new WaitForSeconds(slideDuration);

        cc.height = originalHeight;
        cc.center = originalCenter;
        isSliding = false;
    }

    // ← CHANGED: CompareTag instead of LayerMask.NameToLayer
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            GameManager.instance.AddScore(1);
            AudioManager.instance.PlayCoinCollect();
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            Destroy(other.gameObject, 0.1f);
        }
        if (other.CompareTag("Enemy"))
        {
            health -= 25;
            if (health < 0) health = 0;
            if (health != 0)
            {
                anim.SetTrigger("block");
                AudioManager.instance.PlayEnemyHit();
            }
            GameManager.instance.HealthScore(health);
            Destroy(other.gameObject, 0.1f);

            if (health == 0)
            {
                isDead = true;
                forwardSpeed = 0f;
                laneChangeSpeed = 0f;
                anim.SetTrigger("death");
                AudioManager.instance.PlayFall();
                StartCoroutine(DieCoroutine());
            }
        }
        if (other.CompareTag("Enemy1"))
        {
            health -= 50;
            if (health < 0) health = 0;
            if (health != 0)
            {
                anim.SetTrigger("block");
                AudioManager.instance.PlayEnemyHit();
            }
            GameManager.instance.HealthScore(health);
            Destroy(other.gameObject, 0.1f);

            if (health == 0)
            {
                isDead = true;
                forwardSpeed = 0f;
                laneChangeSpeed = 0f;
                anim.SetTrigger("death");
                AudioManager.instance.PlayFall();
                StartCoroutine(DieCoroutine());
            }
        }
    }
    // ← CHANGED: CompareTag instead of LayerMask.NameToLayer
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle") && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    void Die()
{
    isDead = true;
    forwardSpeed = 0f;
    laneChangeSpeed = 0f;
    anim.SetTrigger("die");

    // Fixed deprecated method
    EnemyMovement[] enemies = FindObjectsByType<EnemyMovement>(FindObjectsSortMode.None);
    foreach (EnemyMovement enemy in enemies)
    {
        enemy.enabled = false;
    }

    AudioManager.instance.PlayObstacleHit();
    StartCoroutine(DieCoroutine());
}

    IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        GameManager.instance.GameOver();
    }

    void MoveLeft()
    {
        currentLane = Mathf.Clamp(currentLane - 1, 0, 2);
        targetX = originX + (currentLane - 1) * laneDistance;
    }

    void MoveRight()
    {
        currentLane = Mathf.Clamp(currentLane + 1, 0, 2);
        targetX = originX + (currentLane - 1) * laneDistance;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}