using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject PlayBtn;
    [SerializeField] private GameObject MainButtons;
    [SerializeField] private GameObject SettingsButtons;    
    [SerializeField] private GameObject MusicBtnRender;
    [SerializeField] private GameObject SoundBtnRender;    
    [SerializeField] private Sprite OnMusImage;
    [SerializeField] private Sprite OffMusImage;

    private bool settingsIsOpen;
    void Start()
    {
        typeof(PlayerDataScript).TypeInitializer.Invoke(null, null);
        settingsIsOpen = false ;
        if (PlayerPrefs.GetInt("playerIsSaveGame") == 0)
        {
            PlayBtn.GetComponent<UnityEngine.UI.Button>().interactable = false;
            Color color = PlayBtn.GetComponent<UnityEngine.UI.Image>().color;
            color.a = 0.6f;
            PlayBtn.GetComponent<UnityEngine.UI.Image>().color =color;
        }
        CheckMusicSound();
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
    
    public void OpenCloseSettings()
    {
        if (settingsIsOpen)
        {
            SettingsButtons.SetActive(false);
            MainButtons.SetActive(true);
        }
        else{
            MainButtons.SetActive(false);
            SettingsButtons.SetActive(true);
        }
        settingsIsOpen = !settingsIsOpen;
    }

    public void newGame()
    {
        PlayerDataScript.gameIsLoad = false;
        PlayerDataScript.Money= 900;
        PlayerPrefs.DeleteAll();
        loadGameScene();
    }
    public void OnOffMusicSound(bool isMusic)
    {
        if (isMusic)
        {
            PlayerDataScript.musicOn = !PlayerDataScript.musicOn;
        }
        else
        {
            PlayerDataScript.soundsOn = !PlayerDataScript.soundsOn;
        }
        var BgMusInOpenWorld = GameObject.FindGameObjectWithTag("BgMusicOpenWorld");
        if (BgMusInOpenWorld != null)
        {
            BgMusInOpenWorld.GetComponent<AudioSource>().enabled = PlayerDataScript.musicOn;
        }

        var BgMusicPlatformer = GameObject.FindGameObjectWithTag("BgMusicPlatformer");
        if (BgMusicPlatformer != null)
        {
            BgMusicPlatformer.GetComponent<AudioSource>().enabled = PlayerDataScript.musicOn;
        }
        
        CheckMusicSound();
    }
    private  void CheckMusicSound()
    {
        if (PlayerDataScript.musicOn)
        {
            MusicBtnRender.GetComponent<UnityEngine.UI.Image>().sprite = OnMusImage;
        }
        else
        {
            MusicBtnRender.GetComponent<UnityEngine.UI.Image>().sprite = OffMusImage;
        }    
        if (PlayerDataScript.soundsOn)
        {
            SoundBtnRender.GetComponent<UnityEngine.UI.Image>().sprite = OnMusImage;
        }
        else
        {
            SoundBtnRender.GetComponent<UnityEngine.UI.Image>().sprite = OffMusImage;
        }
    }
    private void loadGameScene()
    {
        SceneManager.LoadScene("OpenWorld");
    }
}
