using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGameManagerScript : MonoBehaviour
{
	[SerializeField]  private List<GameObject> CardsPrefab = new List<GameObject>();
	  private List<GameObject> EnemyDeck = new List<GameObject>();
    [SerializeField] private Transform EnemyArm;
    [SerializeField] private Transform PlayerArm;
    [SerializeField] private List<Transform> EnemyCardSlots;
    [SerializeField] private List<Transform> PlayerCardSlots;
    [SerializeField] private GameObject DieEffect;
	[SerializeField] private GameObject WinEffect;
	

	private GameObject enemyCard;
    private GameObject playerCard;
    public bool playerCanMove;
    private int strick = 0;
	private bool move = false;
	void Start()
	{
		StartCoroutine(PlayerCanMoveActiavateWithDelay());
        InstantiateCards();

	}
	void Update()
	{
		CheckWin();

	}
	private void InstantiateCards()
    {
		for (int i = 0; i < 3; i++)
		{
			var newEnemyCard = Instantiate(FindCardOfType(PlayerDataScript.EnemyDeck[i], CardsPrefab), EnemyCardSlots[i]);
			newEnemyCard.GetComponent<CardScript>().isPlayerCard = false;
			newEnemyCard.transform.Find("TypeImage").gameObject.SetActive(false);
			EnemyDeck.Add(newEnemyCard);
			var newPlayerCard = Instantiate(FindCardOfType(PlayerDataScript.PlayerDeck[i], CardsPrefab), PlayerCardSlots[i]);
			newPlayerCard.GetComponent<CardScript>().isPlayerCard = true;

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

		playerCanMove = false;
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
		StartCoroutine(PlayCard(FindCardOfType(cardType, EnemyDeck),true));
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
	public IEnumerator PlayCard(GameObject card, bool isEnemy)
	{
		card.GetComponent<CardScript>().isPlayerCard = false;
		Animator animator = card.GetComponent<Animator>();
		if (isEnemy)
		{
			animator.SetTrigger("EnemyCardHide");
			yield return new WaitForSeconds(1f);
			card.SetActive(false);
			card.transform.SetParent(EnemyArm);
			card.transform.Find("TypeImage").gameObject.SetActive(true);
			card.transform.localPosition= Vector3.zero;
			enemyCard = card;
			EnemyDeck.Remove(card);
			card.SetActive(true);
			animator.SetTrigger("EnemyCardShow");
		}
        else
		{
			playerCanMove = false;
			animator.SetTrigger("PlayerCardHide");
			yield return new WaitForSeconds(1f);
			card.SetActive(false);
			card.transform.SetParent(PlayerArm);
			card.transform.localPosition = Vector3.zero;
			playerCard = card;
			PlayerDataScript.PlayerDeck.Remove(card.GetComponent<CardScript>().cardType);
			card.SetActive(true);
			animator.SetTrigger("PlayerCardShow");
		}

		if (playerCard == null)
        {
		}
        else if (enemyCard == null)
		{
		}
        else
        {
			int result = 0;
            switch (playerCard.GetComponent<CardScript>().cardType)
            {
                case "Rook":
					result = CalculateRound("Paper", "Cutter");
					strick =+ result;
					break;
				case "Cutter":
					result = CalculateRound("Rook", "Paper");
					strick = +result;
					break;
				case "Paper":
					result = CalculateRound("Cutter", "Rook");
					strick = + result; 
					break;
			}
			Debug.Log(result);
			if (result > 0)
			{
				Destroy(Instantiate(DieEffect, EnemyArm.transform), 3f);
				Destroy(Instantiate(WinEffect, PlayerArm.transform), 3f);
			}
			else if (result < 0)
			{
				Destroy(Instantiate(DieEffect, PlayerArm.transform), 3f);
				Destroy(Instantiate(WinEffect, EnemyArm.transform), 3f);
			}
			else
			{
				Destroy(Instantiate(DieEffect, PlayerArm.transform),3f);
				Destroy(Instantiate(DieEffect, EnemyArm.transform),3f);
			}
			Destroy(playerCard, 2f);
            Destroy(enemyCard, 2f);
			move = !move;
		}
		StartCoroutine(PlayerCanMoveActiavateWithDelay());

	}
	private IEnumerator PlayerCanMoveActiavateWithDelay()
	{
		yield return new WaitForSeconds(2f);
		if (move && playerCard == null)
		{
			move = false;
			playerCanMove = true;
		}
		else if (move == false && enemyCard == null && EnemyDeck.Count != 0)
		{
			move = true;
			EnemyPlayCard();
		}

	}
}
