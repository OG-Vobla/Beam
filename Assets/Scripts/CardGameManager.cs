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
	public List<GameObject> enemyCards;
	public List<CardMovement> enemyDeck;
	public List<CardMovement> playerDeck;
	public TextMeshProUGUI deckSizeText;

	public Transform[] cardSlots;
	public bool[] availableCardSlots;

	public List<CardMovement> enemyDiscardPile;
	public List<CardMovement> playerDiscardPile;

	public TextMeshProUGUI discardPileSizeText;
	public bool canMove = true;
	public int enemPlayCardIndex = 0;
	public int playerPlayCardIndex = 0;
	public bool playerEnd = true;
	public bool gameEnd = false;
	public int playerCardIndex = 0;
	public int enemyCardIndex = 0;

	private void Start()
	{
		enemyCards = cardPrefabs;
		playerPlayCardIndex = playerDeck.Count;
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
						Invoke("EnemyPlayCard" , 2f);
						return;
					}
				}
			}
		}

	}

	public void GenerateEnemyDeck()
	{
		List<GameObject> newDeck = cardPrefabs;
		for (int i = 0; i < playerDeck.Count; i++)
		{
			var newCard = newDeck[UnityEngine.Random.Range(0, newDeck.Count)];
			enemyCards.Add(newCard);
			newDeck.Remove(newCard);
		}
/*		GameObject newCard = Instantiate(enemyCards[UnityEngine.Random.Range(0, enemyCards.Count)], cardSlots[playerDiscardPile.Count]);
		newCard.transform.position += Vector3.up * 5.5f;
		enemyDeck.Add(newCard.GetComponent<CardMovement>());
		newCard.SetActive(false);
		enemyCards.Remove(newCard);*/
		canMove = true;
	}
	public void EnemyPlayCard()
	{
		if (!gameEnd)
		{
			Debug.Log("dfsdf");
			if (enemyDeck.Count >= 1)
			{
				CardMovement randomCard = enemyDeck[UnityEngine.Random.Range(0, enemyDeck.Count)];
				randomCard.gameObject.SetActive(true);
				enemyDeck.Remove(randomCard);
				PlayCard(randomCard, true);
			}
			else
			{
				GenerateEnemyDeck();
				GameObject newCard = Instantiate(enemyCards[UnityEngine.Random.Range(0, enemyCards.Count)], cardSlots[enemPlayCardIndex]);
				newCard.transform.position += Vector3.up * 5.5f;
				newCard.GetComponent<CardMovement>().isEnemyCard = true;
				enemyDeck.Add(newCard.GetComponent<CardMovement>());
				newCard.SetActive(false);
				enemPlayCardIndex = enemPlayCardIndex + 1;
			}
			if (playerPlayCardIndex == playerDiscardPile.Count)
			{
				gameEnd = true;
				StartCoroutine(AutoEndGame());
			}
		}
		
	}
	public void PlayCard(CardMovement card, bool isEnemy)
	{
		List<CardMovement> newEnemyDiscardPile = new List<CardMovement>();
		List<CardMovement> newPlayerDiscardPile = new List<CardMovement>();
		if (isEnemy)
		{
			newEnemyDiscardPile = playerDiscardPile;
			newPlayerDiscardPile = enemyDiscardPile;
		}
		else
		{
			newEnemyDiscardPile = enemyDiscardPile;
			newPlayerDiscardPile = playerDiscardPile;
		}
			Perk cardPerk = card.selfCard.perk;
			Card playerCard = card.selfCard;
			List<CardMovement> cards = new List<CardMovement>();	
			if (cardPerk.forAllCards)
			{
				cards.AddRange(newEnemyDiscardPile);
				cards.AddRange(newPlayerDiscardPile);
			}
			else if (cardPerk.forEnemyCards)
			{
				cards.AddRange(newEnemyDiscardPile);
			}
			else if (cardPerk.forAlliesCards)
			{
				cards.AddRange(newPlayerDiscardPile);
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
			foreach (CardMovement enemyCard in newEnemyDiscardPile)
			{
				if ((enemyCard.selfCard.type == "Rook" && playerCard.type == "Paper") || (enemyCard.selfCard.type == "Cutter" && playerCard.type == "Rook") || (enemyCard.selfCard.type == "Paper" && playerCard.type == "Cutter"))
				{
					enemyCard.TakeDamage(playerCard.attackPoints);
				}
			}


		if (!gameEnd)
		{
			newPlayerDiscardPile.Add(card);
			Debug.Log(";pl,pomp");
			if (isEnemy)
			{
				
				playerDiscardPile = newEnemyDiscardPile;
				enemyDiscardPile = newPlayerDiscardPile;
				canMove = true;
			}
			else
			{
				enemyDiscardPile = newEnemyDiscardPile;
				playerDiscardPile = newPlayerDiscardPile;
				canMove = false;
				Invoke("EnemyPlayCard", 2f);
			}
		}
		
	}

	private void Update()
	{
		deckSizeText.text = playerDeck.Count.ToString();
	}
	public IEnumerator AutoEndGame()
	{
		int playIndex = 2;

		while (enemyDiscardPile.Count != 0 || playerDiscardPile.Count != 0)
		{

			if (playIndex % 2 == 1)
			{
				PlayCard(playerDiscardPile[playerDiscardPile.Count-1 < playerCardIndex ? playerCardIndex = 0: playerCardIndex] , false);
				playerCardIndex += 1;
			}
			else
			{
				PlayCard(enemyDiscardPile[enemyDiscardPile.Count - 1 < enemyCardIndex ? enemyCardIndex = 0 : enemyCardIndex], true);	
				enemyCardIndex += 1;
			}
			if (enemyCardIndex == enemyDiscardPile.Count )
			{
				Debug.Log("sdfssgddfsg");
				enemyCardIndex = 0;
			}
			if (playerCardIndex == playerDiscardPile.Count)
			{
				Debug.Log("sdfssgddfsg");
				playerCardIndex = 0;
			}
			playIndex += 1;

			yield return new WaitForSeconds(1f);
		}

	}
}
