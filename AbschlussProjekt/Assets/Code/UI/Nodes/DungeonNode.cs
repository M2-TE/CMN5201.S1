using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class DungeonNode : MonoBehaviour
{
	public enum RoomType { Empty, Camp, StandardCombat, EliteCombat, Boss, Treasure }

	[NonSerialized] public DungeonNode ParentNode;
	[NonSerialized] public List<DungeonNode> ChildNodes;

	public RoomType OwnRoomType;
	[NonSerialized] public int Depth;


	public void SignifyPress()
	{
		AssetManager.Instance.GetManager<DungeonManager>().ReceiveNodePress(this);
	}
}
