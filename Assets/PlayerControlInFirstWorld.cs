using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
	[SerializeField] float PlayerSpeed;
	private bool PlayerRight = true;
	private AudioSource walkAudio;
	private bool musicIsPlay;

	private Animator animator;
	// Start is called before the first frame update
	void Start()
	{
        walkAudio= GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		transform.position = new Vector2(transform.position.x + Input.GetAxis("Horizontal") * Time.deltaTime * PlayerSpeed, transform.position.y);
		if (Input.GetAxis("Horizontal") < 0)
		{
			if (PlayerRight)
			{
				Flip();
			}
		}
		else if (Input.GetAxis("Horizontal") > 0)
		{
            if (!PlayerRight)
			{
				Flip();
			}
		}
		else
		{
			musicIsPlay = false;
            walkAudio.Stop();
            animator.SetBool("isWalk", false);

		}
		if (Input.GetAxis("Horizontal") != 0)
        {
            animator.SetBool("isWalk", true);
            if (PlayerDataScript.soundsOn)
            {
                if (musicIsPlay == false)
                {
                    walkAudio.Play();
                    musicIsPlay = true;
                }
            }
        }

	}
	private void Flip()
	{

		PlayerRight = !PlayerRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
