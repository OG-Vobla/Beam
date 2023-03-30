using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxEffectScript : MonoBehaviour
{

	[SerializeField] GameObject cam;
	[SerializeField] float Parallax;
	float startPosX;

	private void Start()
	{
		startPosX= transform.position.x;
	}
	private void Update()
	{
		float distX = (cam.transform.position.x * (1 - Parallax));
		transform.position = new Vector3(startPosX + distX, transform.position.y, transform.position.z);
	}
}
