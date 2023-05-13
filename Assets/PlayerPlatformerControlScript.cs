using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerControlScript : MonoBehaviour
{
	[SerializeField] float PlayerSpeed;
	[SerializeField] Transform groundCheck;
	[SerializeField] float checkRadius;
	[SerializeField] float jumpForce;
	[SerializeField] LayerMask whatIsGround;
	private Rigidbody2D rb;
	private bool isGrounded;
	private int extraJumpsValue;
	private bool PlayerRight = true;
	private Animator animator;
	private CoolPlatformerGameManager gameManager;
    private GameObject dieSound;
    // Start is called before the first frame update
    void Start()
    {
        dieSound = GameObject.FindGameObjectWithTag("DieSound");
        gameManager = FindObjectOfType<CoolPlatformerGameManager>();
		extraJumpsValue = 1;
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
	}
	private void FixedUpdate()
	{
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
	}
	// Update is called once per frame
	void Update()
	{
		if (rb.velocity.y ==0)
		{
			animator.SetBool("Jump", false);
			animator.SetBool("DoubleJump", false);
			extraJumpsValue = 1;
		}
		if (Input.GetKeyDown(KeyCode.Space) && extraJumpsValue > 0 && rb.velocity.y == 0)
		{
			animator.SetBool("Jump", true);
			rb.velocity = Vector2.up * jumpForce;
			extraJumpsValue = 0;
		}
		else if (Input.GetKeyDown(KeyCode.Space) && extraJumpsValue == 0)
		{
			isGrounded = false;
			animator.SetBool("DoubleJump", true);
			rb.velocity = Vector2.up * jumpForce;
			extraJumpsValue--;
		}
		if (rb.velocity.y > 0)
		{
			animator.SetBool("Jump", true);
		}
		transform.position = new Vector2(transform.position.x + Input.GetAxisRaw("Horizontal") * Time.deltaTime * PlayerSpeed, transform.position.y);
		if (Input.GetAxisRaw("Horizontal") < 0 )
		{
			if (PlayerRight)
			{
				Flip();
			}
			if (rb.velocity.y == 0)
			{
				animator.SetBool("isWalk", true);
			}
		}
		else if (Input.GetAxisRaw("Horizontal") > 0 )
		{
			if (!PlayerRight)
			{
				Flip();
			}
			if (rb.velocity.y == 0)
			{
				animator.SetBool("isWalk", true);

			}
		}
		else
		{
			animator.SetBool("isWalk", false);

		}
		
		
	
	}
	private void Flip()
	{
		PlayerRight = !PlayerRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Trap" || collision.gameObject.tag == "Npc")
        {
            if (PlayerDataScript.soundsOn)
            {
                dieSound.GetComponent<AudioSource>().Play();
            }
            gameManager.PlyerDie();
		}
	}
}
