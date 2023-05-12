using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class OpenWorldMenuManager : MonoBehaviour
{
    [SerializeField] private List<BasicInkExample> Npcs;
    [SerializeField] private Transform Player;  
    [SerializeField] private GameObject Menu;
    
    private void Awake()
    {
        if (PlayerPrefs.GetInt("playerIsSaveGame") == 1)
        {
            Load();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }
    }
    public void ExitMenu()
    {
        if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
            Menu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            Menu.SetActive(false);
        }
    }
    public void SaveGame()
    {

        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("playerX", Player.position.x);
        PlayerPrefs.SetInt("playerMoney", PlayerDataScript.Money);
        for (int i = 0; i < PlayerDataScript.PlayerDeck.Count;  i++)
        {
            PlayerPrefs.SetString(i + "Card", PlayerDataScript.PlayerDeck[i]);

        }    
        for (int i = 0; i < PlayerDataScript.DefeatNpcs.Count;  i++)
        {
            PlayerPrefs.SetString(i + "Npc", PlayerDataScript.DefeatNpcs[i]);

        }
        foreach (var npc in Npcs)
        {
            if (npc.story == null)
            {
                npc.RefreshView();
            }
            StorySaveLoadManager.Serialize(npc.story.state.ToJson(), npc.NpcName);
        }
        PlayerPrefs.SetInt("playerIsSaveGame", 1);
        PlayerPrefs.SetInt("playerWorld", 1);

    }

    public void Load()
    {
        Player.position = new Vector2(PlayerPrefs.GetFloat("playerX"), Player.position.y);
        PlayerDataScript.Money = PlayerPrefs.GetInt("playerMoney");
        for (int i = 0; i < 3; i++)
        {
            if (PlayerPrefs.HasKey(i+"Card"))
            {
                PlayerDataScript.PlayerDeck.Add(PlayerPrefs.GetString(i + "Card"));
            }
        }     
        for (int i = 0; i < 3; i++)
        {
            if (PlayerPrefs.HasKey(i+"Npc"))
            {
                PlayerDataScript.DefeatNpcs.Add(PlayerPrefs.GetString(i + "Npc"));
            }
        }



    }
    public void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
    }
}
