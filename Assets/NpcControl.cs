using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcControl : MonoBehaviour
{
	[SerializeField] string[] DeffaultDialogText;
	[SerializeField] string[] BeforeFightDialogText;
	[SerializeField] string[] LoseDialogText;
	[SerializeField] string[] WinDialogText;
	[SerializeField] string DialogState;
	private GameObject DialogCanvas;
    // Start is called before the first frame update
    void Start()
    {
		DialogCanvas = transform.Find("DialogCanvas").gameObject;

	}

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void DialogAgree()
	{
		
	}

    private IEnumerator DialogHideShow(string str)
    {
        if (str == "Hide")
        {
            for (float i = 1; i > 0; i-= 0.01f)
            {
				DialogCanvas.GetComponent<CanvasGroup>().alpha= i;
				yield return new WaitForSeconds(0.001f);
			}
			DialogCanvas.SetActive(false);

		}
        else if(str == "Show")
		{
			DialogCanvas.SetActive(true);
			for (float i = 0; i < 1; i += 0.01f)
			{
				DialogCanvas.GetComponent<CanvasGroup>().alpha = i;
				yield return new WaitForSeconds(0.001f);
			}
		}
    }
	private IEnumerator DialogWriteString(string[] str)
	{
		foreach (var i in str)
		{
			string newStr = i;
			DialogCanvas.transform.Find("Panel").gameObject.transform.Find("DialogText").gameObject.GetComponent<TMP_Text>().text = "";
			while (newStr.Length > 0)
			{
				DialogCanvas.transform.Find("Panel").gameObject.transform.Find("DialogText").gameObject.GetComponent<TMP_Text>().text = DialogCanvas.transform.Find("Panel").gameObject.transform.Find("DialogText").gameObject.GetComponent<TMP_Text>().text + newStr[0];
				newStr = newStr.Remove(0, 1);
				yield return new WaitForSeconds(0.1f);
			}
			yield return new WaitForSeconds(2f);
		}

	}
	private void DialogWrite()
	{
		if (DialogState == "Deffault")
		{
			StartCoroutine(DialogWriteString(DeffaultDialogText));
		}
		else if (DialogState == "BeforeFight")
		{
			StartCoroutine(DialogWriteString(BeforeFightDialogText));
		}
		else if (DialogState == "LoseFight")
		{
			StartCoroutine(DialogWriteString(LoseDialogText));
		}
		else if (DialogState == "WinFight")
		{
			StartCoroutine(DialogWriteString(WinDialogText));
		}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(DialogHideShow("Show"));
			DialogWrite();
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{ 
		if (collision.gameObject.tag == "Player")
		{
			StartCoroutine(DialogHideShow("Hide"));
		}
	}
}
