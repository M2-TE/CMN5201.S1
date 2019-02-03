using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class GeneralCityManager : Manager, IHandlesCharacterSlots
{
	private GeneralCityPanel panel;
	private CharacterSlotDragDropManager dragDropManager;
	private GameObject currentlyDraggedObject;

	public void Register(GeneralCityPanel panel) { this.panel = panel; }

	public void Initialize()
	{
		dragDropManager = AssetManager.Instance.GetManager<CharacterSlotDragDropManager>() ?? new CharacterSlotDragDropManager();
		SetupEquippedCharacterPanels();
	}

	private void SetupEquippedCharacterPanels()
	{
		Entity[] equippedEntities = AssetManager.Instance.Savestate.CurrentTeam;

		for(int i = 0; i < equippedEntities.Length; i++)
		{
			if(equippedEntities[i] != null)
			{
				StoredCharacterSlot tempSlot = panel.EquippedCharSlots[i];
				tempSlot.enabled = true;
				tempSlot.PortraitImage.color = new Color(1f, 1f, 1f, 1f);
				tempSlot.PortraitImage.sprite = equippedEntities[i].CharDataContainer.Portrait;
				tempSlot.StoredEntity = equippedEntities[i];
				tempSlot.ArrayPos = i;
				tempSlot.OwnSlotType = StoredCharacterSlot.SlotType.EquippedCharacter;
				tempSlot.RegisterManager(this);
			}
			else
			{
				panel.EquippedCharSlots[i].enabled = false;
				panel.EquippedCharSlots[i].PortraitImage.color = new Color(1f, 1f, 1f, .2f);
				panel.EquippedCharSlots[i].PortraitImage.sprite = null;
			}
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
		if (dragDropManager.CurrentMousedOverSlot != null &&
			dragDropManager.CurrentDraggedSlot.StoredEntity != dragDropManager.CurrentMousedOverSlot.StoredEntity)
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
		
		// slot 1 has to be an equipped character slot
		if (slot2.OwnSlotType == StoredCharacterSlot.SlotType.EquippedCharacter)
		{
			instance.Savestate.CurrentTeam[slot1.ArrayPos] = slot2.StoredEntity;
			instance.Savestate.CurrentTeam[slot2.ArrayPos] = slot1.StoredEntity;
		}
		else
		{
			instance.Savestate.CurrentTeam[slot1.ArrayPos] = slot2.StoredEntity;
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
