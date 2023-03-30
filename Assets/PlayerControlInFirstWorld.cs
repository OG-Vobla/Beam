using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float PlayerSpeed;
	private bool PlayerRight = true;
	// Start is called before the first frame update
	void Start()
    {
        
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
            GetComponent<Animator>().SetBool("isWalk", true);
        }
		else if (Input.GetAxis("Horizontal") > 0)
		{
			if (!PlayerRight)
			{
				Flip();
			}
			GetComponent<Animator>().SetBool("isWalk", true);
		}
		else
		{
			GetComponent<Animator>().SetBool("isWalk", false);

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
