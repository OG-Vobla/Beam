using UnityEngine;
using UnityEngine.UI;
using System;
using Ink.Runtime;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.SearchService;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEditor.Profiling.Memory.Experimental;
using System.Linq;
using System.ComponentModel;

// This is a super bare bones example of how to play and display a ink story in Unity.
public class BasicInkExample : MonoBehaviour {

	[SerializeField]
	private TextAsset inkJSONAsset = null;
	public Story story;

	[SerializeField]
	private GameObject canvas = null;

	[SerializeField] public string NpcName;

	[SerializeField]
	private GameObject buttons = null;
    [SerializeField]
    private List<string> enemyDeck;

    [SerializeField]
	private Button buttonPrefab = null;
	private bool textEnd = true;

	private bool isGive;

	void Awake () {

		RemoveChildren();
	}
	public void LoadStory()
    {
        story.state.LoadJson(StorySaveLoadManager.Deserialize(NpcName));
    }
    void StartStory () {
		story = new Story (inkJSONAsset.text);
		if (PlayerPrefs.GetInt("playerIsSaveGame") == 1)
        {
            LoadStory();
        }
        RefreshView();
	}
	public void PlayWithNpc()
    {
		if (textEnd)
		{
			textEnd = true;
			if (PlayerDataScript.PlayerDeck.Count == 3)
			{
				if ((PlayerDataScript.DefeatNpcs.Where(element => element == NpcName).Count() == 0 || PlayerDataScript.DefeatNpcs.Count == 0))
				{
					StartCoroutine(DialogWriteString("Давай."));
					GameObject.Find("MenuManager").GetComponent<OpenWorldMenuManager>().SaveGame();
					PlayerDataScript.EnemyDeck = enemyDeck;
					PlayerDataScript.FightNpcs = NpcName;
					StartCoroutine(LoadSceneWithDelay("FightScene", 1));
				}
				else
				{
					StartCoroutine(DialogWriteString("Мы уже играли."));
				}
			}
			else
			{
				StartCoroutine(DialogWriteString("У тебя не хватает карт."));
			}
		}
    }
	public IEnumerator LoadSceneWithDelay(string name, float time)
	{
		yield return new WaitForSeconds(time);
        SceneManager.LoadScene(name);
    }
	public void CheckWins()
    {
		if (textEnd)
		{
			textEnd = true; 
			if (PlayerDataScript.DefeatNpcs.Count == 3)
            {
                StartCoroutine(DialogWriteString("Молодец, удачи в другом измерении. Пока."));
                GameObject.Find("MenuManager").GetComponent<OpenWorldMenuManager>().SaveGame();
                StartCoroutine(LoadSceneWithDelay("PlatformerScene", 4));
            }
            else
            {
                StartCoroutine(DialogWriteString("Не ври мне, я все знаю."));
            }
        }
           
	}
    public void RefreshView () {
		if (story == null)
		{
			StartStory();
			return;
		}
		RemoveChildren();
		
		while (story.canContinue) {
			isGive = false;
            string text = story.Continue();
            text = story.currentText.Trim();
		}
        StartCoroutine(DialogWriteString(story.currentText.Trim()));


		if (story.currentChoices.Count > 0) {
			for (int i = 0; i < story.currentChoices.Count; i++) {
				Choice choice = story.currentChoices [i];
				Button button = CreateChoiceView (choice.text.Trim ());
				button.onClick.AddListener (delegate {
						OnClickChoiceButton(choice);
				});
			}
		}
        HandleTags(story.currentTags);
    }
    private void HandleTags(List<string> currentTags)
	{
		foreach (string tag in currentTags)
		{
			string[] splitTag = tag.Split(':');
			if (splitTag.Length != 2)
			{
				Debug.Log("ErrorTag," + tag);
			}
			Debug.Log(splitTag[0].Trim() + splitTag[1].Trim() + tag);
			string tagKey = splitTag[0].Trim();
			string tagValue = splitTag[1].Trim();
			switch(tagKey)
			{
				case "giveMoney":
					if (isGive == false)
					{
						isGive = true;
						PlayerDataScript.Money = PlayerDataScript.Money + Convert.ToInt32(tagValue);
					}
                    break;
			}
		}
	}
	void OnClickChoiceButton (Choice choice) {
		if (textEnd)
		{
			textEnd = true;
			story.ChooseChoiceIndex(choice.index);
			RefreshView();
		}
	}


	Button CreateChoiceView (string text) {
		Button choice = Instantiate (buttonPrefab, buttons.transform) as Button;

		TMP_Text choiceText = choice.GetComponentInChildren<TMP_Text>();
		choiceText.text = text;

		return choice;
	}

	void RemoveChildren () {
		TMP_Text text = canvas.transform.Find("Panel").gameObject.transform.Find("DialogText").gameObject.GetComponent<TMP_Text>();
		text.text = "";
		int childCount = buttons.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i) {
			if (buttons.transform.GetChild(i).gameObject.GetComponent<Button>() != null)
			{
				if (buttons.transform.GetChild(i).gameObject.tag != "DontRemove")
                {
                    GameObject.Destroy(buttons.transform.GetChild(i).gameObject);
                }
			}
		}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
            StartCoroutine(DialogHideShow("Show"));
				RefreshView();
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
            StopAllCoroutines();
            StartCoroutine(DialogHideShow("Hide"));
        }
	}
	private IEnumerator DialogHideShow(string str)
	{
		if (str == "Hide")
		{
			for (float i = 1; i > 0; i -= 0.01f)
			{
				canvas.GetComponent<CanvasGroup>().alpha = i;
				yield return new WaitForSeconds(0.001f);
			}
			canvas.gameObject.SetActive(false);

		}
		else if (str == "Show")
		{
			canvas.gameObject.SetActive(true);
			for (float i = 0; i < 1; i += 0.01f)
			{
				canvas.GetComponent<CanvasGroup>().alpha = i;
				yield return new WaitForSeconds(0.001f);
			}
		}
	}
	private IEnumerator DialogWriteString(string newtext)
	{
            textEnd = false;
            int rnd = UnityEngine.Random.Range(80, 90);
            TMP_Text text = canvas.transform.Find("Panel").gameObject.transform.Find("DialogText").gameObject.GetComponent<TMP_Text>();
            text.text = "";
            for (int i = 0; i < newtext.Length; i++)
            {
			GetComponent<AudioSource>().Play();
                text.text += newtext[i];
                yield return new WaitForSeconds(0.1f);
                if (i == rnd)
                {
                    yield return new WaitForSeconds(2f);
                    rnd = rnd + UnityEngine.Random.Range(80, 90);
                    text.text = "";
                }
            }
		textEnd = true;
	}
}
