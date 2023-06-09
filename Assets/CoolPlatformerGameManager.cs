using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoolPlatformerGameManager : MonoBehaviour
{
    public int lives = 3;
    [SerializeField] List<GameObject> CheckPoints;
	[SerializeField] GameObject Player;
	[SerializeField] GameObject LivePrefab;
	[SerializeField] Transform LivesTransform;
	public int index = 0;
	private void Start()
	{
		PlayerSpawn();
		for (int i = 0; i < lives; i++)
		{
			Instantiate(LivePrefab, LivesTransform);
		}
	}
	private void Update()
	{

	}
	private void PlayerSpawn()
	{
		Player.transform.position = CheckPoints[index].transform.position;
	}
	public void PlyerDie()
	{
		if (lives > 1)
		{
			Destroy(LivesTransform.GetChild(0).gameObject);
			lives -= 1;
			PlayerSpawn();
		}
		else
        {
            SceneManager.LoadScene("PlatformerScene");
		}
	}
}
