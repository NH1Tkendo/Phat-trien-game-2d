using System.Linq;
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
		//Hàm chỉnh hướng quay mặt của quái
		GameObject playerObj = GameObject.FindWithTag("Player");
		if (playerObj != null)
		{
			playerTransform = playerObj.transform;

			float dir = Mathf.Sign(playerTransform.position.x - transform.position.x);
			if (dir != 0)
			{
				Vector3 scale = transform.localScale;
				scale.x = Mathf.Abs(scale.x) * dir;
				transform.localScale = scale;
				facingRight = dir > 0;
				lastMoveDirection = dir;
			}
		}
		else
		{
			Debug.LogWarning($"{name}: Không tìm thấy Player trong scene!");
		}

		// Tự tìm patrol points
		if (patrolPoints == null || patrolPoints.Length == 0)
		{
			GameObject[] points = GameObject.FindGameObjectsWithTag("Patrol");
			if (points.Length > 0)
			{
				patrolPoints = points.Select(p => p.transform).ToArray();
			}
			else
			{
				Debug.LogWarning($"{name}: Không tìm thấy patrol points nào trong scene!");
			}
		}
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

		if (playerTransform == null || patrolPoints == null || patrolPoints.Length == 0)
			return;

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
		if (patrolPoints == null || patrolPoints.Length == 0)
		{
			Debug.LogWarning($"{name}: patrolPoints null hoặc rỗng trong HandlePatrol");
			return;
		}

		if (patrolDestination < 0 || patrolDestination >= patrolPoints.Length)
		{
			Debug.LogWarning($"{name}: patrolDestination không hợp lệ!");
			return;
		}

		Transform targetPoint = patrolPoints[patrolDestination];
		if (targetPoint == null)
		{
			return;
		}

		Vector3 direction = targetPoint.position - transform.position;
		float moveDir = Mathf.Sign(direction.x);

		UpdateFacing(moveDir);
		transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

		if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f)
		{
			patrolDestination = (patrolDestination + 1) % patrolPoints.Length;
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