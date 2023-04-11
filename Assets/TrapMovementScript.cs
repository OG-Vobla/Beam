using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SawTrapScript : MonoBehaviour
{
	private bool goBack = false;
	private int index = 0;
	[SerializeField] private List<Transform> points;
	[SerializeField] private float spead;
	[SerializeField] private float pauseTime;
	private bool pause = false;
	

	// Update is called once per frame
	void Update()
	{
		if (!pause)
		{
			if (transform.position.x != points[index].position.x || transform.position.y != points[index].position.y)
			{
				transform.position = Vector2.MoveTowards(transform.position, points[index].position, spead * Time.deltaTime);
			}
			if (transform.position.x == points[index].position.x && transform.position.y == points[index].position.y)
			{
				StartCoroutine(Pause());
				if (goBack)
				{
					index--;
				}
				else
				{
					index++;
				}
				if (index == points.Count)
				{
					goBack = true;
					index = points.Count - 1;
				}
				if (index < 0)
				{
					goBack = false;
					index = 0;
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
}
