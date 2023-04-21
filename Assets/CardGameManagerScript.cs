using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class CardGameManagerScript : MonoBehaviour
{
	[SerializeField]  private List<GameObject> CardsPrefab = new List<GameObject>();
	  private List<GameObject> EnemyDeck = new List<GameObject>();
    [SerializeField] private Transform EnemyArm;
    [SerializeField] private Transform PlayerArm;
    [SerializeField] private List<Transform> EnemyCardSlots;
    [SerializeField] private List<Transform> PlayerCardSlots;

    private GameObject enemyCard;
    private GameObject playerCard;
    public static bool playerCanMove;
    private int strick = 0;
	void Start()
    {
        PlayerDataScript.PlayerDeck = new List<string>() { "Rook", "Paper", "Cutter" };
        PlayerDataScript.EnemyDeck = new List<string>() { "Rook", "Rook", "Cutter" };
        InstantiateCards();

	}
    private void InstantiateCards()
    {
		for (int i = 0; i < 3; i++)
		{
			var newEnemyCard = Instantiate(FindCardOfType(PlayerDataScript.EnemyDeck[i], CardsPrefab), EnemyCardSlots[i]);
			EnemyDeck.Add(newEnemyCard);
			var newPlayerCard = Instantiate(FindCardOfType(PlayerDataScript.PlayerDeck[i], CardsPrefab), PlayerCardSlots[i]);

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
	// Update is called once per frame
	void Update()
    {
		CheckWin();

	}
    private void CheckWin()
    {
		if (EnemyDeck.Count == 0 && enemyCard == null)
		{
			if (strick > 0)
			{
				Debug.Log("Win");
			}
			else if (strick < 0)
			{
				Debug.Log("Lose");
			}
			else if (strick == 0)
			{
				Debug.Log("Draw");
			}
		}
	}

	private int CalculateRound(string winCardType, string loseCardType)
    {
        var enemy = enemyCard.GetComponent<CardScript>().cardType;

		if (enemy == winCardType)
        {
            return -1;
        }
        else if (enemy == loseCardType)
        {
			return 1;
		}
        else
        {
            return 0;
        }
    }
    private void EnemyPlayCard()
    {
		string cardType = "";
		if (playerCard == null)
		{
			cardType = GetRandomCardType();
		}
		else
		{
			switch (playerCard.GetComponent<CardScript>().cardType)
			{
				case "Rook":
					cardType = FindCardOfTypeInEnemyDeck("Paper", "Rook");
					break;
				case "Cutter":
					cardType = FindCardOfTypeInEnemyDeck("Rook", "Cutter");
					break;
				case "Paper":
					cardType= FindCardOfTypeInEnemyDeck("Cutter", "Paper");
					break;
			}
		}
		PlayCard(FindCardOfType(cardType, EnemyDeck),true);
    }
	private string FindCardOfTypeInEnemyDeck(string winCardType, string drawCardType)
	{
		var newCardtype = FindCardOfType(winCardType, EnemyDeck);
		if (newCardtype == null)
		{
			newCardtype = FindCardOfType(drawCardType, EnemyDeck);
			if (newCardtype == null)
			{
				return GetRandomCardType();
			}
		}
		return newCardtype.GetComponent<CardScript>().cardType;
	}
	private string GetRandomCardType()
	{
		return EnemyDeck[Random.Range(0, EnemyDeck.Count)].GetComponent<CardScript>().cardType;
	}
	public void PlayCard(GameObject card, bool isEnemy)
	{
        if (isEnemy)
        {
            var newCard = Instantiate(card, EnemyArm);
			newCard.transform.localPosition= Vector3.zero;
			enemyCard = newCard;
			EnemyDeck.Remove(card);
		}
        else
        {
			var newCard = Instantiate(card, PlayerArm);
			newCard.transform.localPosition = Vector3.zero;
			playerCard = newCard;
			PlayerDataScript.PlayerDeck.Remove(card.GetComponent<CardScript>().cardType);
		}
		Destroy(card);
		if (playerCard == null)
        {
			playerCanMove = true;
		}
        else if (enemyCard == null)
        {
            EnemyPlayCard();
		}
        else
        {
            switch (playerCard.GetComponent<CardScript>().cardType)
            {
                case "Rook":
                    strick =+ CalculateRound("Paper", "Cutter");
					break;
				case "Cutter":
					strick = +CalculateRound("Rook", "Paper");
					break;
				case "Paper":
					strick = +CalculateRound("Cutter", "Cutter");
					break;
			}
            Destroy(playerCard, 2f);
            Destroy(enemyCard, 2f);
		}
		
	}
}
