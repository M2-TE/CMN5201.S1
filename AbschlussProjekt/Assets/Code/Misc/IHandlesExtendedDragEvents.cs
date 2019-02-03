public interface IHandlesCharacterSlots
{
	void OnStartDrag(StoredCharacterSlot slot);

	void DuringDrag(StoredCharacterSlot slot);

	void OnEndDrag(StoredCharacterSlot slot);

	void OnMouseEnter(StoredCharacterSlot slot);

	void OnMouseExit(StoredCharacterSlot slot);

	void SwapSlots(StoredCharacterSlot slot1, StoredCharacterSlot slot2);
}
