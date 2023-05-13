using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NpcControl : MonoBehaviour
{
	[SerializeField] string[] DeffaultDialogText;
	[SerializeField] string DialogState;
	private GameObject DialogCanvas;
    private AudioSource typeAudio;

    // Start is called before the first frame update
    void Start()
    {
		DialogCanvas = transform.Find("DialogCanvas").gameObject;
        typeAudio = gameObject.GetComponent<AudioSource>();

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
			TMP_Text tM =  DialogCanvas.transform.Find("Panel").gameObject.transform.Find("DialogText").gameObject.GetComponent<TMP_Text>();
			tM.text = "";
			while (newStr.Length > 0)
            {
                if (PlayerDataScript.soundsOn)
                {
                    typeAudio.Play();
                }

                tM.text = tM.text + newStr[0];
				newStr = newStr.Remove(0, 1);
				yield return new WaitForSeconds(0.1f);
			}
			yield return new WaitForSeconds(2f);
		}

	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.tag == "Player")
        {
			
            StartCoroutine(DialogHideShow("Show"));
            StartCoroutine(DialogWriteString(DeffaultDialogText));
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
}
