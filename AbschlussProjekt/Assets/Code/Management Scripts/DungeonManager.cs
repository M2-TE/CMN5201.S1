using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonManager : Manager
{
	private DungeonPanel dungeonPanel;
	private Dungeon dungeon;

	public void CreateNewDungeon(DungeonPanel dungeonPanel)
	{
		this.dungeonPanel = dungeonPanel;
		dungeon = new Dungeon();

		Image image = dungeonPanel.MapParent.GetComponent<Image>();
		Rect fullMapRect = image.rectTransform.rect;
	}
}

public class Dungeon
{
	public enum RoomType { Empty, Camp, Treasure, StandardCombat, EliteCombat, Boss }

	private DungeonManager dungeonManager;

	private DungeonNode firstNode;
	private DungeonNode lastNode;
	private List<DungeonNode> allNodes;

	public Dungeon()
	{
		dungeonManager = AssetManager.Instance.GetManager<DungeonManager>();
		allNodes = new List<DungeonNode>();

		DungeonNode node = new CombatNode(RoomType.Boss, 10, new Entity[4]);
	}

	private abstract class DungeonNode
	{
		public DungeonNode ParentNode;
		public List<DungeonNode> ChildNodes;

		public RoomType RoomType;
		public int Depth;

		public DungeonNode(RoomType roomType, int depth)
		{
			RoomType = roomType;
			Depth = depth;
		}

		public virtual void SetFamily(DungeonNode parent, List<DungeonNode> children)
		{
			SetParent(parent);

			ChildNodes = children;
			for (int i = 0; i < ChildNodes.Count; i++)
				SetChild(i, children[i]);
		}

		public virtual void SetParent(DungeonNode parent)
		{

		}

		public virtual void SetChild(int pos, DungeonNode child)
		{

		}
	}

	private class StandardNode : DungeonNode // Empty, Camp
	{
		public bool CanRest;

		public StandardNode(RoomType roomType, int depth, bool canRest) : base(roomType, depth)
		{
			CanRest = canRest;
		}
	}

	private class TreasureNode : DungeonNode // Treasure
	{
		//public TreasureBox treasureBox;

		public TreasureNode(RoomType roomType, int depth) : base(roomType, depth)
		{

		}
	}

	private class CombatNode : DungeonNode // StandardCombat, EliteCombat, Boss
	{
		public Entity[] HostileEntities;

		public CombatNode(RoomType roomType, int depth, Entity[] hostileEntities) : base (roomType, depth)
		{
			HostileEntities = hostileEntities;
		}
	}
}
