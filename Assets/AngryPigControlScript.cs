using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class AngryPigControlScript : MonoBehaviour
{
	private Animator animator;
	private bool goRight = false;
	[SerializeField] private Transform rayPoint;
	[SerializeField] private LayerMask ignoreMask;
	[SerializeField] private Transform leftPoint;
	[SerializeField] private Transform rightPoint;
	[SerializeField] private float spead;
	private Vector2 playerPos = Vector2.zero;
	[SerializeField] private float pauseTime;
	private bool pause = false;
	// Start is called before the first frame update
	void Start()
	{
		animator = GetComponent<Animator>();
		rayPoint = transform.Find("rayPoint");
	}

    private bool Scan()
    {
		bool result = false;
		RaycastHit2D hit;
		hit = Physics2D.Raycast(rayPoint.position, rayPoint.TransformDirection(new Vector3(-1, 0, 0)), 10, ~ignoreMask);
	

		if (hit.collider != null )
		{
			if (hit.collider.tag == "Player")
			{
				result = true;
				Debug.DrawLine(rayPoint.position, hit.point, Color.green);
				playerPos = hit.collider.transform.position;
			}
			else
			{
				Debug.DrawLine(rayPoint.position, hit.point, Color.blue);
				result = false;
			}

		}
		else
		{
			Debug.DrawRay(rayPoint.position, hit.point, Color.red);
			result = false;
		}
		return result;
	}
	// Update is called once per frame
	void Update()
    {
		if (Scan())
		{
			animator.SetBool("Run", true);
			animator.SetBool("Stop", false);
			animator.SetBool("Walk", false);
			transform.position = Vector2.MoveTowards(transform.position, (new Vector2(playerPos.x, transform.position.y)), Time.deltaTime * spead * 1.3f);
		}
		else if(!pause)
		{
			animator.SetBool("Run", false);
			if ((transform.position.x == rightPoint.position.x  && goRight)|| (transform.position.x == leftPoint.position.x && !goRight))
			{
				animator.SetBool("Stop", true);
				animator.SetBool("Walk", false);
				StartCoroutine(Pause());
				goRight = !goRight;
				transform.Rotate(new Vector3(0, -180, 0));
			}
			else
			{
				animator.SetBool("Stop", false);
				animator.SetBool("Walk", true);
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
			collision.GetComponent<Rigidbody2D>().velocity = Vector2.up * 10;
			pause = true;
			animator.SetTrigger("Hit");
			GetComponent<Rigidbody2D>().velocity = Vector2.up * 12;
			GetComponent<BoxCollider2D>().enabled = false;
			GetComponent<Rigidbody2D>().gravityScale = 5;
			Destroy(gameObject, 2f);
		}
	}
}
