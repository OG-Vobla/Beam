using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailControlScript : MonoBehaviour
{
	private Animator animator;
	private bool goRight = false;
	[SerializeField] private Transform leftPoint;
	[SerializeField] private Transform rightPoint;
	[SerializeField] private float spead;
	[SerializeField] private float pauseTime;
	[SerializeField] private BoxCollider2D ShellCollider;
	[SerializeField] private BoxCollider2D SnailCollider;
	private Rigidbody2D rb;
	private bool pause = false;
	private bool isSnail = true;
    private AudioSource audioSource;
    private GameObject dieSound;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        dieSound = GameObject.FindGameObjectWithTag("DieSound");
        isSnail = true;
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		if (!pause)
		{
			if (isSnail)
			{

				if ((transform.position.x == rightPoint.position.x && goRight) || (transform.position.x == leftPoint.position.x && !goRight))
				{
					animator.SetBool("isWalk", false);
					StartCoroutine(Pause());
					goRight = !goRight;
					transform.Rotate(new Vector3(0, -180, 0));
				}
				else
				{
					animator.SetBool("isWalk", true);
					if (goRight)
					{
						transform.position = Vector2.MoveTowards(transform.position, (new Vector2(rightPoint.position.x, transform.position.y)), Time.deltaTime * spead);
					}
					else
					{
						transform.position = Vector2.MoveTowards(transform.position, (new Vector2(leftPoint.position.x, transform.position.y)), Time.deltaTime * spead);
					}
				}
			}
			else
			{
				rb.velocity = rb.velocity.normalized * 15;
			}
		}
	}
	private IEnumerator Pause()
	{
		pause = true;
		yield return new WaitForSeconds(pauseTime);
		pause = false;
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
        {
            if (PlayerDataScript.soundsOn)
            {
                dieSound.GetComponent<AudioSource>().Play();
            }
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.up * 10;
			animator.SetTrigger("Hit");
			if (isSnail)
			{
				isSnail= false;
				animator.SetBool("isSnail", false);
				rb.velocity = (Random.Range(0,2)  == 0? Vector2.right : Vector2.left) * 15;
				SnailCollider.enabled = false;
				ShellCollider.enabled = true;
			}
			else
			{
				pause = true;
				ShellCollider.enabled = false;
				rb.velocity = Vector2.up * 12;
				rb.gravityScale = 5;
				Destroy(gameObject, 2f);
			}

		}
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag != "Player" )
		{

            if (PlayerDataScript.soundsOn)
            {
                audioSource.Play();
            }
            animator.SetTrigger("WallHit");
			animator.SetTrigger("WallHit");
		}
	}
}
