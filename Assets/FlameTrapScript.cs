using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameTrapScript : MonoBehaviour
{
    public bool allTimeOn;
    public float onTime;
    public float breakTime;
    private float time = 0;
    public bool isOn;
    [SerializeField] PolygonCollider2D fireCollider;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator= GetComponent<Animator>();
        if (allTimeOn)
        {
			animator.SetBool("isOn", true);
			fireCollider.enabled= true;
		}
    }

    // Update is called once per frame
    void Update()
    {
        if (!allTimeOn)
        {
			if (time == 0)
			{
				isOn = false;
				time = onTime;
				animator.SetBool("isOn", isOn);
				fireCollider.enabled = false;
				Invoke("startTimer", breakTime);
			}
		}
        
    }
    void startTimer()
	{
		isOn = true;
		animator.SetBool("isOn", isOn);
		fireCollider.enabled = true;
		StartCoroutine(timer());
	}
    IEnumerator timer()
    {
		while (isOn)
        {
            time--;
			yield return new WaitForSeconds(1f);
        }
    }
}
