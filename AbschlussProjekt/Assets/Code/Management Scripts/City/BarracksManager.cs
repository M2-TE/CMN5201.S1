using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class BarracksManager : Manager, IHandlesCharacterSlots
{
	private GameObject currentlyDraggedObject;

	private BarracksPanel panel;
	private CharacterSlotDragDropManager dragDropManager;
	private List<GameObject> storedCharacters;

	public BarracksManager() { storedCharacters = new List<GameObject>(); }

	public void RegisterPanel(BarracksPanel barracksPanel) { panel = barracksPanel; }

	public void Initialize()
	{
		dragDropManager = AssetManager.Instance.GetManager<CharacterSlotDragDropManager>() ?? new CharacterSlotDragDropManager();
		SetupStoredCharScrollView();
	}

	private void SetupStoredCharScrollView()
	{
		List<Entity> ownedCharacters = AssetManager.Instance.Savestate.OwnedCharacters;
		storedCharacters.Clear();

		var rectTrans = panel.CharStorageNode.GetComponent<RectTransform>();
		rectTrans.sizeDelta = new Vector2(0f, ownedCharacters.Count *- panel.CharStoragePerSlotOffset.y);

		for (int i = 0; i < ownedCharacters.Count; i++)
		{
			var go = Object.Instantiate(panel.StorageSlotPrefab, panel.CharStorageNode);
			go.GetComponent<RectTransform>().localPosition = panel.CharStorageStartPos + panel.CharStoragePerSlotOffset * i;
			storedCharacters.Add(go);

			var storedChar = go.GetComponent<StoredCharacterSlot>();
			storedChar.PortraitImage.sprite = ownedCharacters[i].CharDataContainer.Portrait;
			storedChar.StoredEntity = ownedCharacters[i];
			storedChar.ArrayPos = i;
			storedChar.OwnSlotType = StoredCharacterSlot.SlotType.OwnedCharacter;
			storedChar.RegisterManager(this);
		}
	}

	public void OnStartDrag(StoredCharacterSlot slot)
	{
		dragDropManager.CurrentDraggedSlot = slot;
		// this is the actual character slot /\
		// while this is just a proxy for dragging \/
		currentlyDraggedObject = Object.Instantiate(panel.DraggableSlotPrefab, panel.transform.parent); 
		currentlyDraggedObject.GetComponent<DraggableCharacterSlot>().PortraitImage.sprite = slot.PortraitImage.sprite;
	}

	public void DuringDrag(StoredCharacterSlot draggedSlot)
	{
		currentlyDraggedObject.transform.position = Input.mousePosition;
	}

	public void OnEndDrag(StoredCharacterSlot slot)
	{
		if (dragDropManager.CurrentMousedOverSlot != null
			&& dragDropManager.CurrentDraggedSlot.StoredEntity != dragDropManager.CurrentMousedOverSlot.StoredEntity)
				SwapSlots(dragDropManager.CurrentDraggedSlot, dragDropManager.CurrentMousedOverSlot);

		dragDropManager.CurrentDraggedSlot = null;
		Object.Destroy(currentlyDraggedObject);
	}

	public void OnMouseEnter(StoredCharacterSlot slot)
	{
		dragDropManager.CurrentMousedOverSlot = slot;
	}

	public void OnMouseExit(StoredCharacterSlot slot)
	{
		dragDropManager.CurrentMousedOverSlot = null;
	}

	public void SwapSlots(StoredCharacterSlot slot1, StoredCharacterSlot slot2)
	{
		AssetManager instance = AssetManager.Instance;

		// slot 1 has to be an owned character slot
		if (slot2.OwnSlotType == StoredCharacterSlot.SlotType.EquippedCharacter)
		{
			instance.Savestate.CurrentTeam[slot2.ArrayPos] = slot1.StoredEntity;
			instance.Savestate.OwnedCharacters[slot1.ArrayPos] = slot2.StoredEntity;
		}
		else
		{
			instance.Savestate.OwnedCharacters[slot1.ArrayPos] = slot2.StoredEntity;
			instance.Savestate.OwnedCharacters[slot2.ArrayPos] = slot1.StoredEntity;
		}

		Entity bufferEntity = slot1.StoredEntity;
		slot1.StoredEntity = slot2.StoredEntity;
		slot2.StoredEntity = bufferEntity;

		Sprite bufferImage = slot1.PortraitImage.sprite;
		slot1.PortraitImage.sprite = slot2.PortraitImage.sprite;
		slot2.PortraitImage.sprite = bufferImage;
	}
}