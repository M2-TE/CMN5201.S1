using System;
using System.Collections.Generic;
using Utilities;

[Serializable]
public class Savestate
{
	public RuntimeSettings RuntimeSettings;

	public List<Entity> OwnedCharacters;
	public Entity[] CurrentTeam;
    public List<StorageSlot> Inventory;
	public int Gold;
	public int Souls;


	private string m_currentLocation;
	[NonSerialized] private AreaData currentLocation;
	public AreaData CurrentLocation
	{
		get { return currentLocation ?? (currentLocation = AssetManager.Instance.LoadArea(m_currentLocation)); }
		set { currentLocation = value; m_currentLocation = value.name; }
	}

	public Savestate()
	{
		RuntimeSettings = new RuntimeSettings();

		OwnedCharacters = new List<Entity>();
        Inventory = new List<StorageSlot>();
		CurrentTeam = new Entity[4];
		m_currentLocation = AssetManager.Instance.Paths.DefaultLocation;
	}
}