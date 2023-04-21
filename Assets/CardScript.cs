using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
	[SerializeField] public string cardType;
	public void PlayCard()
	{
		FindObjectOfType<CardGameManagerScript>().PlayCard(gameObject, false);
	}
}
