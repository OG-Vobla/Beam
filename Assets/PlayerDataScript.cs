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
	public static int Money;
	public static List<string> PlayerDeck = new List<string>() { };
	public static List<string> EnemyDeck = new List<string>() { };
	public static List<string> DefeatNpcs = new List<string>() {"","","" };
	public static string FightNpcs;
	public static int PlatformerLvl;
	public static int strick = 0;
	public static bool gameIsLoad;

}
