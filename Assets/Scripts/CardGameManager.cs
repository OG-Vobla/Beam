using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Runtime.CompilerServices;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;

[Serializable]
public class Perk
{
	public bool forAllCards;
	public bool forAlliesCards;
	public bool forEnemyCards;
	public string[] typeCards;
	public bool isHeal;
	public int points;

	public Perk(bool forAllCards, bool forAlliesCards, bool forEnemyCards, string[] typeCards, bool isHeal,int  points)
	{
		this.forAllCards = forAllCards;
		this.forAlliesCards = forAlliesCards;
		this.forEnemyCards = forEnemyCards;
		this.typeCards = typeCards;
		this.isHeal = isHeal;
		this.points = points;
	}
}
[Serializable]
public class Card
{
	public int attackPoints;
	public int healthPoints;
	public string type;
	public Perk perk;

	public Card(int attackPoints, int healthPoints, Perk perk, string type)
	{
		this.attackPoints = attackPoints;
		this.healthPoints = healthPoints;
		this.perk = perk;
		this.type = type;
	}
}

public class CardGameManager : MonoBehaviour
{
	public List<GameObject> cardPrefabs;
	public List<CardMovement> enemyDeck;
	public List<CardMovement> playerDeck;
	public TextMeshProUGUI deckSizeText;

	public Transform[] cardSlots;
	public bool[] availableCardSlots;

	public List<CardMovement> enemyDiscardPile;
	public List<CardMovement> playerDiscardPile;

	public TextMeshProUGUI discardPileSizeText;
	public bool canMove = true;

	private void Start()
	{
		GenerateEnemyDeck();
	}

	public void DrawCard()
	{
		if (canMove)
		{
			if (playerDeck.Count >= 1)
			{
				CardMovement randomCard = playerDeck[UnityEngine.Random.Range(0, playerDeck.Count)];
				for (int i = 0; i < availableCardSlots.Length; i++)
				{
					if (availableCardSlots[i] == true)
					{
						randomCard.gameObject.SetActive(true);
						randomCard.handIndex = i;
						randomCard.transform.position = cardSlots[i].position;
						randomCard.hasBeenPlayed = false;
						playerDeck.Remove(randomCard);
						availableCardSlots[i] = false;
						canMove = false;
						return;
					}
				}
			}
		}

	}

	public void GenerateEnemyDeck()
	{
		List<GameObject> newDeck = cardPrefabs;
		for (int i = 0; i< playerDeck.Count; i++)
		{
			var newCard = Instantiate(newDeck[UnityEngine.Random.Range(0, newDeck.Count)], transform);
			enemyDeck.Add(newCard.GetComponent<CardMovement>());
			newCard.SetActive(false);
			newDeck.Remove(newCard);
		}
	}
	public void EnemyPlayCard()
	{

	}
	public void PlayerPlayCard(CardMovement card)
	{
		if (canMove)
		{
			Perk cardPerk = card.selfCard.perk;
			Card playerCard = card.selfCard;
			List<CardMovement> cards = new List<CardMovement>();	
			if (cardPerk.forAllCards)
			{
				cards.AddRange(enemyDiscardPile);
				cards.AddRange(playerDiscardPile);
			}
			else if (cardPerk.forEnemyCards)
			{
				cards.AddRange(enemyDiscardPile);
			}
			else if (cardPerk.forAlliesCards)
			{
				cards.AddRange(playerDiscardPile);
			}
			foreach (CardMovement discCardScript in cards)
			{
				Card discCard = discCardScript.selfCard;
				if (cardPerk.typeCards.Contains(discCard.type))
				{
					if (cardPerk.isHeal)
					{
						discCardScript.Heal(cardPerk.points);
					}
					else
					{
						discCardScript.TakeDamage(cardPerk.points);
					}
				}
			}
			foreach (CardMovement enemyCard in enemyDiscardPile)
			{
				if ((enemyCard.selfCard.type == "Rook" && playerCard.type == "Paper") || (enemyCard.selfCard.type == "Cutter" && playerCard.type == "Rook") || (enemyCard.selfCard.type == "Paper" && playerCard.type == "Cutter"))
				{
					enemyCard.TakeDamage(playerCard.attackPoints);
				}
			}
			playerDiscardPile.Add(card);
			canMove = false;
		}
	}

	private void Update()
	{
		deckSizeText.text = playerDeck.Count.ToString();
	}

}
