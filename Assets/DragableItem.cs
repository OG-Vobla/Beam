using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragableItem : MonoBehaviour,IBeginDragHandler, IEndDragHandler, IDragHandler
{
	[SerializeField] private bool isDragable;
	private Transform parentAfterDrag;

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (transform.gameObject.tag == "Slot")
		{

		}
		parentAfterDrag = transform.parent;
		transform.SetParent(transform.root);
		transform.SetAsLastSibling();
	}

	public void OnDrag(PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		transform.SetParent(parentAfterDrag);
		transform.position = new Vector3(0, 0, 0);
	}
}
