using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
	[SerializeField] private Transform inventoryCan;
	[SerializeField] private List<Transform> inventorySlots;
	private bool inventoryIsOpen;


	void Start()
    {
		inventoryIsOpen = false;

	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.I))
		{
			inventoryIsOpen = !inventoryIsOpen;
			inventoryCan.gameObject.SetActive(inventoryIsOpen);
		}
	}
	public void AddItemInInventory(GameObject card)
	{
		GameObject newCard = Instantiate(card, inventorySlots[PlayerDataScript.PlayerDeck.Count]);
		RectTransform rect = newCard.GetComponent<RectTransform>();	
		rect.localScale = new Vector3(131.17f, 178.1554f, 0);
		rect.position = new Vector3(0,0,0);
	}
}
