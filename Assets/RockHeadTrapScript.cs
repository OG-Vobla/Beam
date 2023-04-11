using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockHeadTrapScript : MonoBehaviour
{
	public int index = 0;
	public string angle;
	[SerializeField] private List<Transform> points;
	private Animator animator;
	public bool pause = false;
	public bool isVertical = false;
	[SerializeField] private float spead;
	[SerializeField] private float pauseTime;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}
	// Update is called once per frame
	void Update()
	{
		if (!pause)
		{
			if (transform.position.x != points[index].position.x || transform.position.y != points[index].position.y)
			{
				transform.position = Vector2.MoveTowards(transform.position, points[index].position, spead * Time.deltaTime);
				animator.SetBool("isBlink", true);
			}
			if (transform.position.x == points[index].position.x && transform.position.y == points[index].position.y)
			{
				if (points.Count == 2)
				{
					if (index == 0)
					{
						if (isVertical)
						{
							angle = "top";
						}
						else
						{
							angle = "left";
						}
					}
					else
					{
						if (isVertical)
						{
							angle = "bottom";
						}
						else
						{
							angle = "right";
						}
					}
				}
				else
				{
					angle = index == 0 ? "top" : (index == 1 ? "right" : (index == 2 ? "bottom" : (index == 3 ? "left" : (""))));
				}
				StartCoroutine(Pause());
				index++;
				if (index >= points.Count)
				{
					index = 0;
				}
			}
		}
		
	}
	private IEnumerator Pause()
	{
		animator.SetTrigger(angle);
		animator.SetBool("isBlink", false);
		pause = true;
		yield return new WaitForSeconds(pauseTime);
		pause = false;
	}
}
