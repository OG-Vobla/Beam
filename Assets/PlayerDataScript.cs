using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataScript : MonoBehaviour
{
	public static int Money = 9999;
	public static List<string> PlayerDeck = new List<string>() { "Rook", "Paper", "Cutter" };
	public static List<string> EnemyDeck = new List<string>() { "Rook", "Rook", "Cutter" };
	public static int PlatformerLvl;
	private void Start()
	{
		PlayerDeck = new List<string>() { "Rook", "Paper", "Cutter" };
		EnemyDeck = new List<string>() { "Rook", "Rook", "Cutter" };
		DontDestroyOnLoad(this);
	}

}
