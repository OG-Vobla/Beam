using UnityEngine;
using UnityEngine.UI;
using System;
using Ink.Runtime;
using System.Collections.Generic;
using System.Collections;
using TMPro;

// This is a super bare bones example of how to play and display a ink story in Unity.
public class BasicInkExample : MonoBehaviour {
	[SerializeField]
	private TextAsset inkJSONAsset = null;
	public Story story;

	[SerializeField]
	private GameObject canvas = null;


	[SerializeField]
	private GameObject buttons = null;

	[SerializeField]
	private Button buttonPrefab = null;
	private bool textEnd = false;

	private bool isFirstRun = false;
	void Awake () {
		isFirstRun = true;
		RemoveChildren();
		
	}

	void StartStory () {
		story = new Story (inkJSONAsset.text);
		RefreshView();
	}

	void RefreshView () {
		if (story == null)
		{
			StartStory();
			return;
		}
		RemoveChildren();
		
		while (story.canContinue) {
			string text = story.Continue ();
			text = text.Trim();
			StartCoroutine(DialogWriteString(text));
		}

		if(story.currentChoices.Count > 0) {
			for (int i = 0; i < story.currentChoices.Count; i++) {
				Choice choice = story.currentChoices [i];
				Button button = CreateChoiceView (choice.text.Trim ());
				button.onClick.AddListener (delegate {

						OnClickChoiceButton(choice);
					
				});
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
				GameObject.Destroy(buttons.transform.GetChild(i).gameObject);
			}
			
		}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			StartCoroutine(DialogHideShow("Show"));
			if (isFirstRun)
			{

				RefreshView();
				isFirstRun = false;
			}
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
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
		int rnd  = UnityEngine.Random.Range(80,90);
		TMP_Text text = canvas.transform.Find("Panel").gameObject.transform.Find("DialogText").gameObject.GetComponent<TMP_Text>();
		text.text = "";
		for (int i = 0; i< newtext.Length; i++)
		{
			Debug.Log(newtext[i]);
			text.text += newtext[i];
			yield return new WaitForSeconds(0.1f);
			if (i == rnd)
			{
				yield return new WaitForSeconds(2f);
				rnd = UnityEngine.Random.Range(100, 110);
				text.text = "";
			}
		}
		textEnd = true;
	}
}
