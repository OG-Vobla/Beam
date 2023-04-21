using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataScript : MonoBehaviour
{
	public static int Money = 0;
	public static List<string> PlayerDeck = new List<string>();
	public static List<string> EnemyDeck = new List<string>();
	public static int PlatformerLvl;
	private void Start()
	{
		DontDestroyOnLoad(this);
	}

}
