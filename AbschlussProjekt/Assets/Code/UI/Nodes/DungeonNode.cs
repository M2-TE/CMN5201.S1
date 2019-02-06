using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class DungeonNode : MonoBehaviour
{
	public enum RoomType { Empty, Camp, StandardCombat, EliteCombat, Boss, Treasure }

	[NonSerialized] public DungeonNode ParentNode;
	[NonSerialized] public List<DungeonNode> ChildNodes;
	[NonSerialized] public RoomType OwnRoomType;
	[NonSerialized] public int Depth;

	private LineRenderer ownLineRenderer;
	public LineRenderer OwnLineRenderer
	{
		get { return ownLineRenderer ?? (ownLineRenderer = GetComponent<LineRenderer>()); }
	}
	
	public void SignifyPress()
	{
		AssetManager.Instance.GetManager<DungeonManager>().ExtendNodePress(this);
	}
}
