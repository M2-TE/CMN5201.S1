using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoredCharacterSlot : CharacterSlot, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[NonSerialized] public Entity StoredEntity;

	private BarracksManager barracksManager;

	public void RegisterManager(BarracksManager barracksManager)
	{
		this.barracksManager = barracksManager;
	}

	private void HandleRightClick()
	{

	}

	#region Events
	public void OnBeginDrag(PointerEventData eventData)
	{
		barracksManager.StartDrag(this);
	}

	public void OnDrag(PointerEventData eventData)
	{
		barracksManager.DuringDrag();
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		barracksManager.EndDrag();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		switch (eventData.pointerId)
		{
			case -1:
				Debug.Log("Left Clicked.");
				return;

			case -2:
				Debug.Log("Right Clicked");
				HandleRightClick();
				return;

			default: return;
		}
	}
	#endregion
}
