using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class DungeonManager : Manager
{
	public Entity[] BufferedEnemies;

	private DungeonPanel dungeonPanel;

	private DungeonNode currentNode;
	private Color currentNodeColorBuffer;
	private List<DungeonNode>[] dungeonNodes;

	public void CreateNewDungeon(DungeonPanel dungeonPanel)
	{
		this.dungeonPanel = dungeonPanel;
		GenerateDungeon();
		SetAllNodePositions();
		SetAllNodeFamilies();
	}

	private void GenerateDungeon()
	{
		dungeonNodes = new List<DungeonNode>[dungeonPanel.DungeonLength];

		var parent = AddNode(CreateNode(DungeonNode.RoomType.Empty), null);
		currentNode = parent;

		var image = parent.GetComponent<Image>();
		currentNodeColorBuffer = image.color;
		image.color = dungeonPanel.CurrentStandingColor;
		
		for (int depth = 0; depth < dungeonPanel.DungeonLength - 2; depth++)
		{
			int siblingAmount = Random.Range(1, dungeonPanel.MaxSiblings);
			for (int x = 0; x < siblingAmount; x++)
				AddNode(CreateNode(GetRandomRoomType(depth)), parent);

			parent = AddNode(CreateNode(GetRandomRoomType(depth)), parent);
		}
		AddNode(CreateNode(DungeonNode.RoomType.Boss), parent);
	}

	private DungeonNode.RoomType GetRandomRoomType(int depth)
	{
		// compensate for gen smoothing
		int maxDepth = dungeonPanel.DungeonLength - 2; 
		depth++;

		if (depth == 0) return DungeonNode.RoomType.Empty;
		else if (depth == maxDepth) return DungeonNode.RoomType.Camp;
		else if (depth < maxDepth * .5f) return dungeonPanel.firstHalfRoomPool[Random.Range(0, dungeonPanel.firstHalfRoomPool.Length)];
		else return dungeonPanel.secondHalfRoomPool[Random.Range(0, dungeonPanel.secondHalfRoomPool.Length)];
	}

	private DungeonNode CreateNode(DungeonNode.RoomType roomType)
	{
		DungeonNode node = null;
		switch (roomType)
		{
			default:
			case DungeonNode.RoomType.Empty:
				node = Object.Instantiate(dungeonPanel.EmptyNodePrefab, dungeonPanel.MapParent.transform).GetComponent<DungeonNode>();
				break;

			case DungeonNode.RoomType.Camp:
				node = Object.Instantiate(dungeonPanel.CampfireNodePrefab, dungeonPanel.MapParent.transform).GetComponent<DungeonNode>();
				break;

			case DungeonNode.RoomType.StandardCombat:
			case DungeonNode.RoomType.EliteCombat:
			case DungeonNode.RoomType.Boss:
				node = Object.Instantiate(dungeonPanel.CombatNodePrefab, dungeonPanel.MapParent.transform).GetComponent<DungeonNode>();
				(node as CombatNode).HostileEntities = CreateEnemyGroup(roomType);
				break;

			case DungeonNode.RoomType.Unknown:
				node = Object.Instantiate(dungeonPanel.UnknownNodePrefab, dungeonPanel.MapParent.transform).GetComponent<DungeonNode>();
				break;
		}
		node.OwnRoomType = roomType;
		return node;
	}

	private DungeonNode AddNode(DungeonNode nodeToAdd, DungeonNode parentNode)
	{
		if (parentNode == null)
		{
			nodeToAdd.Depth = 0;
			currentNode = nodeToAdd;
		}
		else nodeToAdd.Depth = parentNode.Depth + 1;

		(dungeonNodes[nodeToAdd.Depth] ?? (dungeonNodes[nodeToAdd.Depth] = new List<DungeonNode>())).Add(nodeToAdd);
		return nodeToAdd;
	}

	private void SetAllNodePositions()
	{
		var image = dungeonPanel.MapParent.GetComponent<Image>();
		var horizontalNodeStep = image.rectTransform.sizeDelta.x / dungeonPanel.DungeonLength;
		var verticalNodeStep = image.rectTransform.sizeDelta.y / dungeonPanel.MaxSiblings;

		for(int i = 0; i < dungeonNodes.Length; i++)
			for (int x = 0; x < dungeonNodes[i].Count; x++)
				(dungeonNodes[i][x].transform as RectTransform).anchoredPosition = new Vector2
					(horizontalNodeStep * dungeonNodes[i][x].Depth + horizontalNodeStep / dungeonPanel.DungeonLength,
					verticalNodeStep * .5f + x * verticalNodeStep - (dungeonNodes[i].Count * verticalNodeStep * .5f));
	}

	private void SetAllNodeFamilies()
	{
		for (int i = 1; i < dungeonPanel.DungeonLength; i++)
			SetParents(dungeonNodes[i]);
	}

	private void SetParents(List<DungeonNode> siblings)
	{
		List<DungeonNode> parents = dungeonNodes[siblings[0].Depth - 1];

		if (siblings.Count - 1 <= 0)
			for (int i = 0; i < parents.Count; i++)
				ConnectNodes(siblings[0], parents[i]);

		else if (siblings.Count >= parents.Count)
			for (int i = 0; i < siblings.Count; i++)
				ConnectNodes(siblings[i], parents[(int)(i * (((float)parents.Count - 1f) / ((float)siblings.Count - 1f)))]);

		else
			for (int i = 0; i < parents.Count; i++)
				ConnectNodes(siblings[(int)(i * (((float)siblings.Count - 1f) / ((float)parents.Count - 1f)))], parents[i]);

		// RANDOMIZER #1
		//for(int i = 0; i < siblings.Count; i++)
		//{
		//	int parentIndex = Random.Range(0, parents.Count);
		//	if (!parents[parentIndex].ChildNodes.Contains(siblings[i]) && Random.Range(0f, 1f) > .66f)
		//		ConnectNodes(siblings[i], parents[parentIndex]);
		//}

		// RANDOMIZER #2
		for (int i = 0; i < siblings.Count; i++)
		{
			int parentIndex = Random.Range(Mathf.Min(Mathf.Max(i - 1, 0), parents.Count - 1) , Mathf.Min(i + 1, parents.Count - 1));
			if (!parents[parentIndex].ChildNodes.Contains(siblings[i]) && Random.Range(0f, 1f) > dungeonPanel.randomPathingChance)
				ConnectNodes(siblings[i], parents[parentIndex]);
		}
	}

	private void ConnectNodes(DungeonNode childNode, DungeonNode parentNode)
	{
		childNode.ParentNode = parentNode;

		if (parentNode.ChildNodes == null) parentNode.ChildNodes = new List<DungeonNode>();
		parentNode.ChildNodes.Add(childNode);

		var lineRenderer = Object.Instantiate(dungeonPanel.LineRendererPrefab, parentNode.transform).GetComponent<LineRenderer>();
		lineRenderer.positionCount = 2;
		
		Vector3 parentPos = parentNode.transform.position + new Vector3((parentNode.transform as RectTransform).sizeDelta.x * .005f, 0f, -1f);
		Vector3 childPos = childNode.transform.position + new Vector3((childNode.transform as RectTransform).sizeDelta.x * .005f, 0f, -1f);

		Vector3 distVec = (childPos - parentPos).normalized * (parentNode.transform as RectTransform).sizeDelta.x * .007f;

		lineRenderer.SetPosition(0, parentPos + distVec);
		lineRenderer.SetPosition(1, childPos - distVec);
	}

	private void MoveToNextNode(DungeonNode newNode)
	{
		currentNode.GetComponent<Image>().color = currentNodeColorBuffer;

		currentNode = newNode;
		var image = currentNode.GetComponent<Image>();
		currentNodeColorBuffer = image.color;
		image.color = dungeonPanel.CurrentStandingColor;

		switch (newNode.OwnRoomType)
		{
			default:
			case DungeonNode.RoomType.Empty:

				break;

			case DungeonNode.RoomType.Camp:

				break;

			case DungeonNode.RoomType.StandardCombat:
			case DungeonNode.RoomType.EliteCombat:
				BufferedEnemies = (newNode as CombatNode).HostileEntities;
				var instance = AssetManager.Instance;
				instance.GetManager<GameManager>().LoadCombatAreaAsync(instance.LoadArea(instance.Paths.StandardCombatArea));
				break;

			case DungeonNode.RoomType.Boss:

				break;

			case DungeonNode.RoomType.Unknown:

				break;
		}
	}

	private Entity[] CreateEnemyGroup(DungeonNode.RoomType roomType)
	{
		var instance = AssetManager.Instance;
		return BufferedEnemies = new Entity[]
		{
			//new Entity(instance.LoadBundle<PlayableCharacter>(instance.Paths.PlayableCharactersPath, "Knight"))
			new Entity(instance.LoadBundle<PlayableCharacter>(instance.Paths.PlayableCharactersPath, "Mage"))
		};
	}

	public void ExtendNodePress(DungeonNode node)
	{
		if (currentNode.ChildNodes != null && currentNode.ChildNodes.Contains(node))
			MoveToNextNode(node);
		else Debug.Log("Illegal Movement");
	}
}