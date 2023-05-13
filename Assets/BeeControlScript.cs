using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeControlScript : MonoBehaviour
{
	protected Animator animator;
	protected bool pause = false;
	[SerializeField] protected float pauseTime;
	protected GameObject bulletPrefab;
    private GameObject dieSound;
    private AudioSource audioSource;
    // Start is called before the first frame update
    protected void Start()
    {
        audioSource = GetComponent<AudioSource>();
        dieSound = GameObject.FindGameObjectWithTag("DieSound");
		animator = GetComponent<Animator>();
		bulletPrefab = transform.Find("Bullet").gameObject;
	}

    // Update is called once per frame
    void Update()
    {
		if (!pause)
        {
            if (PlayerDataScript.soundsOn)
            {
                audioSource.Play();
            }
            animator.SetTrigger("Attack");
			var bull = Instantiate(bulletPrefab,transform);
			bull.SetActive(true);
			bull.GetComponent<Rigidbody2D>().gravityScale = 2;
			bull.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
			Destroy(bull, 5f);
			animator.SetTrigger("Attack");
			StartCoroutine(Pause());
		}
    }
	protected IEnumerator Pause()
	{
		pause = true;
		yield return new WaitForSeconds(pauseTime);
		pause = false;
	}
	protected void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
        {
            if (PlayerDataScript.soundsOn)
            {
                dieSound.GetComponent<AudioSource>().Play();
            }
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.up * 10;
			pause = true;
			animator.SetTrigger("Hit");
			GetComponent<Rigidbody2D>().velocity = Vector2.up * 12;
			GetComponent<BoxCollider2D>().enabled = false;
			GetComponent<Rigidbody2D>().gravityScale = 5;
			Destroy(gameObject, 2f);
		}
	}
}
