using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerDataScript
{
	public static int Money;
	public static List<string> PlayerDeck = new List<string>() { };
	public static List<string> EnemyDeck = new List<string>() { };
	public static List<string> DefeatNpcs = new List<string>() {};
	public static string FightNpcs;
	public static int PlatformerLvl;
	public static int strick = 0;
	public static bool gameIsLoad;
	public static bool musicOn =  true;
	public static bool soundsOn = true;
	public static bool isCardGame ;

}
