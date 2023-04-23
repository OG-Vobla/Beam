using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
	public bool isPlayerCard = true;
	[SerializeField] public string cardType;
	private CardGameManagerScript gm;
	private void Start()
	{
		gm = FindObjectOfType<CardGameManagerScript>();
	}
	public void PlayCard()
	{
		
		if (isPlayerCard && gm.playerCanMove)
		{
			
			StartCoroutine(gm.PlayCard(gameObject, false));
		}
	}
}
