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
	public TMP_Text takeDamgeText;
	public TMP_Text healText;

	CardGameManager gm;

	public Animator anim;

	public bool isEnemyCard = false;

	public GameObject healEffect;
	public GameObject takeDamageEffect;
	public GameObject dieEffect;
	public GameObject hollowCircle;

	public GameObject cloneEffect;

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
			cloneEffect = Instantiate(dieEffect, transform.position, Quaternion.identity);
			if (isEnemyCard)
			{
				gm.enemyDiscardPile.Remove(this);
			}
			else
			{
				gm.playerDiscardPile.Remove(this);
			}
			Destroy(cloneEffect, 1f);
			Destroy(gameObject);
		}
	}
	public void OnMouseDown()
	{
		if (!hasBeenPlayed && gm.canMove)
		{
			cloneEffect =Instantiate(hollowCircle, transform.position, Quaternion.identity);
			anim.SetTrigger("move");
			transform.position += Vector3.up * 3f;
			hasBeenPlayed = true;
			gm.PlayCard(this, false);
			Destroy(cloneEffect, 1f);
		}
	}
	public void Heal(int points)
	{
		selfCard.healthPoints += points;
		healText.text = $"+{points}";
		healText.gameObject.SetActive(true);
		StartCoroutine(SetActiveText(true));
		cloneEffect = Instantiate(healEffect, transform.position, Quaternion.identity);
		Destroy(cloneEffect, 1f);
	}
	public void TakeDamage(int points)
	{
		selfCard.healthPoints -= points;
		takeDamgeText.text = $"-{points}";
		takeDamgeText.gameObject.SetActive(true);
		StartCoroutine(SetActiveText(false));
		cloneEffect = Instantiate(takeDamageEffect, transform.position, Quaternion.identity);
		Destroy(cloneEffect, 1f);
	}
	public IEnumerator SetActiveText(bool isHeal)
	{
		yield return new WaitForSeconds(1);
		if (isHeal)
		{
			healText.gameObject.SetActive(false);
		}
		else
		{
			takeDamgeText.gameObject.SetActive(false);
		}
	}
	/*
		void MoveToDiscardPile()
		{
			Instantiate(effect, transform.position, Quaternion.identity);
		}*/


}
