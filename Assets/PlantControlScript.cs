using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class PlantControlScript : BeeControlScript
{
	public int turn = -1;
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        transform.localScale = new Vector2(transform.localScale.x * -turn, transform.localScale.y) ;
	}
	void Update()
	{
		if (!pause)
		{
            if (PlayerDataScript.soundsOn)
            {
                audioSource.Play();
            }

            animator.SetTrigger("Attack");
			var bull = Instantiate(bulletPrefab, transform);
			bull.SetActive(true);
			bull.GetComponent<Rigidbody2D>().velocity = new Vector2(turn, 0) * 12;
			Destroy(bull, 4f);
			animator.SetTrigger("Attack");
			StartCoroutine(Pause());
		}
	}

}
