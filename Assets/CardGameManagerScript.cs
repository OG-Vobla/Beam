using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	[SerializeField] private TMP_Text GameText;
	

	private GameObject enemyCard;
    private GameObject playerCard;
    public bool playerCanMove;
    private int strick = 0;
	private bool endGame = false;

	void Start()
	{
		GameText.text = "Бой начался";
        InstantiateCards();
        StartCoroutine(PlayerCanMoveActiavateWithDelay());
    }
	void Update()
	{
		CheckWin();
		if (endGame == false)
		{
            if (playerCanMove)
            {
                GameText.text = "Сделайте ход!";
            }
            else
            {
                GameText.text = "Обдумайте ход!";
            }
        }
		
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
            endGame = true;
            if (strick > 0)
			{
				PlayerDataScript.strick = 2;
				if (PlayerDataScript.DefeatNpcs.Where(element => element == PlayerDataScript.FightNpcs).Count() == 0 ||  PlayerDataScript.DefeatNpcs.Count == 0)
				{
					PlayerDataScript.DefeatNpcs.Add(PlayerDataScript.FightNpcs);

                }
                GameText.text = "Вы выйграли";
			}
			else if (strick < 0)
			{
				PlayerDataScript.strick = 1;
				GameText.text = "Вы проиграли";
            }
			else if (strick == 0)
			{
                PlayerDataScript.strick = 1;
                GameText.text = "Ничья";
			}
			StartCoroutine(LoadSceneWithDelay());
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
	private IEnumerator LoadSceneWithDelay()
	{
		yield return new WaitForSeconds(2);
		SceneManager.LoadScene("OpenWorld");
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
            EnemyPlayCard();
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
			StartCoroutine(PlayerCanMoveActiavateWithDelay());
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
					strick = strick + result;
					break;
				case "Cutter":
					result = CalculateRound("Rook", "Paper");
					strick = strick + result;
					break;
				case "Paper":
					result = CalculateRound("Cutter", "Rook");
					strick = strick + result; 
					break;
			}
			Debug.Log(result);
			Debug.Log(strick);
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
		}
		
	}
	private IEnumerator PlayerCanMoveActiavateWithDelay()
	{
		yield return new WaitForSeconds(3f);
			
			playerCanMove = true;

	}
}
