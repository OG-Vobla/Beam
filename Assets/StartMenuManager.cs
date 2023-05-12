using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject PlayBtn;
    void Start()
    {
        if (PlayerPrefs.GetInt("playerIsSaveGame") == 0)
        {
            PlayBtn.GetComponent<Button>().interactable = false;
            Color color = PlayBtn.GetComponent<Image>().color;
            color.a = 0.6f;
            PlayBtn.GetComponent<Image>().color =color;
        }
    }
    public void continueGame()
    {
        PlayerDataScript.gameIsLoad = true;
        if (PlayerPrefs.GetInt("playerWorld") == 1)
        {
            SceneManager.LoadScene("OpenWorld");
        }
        else if (PlayerPrefs.GetInt("playerWorld") == 2)
        {
            SceneManager.LoadScene("PlatformerScene");
        }
    }

    public void GoMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
    public void newGame()
    {
        PlayerDataScript.gameIsLoad = false;
        PlayerDataScript.Money= 100;
        PlayerPrefs.DeleteAll();
        loadGameScene();
    }
    private void loadGameScene()
    {
        SceneManager.LoadScene("OpenWorld");
    }
}
