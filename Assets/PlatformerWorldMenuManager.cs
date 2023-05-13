using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformerWorldMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject Menu;

    [SerializeField] private GameObject MainButtons;
    [SerializeField] private GameObject SettingsButtons;
    [SerializeField] private GameObject MusicBtnRender;
    [SerializeField] private GameObject SoundBtnRender;
    [SerializeField] private Sprite OnMusImage;
    private bool settingsIsOpen;
    [SerializeField] private Sprite OffMusImage;
    private void Awake()
    {
        Destroy(GameObject.FindGameObjectWithTag("BgMusicOpenWorld"));
        if (PlayerPrefs.GetInt("playerIsSaveGame") == 1)
        {
            Load();
        }
        CheckMusicSound();
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
    public void OpenCloseSettings()
    {
        if (settingsIsOpen)
        {
            SettingsButtons.SetActive(false);
            MainButtons.SetActive(true);
        }
        else
        {
            MainButtons.SetActive(false);
            SettingsButtons.SetActive(true);
        }
        settingsIsOpen = !settingsIsOpen;
    }
    private void CheckMusicSound()
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
