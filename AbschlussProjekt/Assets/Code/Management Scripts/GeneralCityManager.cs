using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

public class GeneralCityManager : Manager
{
	public EquippedCharacterPanel charPanel;
	public BarracksPanel barracksPanel;

	private CharacterSlot currentlyDraggedCharacter;
	private CharacterSlot currentlyMousedOverCharacter;
	private GameObject currentlyDraggedProxySlot;

	internal void SetupEquippedCharPanel(EquippedCharacterPanel charPanel)
	{
		this.charPanel = charPanel;
		var savestate = AssetManager.Instance.Savestate;
		for (int i = 0; i < charPanel.equippedCharSlots.Length; i++)
		{
			charPanel.equippedCharSlots[i].StoredEntity = savestate.CurrentTeam[i];
			charPanel.equippedCharSlots[i].OwnSlotType = CharacterSlot.SlotType.EquippedCharacter;
			charPanel.equippedCharSlots[i].SlotPos = i;
		}
	}

	internal void SetupBarracksPanel(BarracksPanel barracksPanel)
	{
		this.barracksPanel = barracksPanel;
		barracksPanel.barrackSlots = new List<CharacterSlot>();
		var savestate = AssetManager.Instance.Savestate;

		for(int i = 0; i < savestate.OwnedCharacters.Count; i++)
			AddSlotToBarracks(savestate.OwnedCharacters[i]);
	}

	private void AddSlotToBarracks(Entity slotEntity)
	{
		GameObject go;
		go = Object.Instantiate(barracksPanel.characterSlotPrefab, barracksPanel.CharacterStorageNode);
		go.GetComponent<RectTransform>().anchoredPosition = barracksPanel.StartingSlotOffset + barracksPanel.SlotOffset * (barracksPanel.barrackSlots.Count);

		CharacterSlot slot;
		slot = go.GetComponent<CharacterSlot>();
		slot.StoredEntity = slotEntity;
		slot.OwnSlotType = CharacterSlot.SlotType.OwnedCharacter;
		slot.SlotPos = barracksPanel.CharacterStorageNode.childCount - 1;
		barracksPanel.barrackSlots.Add(slot);

		barracksPanel.CharacterStorageNode.GetComponent<RectTransform>().sizeDelta 
			= new Vector2(0f, barracksPanel.barrackSlots.Count * 102.5f);
	}

	private void RemoveBarrackSlot(CharacterSlot slotToRemove)
	{
		int slotIndex = slotToRemove.SlotPos;
		
		for(int i = slotIndex; i < barracksPanel.barrackSlots.Count - 1; i++)
			barracksPanel.barrackSlots[i].StoredEntity = barracksPanel.barrackSlots[i + 1].StoredEntity;

		slotToRemove = barracksPanel.barrackSlots[barracksPanel.barrackSlots.Count - 1];
		barracksPanel.barrackSlots.Remove(slotToRemove);
		Object.Destroy(slotToRemove.gameObject);
		
		barracksPanel.CharacterStorageNode.GetComponent<RectTransform>().sizeDelta
			= new Vector2(0f, barracksPanel.barrackSlots.Count * 102.5f);
	}

	private bool SetEquippedSlot(Entity slotEntity, int slotPos)
	{
		if(slotEntity == null)
		{
			int equippedChars = 0;
			for (int i = 0; i < charPanel.equippedCharSlots.Length; i++)
				if (charPanel.equippedCharSlots[i].StoredEntity != null) equippedChars++;
			if (equippedChars > 1)
			{
				// simply set slot to null
				charPanel.equippedCharSlots[slotPos].StoredEntity = slotEntity;
				SortEquippedSlots();
				return true; // return true if operation could be fulfilled
			}
			else return false; // return false if there was only one entity remaining on the team
		}
		charPanel.equippedCharSlots[slotPos].StoredEntity = slotEntity;
		SortEquippedSlots();
		return true; 
	}

	private void SortEquippedSlots()
	{
		for(int i = 0; i < charPanel.equippedCharSlots.Length; i++)
		{
			// check from first to last slot if entities need to be moved up
			if (charPanel.equippedCharSlots[i].StoredEntity == null)
			{
				// then find next filled slot
				for(int x = i; x < charPanel.equippedCharSlots.Length; x++)
				{
					// when the next entity has been found, swap
					if (charPanel.equippedCharSlots[x].StoredEntity != null)
					{
						charPanel.equippedCharSlots[i].StoredEntity = charPanel.equippedCharSlots[x].StoredEntity;
						charPanel.equippedCharSlots[x].StoredEntity = null;
						break;
					}
					else continue;
				}
			}
			else continue;
		}
	}

	private void SetUnkownSlotType(CharacterSlot slot, Entity entity)
	{
		switch (slot.OwnSlotType)
		{
			case CharacterSlot.SlotType.EquippedCharacter:
				SetEquippedSlot(entity, slot.SlotPos);
				break;

			case CharacterSlot.SlotType.OwnedCharacter:
				slot.StoredEntity = entity;
				break;

			default: return;
		}
	}

	private void MoveToNextFreeEquippedCharSlot(CharacterSlot fromSlot)
	{
		Entity buffer = fromSlot.StoredEntity;
		if (SetEquippedSlot(null, fromSlot.SlotPos))
			AddSlotToBarracks(buffer);
	}

	private void MoveToNextFreeBarrackCharSlots(CharacterSlot fromSlot)
	{
		for (int i = 0; i < charPanel.equippedCharSlots.Length; i++)
		{
			if (charPanel.equippedCharSlots[i].StoredEntity == null)
			{
				charPanel.equippedCharSlots[i].StoredEntity = fromSlot.StoredEntity;
				RemoveBarrackSlot(fromSlot);
				return;
			}
		}
	}

	private void Swap(CharacterSlot slotOne, CharacterSlot slotTwo)
	{
		if(slotTwo == null)
		{
			switch (slotOne.OwnSlotType)
			{
				case CharacterSlot.SlotType.EquippedCharacter:
					MoveToNextFreeEquippedCharSlot(slotOne);
					return;

				case CharacterSlot.SlotType.OwnedCharacter:
					MoveToNextFreeBarrackCharSlots(slotOne);
					return;

				default: return;
			}
		}
		else if(slotTwo.StoredEntity == null)
		{
			switch (slotOne.OwnSlotType)
			{
				case CharacterSlot.SlotType.EquippedCharacter:
					SetEquippedSlot(slotOne.StoredEntity, slotTwo.SlotPos);
					SetEquippedSlot(null, slotOne.SlotPos);
					return;

				case CharacterSlot.SlotType.OwnedCharacter:
					// move entity from barrack slot to an empty equipment slot, thus removing the obsolete barrack slot
					SetEquippedSlot(slotOne.StoredEntity, slotTwo.SlotPos);
					RemoveBarrackSlot(slotOne);
					return;

				default: return;
			}
		}
		else
		{
			// simple swap
			Entity buffer = slotOne.StoredEntity;
			SetUnkownSlotType(slotOne, slotTwo.StoredEntity);
			SetUnkownSlotType(slotTwo, buffer);
		}
	}

	#region Event Calls
	internal void OnStartDrag(CharacterSlot storedCharacterSlot)
	{
		currentlyDraggedCharacter = storedCharacterSlot;
		currentlyDraggedProxySlot = Object.Instantiate(barracksPanel.draggableSlotPrefab, barracksPanel.transform.parent);
		currentlyDraggedProxySlot.GetComponent<CharacterSlot>().StoredEntity = storedCharacterSlot.StoredEntity;
	}

	internal void DuringDrag(CharacterSlot storedCharacterSlot)
	{
		currentlyDraggedProxySlot.transform.position = Input.mousePosition;
	}

	internal void OnEndDrag(CharacterSlot storedCharacterSlot)
	{
		if(storedCharacterSlot != currentlyMousedOverCharacter)
			Swap(currentlyDraggedCharacter, currentlyMousedOverCharacter);

		currentlyDraggedCharacter = null;
		Object.Destroy(currentlyDraggedProxySlot);

	}

	internal void OnMouseEnter(CharacterSlot storedCharacterSlot)
	{
		currentlyMousedOverCharacter = storedCharacterSlot;
	}

	internal void OnMouseExit(CharacterSlot storedCharacterSlot)
	{
		currentlyMousedOverCharacter = null;
	}

	internal void OnPointerClick(CharacterSlot characterSlot, PointerEventData eventData)
	{
		if (currentlyDraggedProxySlot != null) return;

		switch (eventData.pointerId)
		{
			case -1:
				switch (characterSlot.OwnSlotType)
				{
					case CharacterSlot.SlotType.EquippedCharacter:
						MoveToNextFreeEquippedCharSlot(characterSlot);
						return;

					case CharacterSlot.SlotType.OwnedCharacter:
						MoveToNextFreeBarrackCharSlots(characterSlot);
						return;

					default: return;
				}

			case -2:
				AssetManager.Instance.GetManager<CharacterInfoManager>().CharacterInfoPanel.OpenCharacterInfo(characterSlot.StoredEntity);
				return;

			default: return;
		}
	}
	#endregion
}
