using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
	public string Name;
	public bool isComplete;
}
public class PlayerDataScript : MonoBehaviour
{
	public static int Money = 9999;
	public static List<string> PlayerDeck = new List<string>() { "Rook", "Paper", "Cutter" };
	public static List<string> EnemyDeck = new List<string>() { "Rook", "Rook", "Cutter" };
	public static int PlatformerLvl;
	public static List<Quest> questNames;
	public static bool questComplete;
	private void Start()
	{
		questNames = new List<Quest>();
		PlayerDeck = new List<string>() { "Rook", "Paper", "Cutter" };
		EnemyDeck = new List<string>() { "Rook", "Rook", "Cutter" };
		DontDestroyOnLoad(this);
	}

}
