using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformScript : MonoBehaviour
{
	Vector3 startPos;
    private void Start()
    {
		startPos= transform.position;
    }
    private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			Invoke("FallPlatform", 0.4f);
		}
	}
	private void FallPlatform()
	{
		GetComponent<Animator>().enabled = false;
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<Rigidbody2D>().gravityScale = 5;
		GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        Invoke("ReturnPlatform", 3f);
    }
	private void ReturnPlatform()
	{
        GetComponent<Animator>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
		transform.position = startPos;
    }
}
