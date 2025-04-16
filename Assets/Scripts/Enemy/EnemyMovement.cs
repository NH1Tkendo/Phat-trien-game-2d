using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //Các biến để tuần tra của mục tiêu
    public Transform[] patrolPoints;
    public int patrolDestination;
    //Các biến liên quan đến di chuyển
    public float moveSpeed = 3f;//Tốc độ di chuyển
    private float xVelocity = 0f;//Biến để truyền vào animator
    private Vector3 lastPosition;

    private Animator animator;
    private bool facingRight = true;

    public Transform playerTransform;
    public bool isChasing;
    public float chaseDistance;
    public float chaseTimeout = 3f;

    public float idleTime = 2f;
    private bool isIdle = false;
    private float idleTimer = 0f;

    private float chaseTimer = 0f;
    private float lastMoveDirection = 0f;

    private EnemyShooter shooter;

    void Start()
    {
        animator = GetComponent<Animator>();
        shooter = GetComponent<EnemyShooter>();
        lastPosition = transform.position;
    }

    void Update()
    {
        if (shooter != null)
            shooter.isChasing = isChasing;

        if (isIdle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleTime)
            {
                isIdle = false;
                idleTimer = 0f;
            }

            if (animator != null)
                animator.SetBool("IsMoving", false);

            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (!isChasing && distanceToPlayer < chaseDistance)
        {
            isChasing = true;
            chaseTimer = 0f;
        }

        if (isChasing)
            HandleChase(distanceToPlayer);
        else
            HandlePatrol();
        // ✅ Tính xVelocity sau khi đã di chuyển xong
        xVelocity = (transform.position.x - lastPosition.x) / Time.deltaTime;
        lastPosition = transform.position;

        if (animator != null)
            animator.SetFloat("xVelocity", Mathf.Abs(xVelocity));
    }

    void HandleChase(float distanceToPlayer)
    {
        float directionToPlayer = playerTransform.position.x - transform.position.x;
        float moveDir = Mathf.Sign(directionToPlayer);

        UpdateFacing(moveDir);
        transform.position += (moveDir > 0 ? Vector3.right : Vector3.left) * moveSpeed * Time.deltaTime;

        if (distanceToPlayer < chaseDistance)
        {
            chaseTimer = 0f;
        }
        else
        {
            chaseTimer += Time.deltaTime;
            if (chaseTimer > chaseTimeout)
            {
                StopChase();
                StartIdle();
            }
        }

        if (animator != null)
            animator.SetBool("IsMoving", true);
    }

    void HandlePatrol()
    {
        Transform targetPoint = patrolPoints[patrolDestination];
        Vector3 direction = targetPoint.position - transform.position;
        float moveDir = Mathf.Sign(direction.x);

        UpdateFacing(moveDir);
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            patrolDestination = 1 - patrolDestination;
            StartIdle();
        }

        if (animator != null)
            animator.SetBool("IsMoving", true);
    }

    void StopChase()
    {
        isChasing = false;
        chaseTimer = 0f;
    }

    void StartIdle()
    {
        isIdle = true;
        idleTimer = 0f;
        if (animator != null)
            animator.SetBool("IsMoving", false);
    }

    void UpdateFacing(float moveDir)
    {
        if (moveDir != 0 && moveDir != lastMoveDirection)
        {
            Flip();
            lastMoveDirection = moveDir;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;

        if (shooter != null && shooter.firePoint != null)
        {
            Vector3 fireScale = shooter.firePoint.localScale;
            fireScale.x *= -1f;
            shooter.firePoint.localScale = fireScale;
        }
    }
}