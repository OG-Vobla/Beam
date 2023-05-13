using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
	public bool isActive = false;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!isActive)
		{
		 	GameObject.FindGameObjectWithTag("GameManager").GetComponent<CoolPlatformerGameManager>().index += 1;
			GetComponent<Animator>().SetBool("Enter", true);
			isActive = true;
		}

	}
}
