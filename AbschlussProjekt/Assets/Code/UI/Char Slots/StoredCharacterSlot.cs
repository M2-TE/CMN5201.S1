using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoredCharacterSlot : CharacterSlot, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
	public enum SlotType { OwnedCharacter, EquippedCharacter }
	[NonSerialized] public Entity StoredEntity;
	[NonSerialized] public SlotType OwnSlotType;
	[NonSerialized] public int ArrayPos;

	private IHandlesCharacterSlots manager;

	public void RegisterManager(IHandlesCharacterSlots manager)
	{
		this.manager = manager;
	}

	public override string ToString()
	{
		return StoredEntity.Name + " " + OwnSlotType + " " + ArrayPos;
	}

	#region Events
	public void OnBeginDrag(PointerEventData eventData)
	{
		manager.OnStartDrag(this);
	}

	public void OnDrag(PointerEventData eventData)
	{
		manager.DuringDrag(this);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		manager.OnEndDrag(this);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		manager.OnMouseEnter(this);
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		manager.OnMouseExit(this);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		switch (eventData.pointerId)
		{
			case -1:
				Debug.Log("Left Clicked.");
				return;

			case -2:
                Debug.Log(AssetManager.Instance.GetManager<CharacterInfoManager>() == null ? "YES" : "NO");
				AssetManager.Instance.GetManager<CharacterInfoManager>().CharacterInfoPanel.OpenCharacterInfo(StoredEntity);
				return;

			default: return;
		}
	}
	#endregion
}
