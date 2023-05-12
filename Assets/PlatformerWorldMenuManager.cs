using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformerWorldMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject Menu;

    private void Awake()
    {
        Destroy(GameObject.FindGameObjectWithTag("BgMusic"));
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
        PlayerPrefs.SetInt("playerIsSaveGame", 1);
        PlayerPrefs.SetInt("playerWorld", 2);

    }

    public void Load()
    {

    }
    public void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
    }
}
