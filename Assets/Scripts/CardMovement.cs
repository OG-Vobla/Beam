using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardMovement : MonoBehaviour
{

	public bool hasBeenPlayed;
	public int handIndex;
	public TMP_Text attackText;
	public TMP_Text healthText;

	CardGameManager gm;

	private Animator anim;

	public GameObject healEffect;
	public GameObject takeDamageEffect;
	public GameObject dieEffect;
	public GameObject hollowCircle;

	public Card selfCard;

	private void Start()
	{
		gm = FindObjectOfType<CardGameManager>();
		anim = GetComponent<Animator>();
	}
	private void Update()
	{
		attackText.text = selfCard.attackPoints.ToString();
		healthText.text = selfCard.healthPoints.ToString();
		if (selfCard.healthPoints <= 0)
		{
			Instantiate(dieEffect, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
	private void OnMouseDown()
	{
		if (!hasBeenPlayed && gm.canMove)
		{
			Instantiate(hollowCircle, transform.position, Quaternion.identity);
			anim.SetTrigger("move");
			transform.position += Vector3.up * 3f;
			hasBeenPlayed = true;
			gm.PlayCard(this, false);
		}
	}
	public void Heal(int points)
	{
		selfCard.healthPoints += points;
		Instantiate(healEffect, transform.position, Quaternion.identity);
	}
	public void TakeDamage(int points)
	{
		selfCard.healthPoints -= points;
		Instantiate(takeDamageEffect, transform.position, Quaternion.identity);
	}
	/*
		void MoveToDiscardPile()
		{
			Instantiate(effect, transform.position, Quaternion.identity);
		}*/


}
