using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FirstWorldGameManager : MonoBehaviour
{

	[SerializeField] private List<GameObject> CardsPrefab = new List<GameObject>();
	[SerializeField] public int cardCost;
	[SerializeField] public TMP_Text cardCostText;
	[SerializeField] public TMP_Text playerMoneyText;
	[SerializeField] private List<Transform> PlayerCardSlots;
	private void Start()
	{
		InstantiateCards();
	}
	private void Update()
	{
		cardCostText.text = cardCost.ToString();
		playerMoneyText.text = PlayerDataScript.Money.ToString();
	}
	private void InstantiateCards()
	{
		for (int i = 0; i < 3; i++)
		{
			if (PlayerCardSlots[i].childCount != 0)
			{
				Destroy(PlayerCardSlots[i].GetChild(0).gameObject);
			}
		}
		for (int i = 0; i < PlayerDataScript.PlayerDeck.Count; i++)
		{
			var newPlayerCard = Instantiate(FindCardOfType(PlayerDataScript.PlayerDeck[i], CardsPrefab), PlayerCardSlots[i]);
			newPlayerCard.GetComponent<CardScript>().enabled = false;
		}
	}
	private GameObject FindCardOfType(string cardType, List<GameObject> cards)
	{
		foreach (var card in cards)
		{
			if (card.GetComponent<CardScript>().cardType == cardType)
			{
				return card;
			}
		}
		return null;
	}
	public void BuyCard(string cardType)
	{
		if (PlayerDataScript.PlayerDeck.Count <3)
		{
			if (PlayerDataScript.Money >= PlayerDataScript.Money - cardCost)
			{
				PlayerDataScript.PlayerDeck.Add(cardType);
				PlayerDataScript.Money = PlayerDataScript.Money - cardCost;
				InstantiateCards();
			}
		}
	}
	public void SellCard(int place)
	{
		if (PlayerDataScript.PlayerDeck.Count > 0)
		{
			PlayerDataScript.PlayerDeck.Remove(PlayerDataScript.PlayerDeck[place]);
			PlayerDataScript.Money = PlayerDataScript.Money + cardCost;
			InstantiateCards();
		}
	}
}
