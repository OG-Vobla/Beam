using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformScript : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			Invoke("FallPlatform", 2f);
		}
	}
	private void FallPlatform()
	{
		GetComponent<Animator>().enabled = false;
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<Rigidbody2D>().gravityScale = 5;
		GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
		Destroy(gameObject, 2f);
	}
}
