using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveController : MonoBehaviour
{
	// Start is called before the first frame update
	public static SaveController instance;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(this);
		}
		DontDestroyOnLoad(this);
	}

public bool IsSaveFile() 
	{ 
		return Directory.Exists(Application.persistentDataPath + "/game_save");
	}

}
