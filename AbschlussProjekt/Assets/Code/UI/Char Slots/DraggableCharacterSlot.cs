using System;

public class DraggableCharacterSlot : CharacterSlot
{
	[NonSerialized] public StoredCharacterSlot correspondingStoredCharSlot;
}
