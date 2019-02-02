using System.Collections.Generic;
using UnityEngine;

public class BarracksManager : Manager
{
	private BarracksPanel panel;
	private List<GameObject> storedCharacters;

	private GameObject currentlyDraggedObject;

	public BarracksManager() { storedCharacters = new List<GameObject>(); }

	public void RegisterPanel(BarracksPanel barracksPanel)
	{
		panel = barracksPanel;

		SetupStoredCharScrollView();
	}

	private void SetupStoredCharScrollView()
	{
		List<Entity> storedEntities = AssetManager.Instance.Savestate.OwnedCharacters;
		storedCharacters.Clear();

		var rectTrans = panel.CharStorageNode.GetComponent<RectTransform>();
		rectTrans.sizeDelta = new Vector2(0f, storedEntities.Count *- panel.CharStoragePerSlotOffset.y);

		for (int i = 0; i < storedEntities.Count; i++)
		{
			var go = Object.Instantiate(panel.StorageSlotPrefab, panel.CharStorageNode);
			go.GetComponent<RectTransform>().localPosition = panel.CharStorageStartPos + panel.CharStoragePerSlotOffset * i;
			storedCharacters.Add(go);

			var storedChar = go.GetComponent<StoredCharacterSlot>();
			storedChar.PortraitImage.sprite = storedEntities[i].CharDataContainer.Portrait;
			storedChar.RegisterManager(this);
		}
	}

	public void StartDrag(StoredCharacterSlot storedCharacter)
	{
		currentlyDraggedObject = Object.Instantiate(panel.DraggableSlotPrefab, panel.transform);
		currentlyDraggedObject.GetComponent<DraggableCharacterSlot>().PortraitImage.sprite = storedCharacter.PortraitImage.sprite;
	}

	public void DuringDrag()
	{
		currentlyDraggedObject.transform.position = Input.mousePosition;
	}

	public void EndDrag()
	{
		Object.Destroy(currentlyDraggedObject);
	}
}