using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float jumpForce = 15f;
    private Rigidbody2D rb;

	public void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		HandleMovement();
		rb.linearVelocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), rb.linearVelocity.y);

		if (Input.GetButtonDown("Jump"))
		{
			rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
		}
	}
	private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        if (moveInput > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);
    }
}
