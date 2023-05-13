using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineTrapScript : MonoBehaviour
{
	private Animator animator;
	private AudioSource audioSource1;
	private void Start()
	{
		audioSource1= GetComponent<AudioSource>();	
		animator = GetComponent<Animator>();
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			if (PlayerDataScript.soundsOn)
			{
                audioSource1.Play();
            }
            animator.SetBool("jump", true);

		}
	}
	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			animator.SetBool("jump", false);

		}
	}
}
