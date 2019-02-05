using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class DungeonManager : Manager
{
	private DungeonPanel dungeonPanel;

	private DungeonNode currentNode;
	private Color currentNodeColorBuffer;
	private List<DungeonNode>[] dungeonNodes;
	private Color currentStandingColor = new Color(.5f, 0f, 0f, 1f);

	public void CreateNewDungeon(DungeonPanel dungeonPanel)
	{
		this.dungeonPanel = dungeonPanel;
		GenerateDungeon();
		SetAllNodePositions();
	}

	private void GenerateDungeon()
	{
		dungeonNodes = new List<DungeonNode>[dungeonPanel.DungeonLength];

		var parent = AddNode(CreateNode(DungeonNode.RoomType.Empty), null);
		currentNode = parent;

		var image = parent.GetComponent<Image>();
		currentNodeColorBuffer = image.color;
		image.color = currentStandingColor;
		
		for (int i = 0; i < dungeonPanel.DungeonLength - 2; i++)
		{
			int siblingAmount = Random.Range(1, dungeonPanel.MaxSiblings);
			for (int x = 0; x < siblingAmount; x++)
				AddNode(CreateNode(GetRandomRoomType()), parent);

			parent = AddNode(CreateNode(GetRandomRoomType()), parent);
		}
		AddNode(CreateNode(DungeonNode.RoomType.Boss), parent);
	}

	private DungeonNode.RoomType GetRandomRoomType()
	{
		return (DungeonNode.RoomType)Random.Range(0, Enum.GetNames(typeof(DungeonNode.RoomType)).Length);
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
				break;

			case DungeonNode.RoomType.Treasure:
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
		else
		{
			nodeToAdd.Depth = parentNode.Depth + 1;

			// TODO: conditional parenting
			//nodeToAdd.ParentNode = parentNode;

			//if (parentNode.ChildNodes == null) parentNode.ChildNodes = new List<DungeonNode>();
			//parentNode.ChildNodes.Add(nodeToAdd);
		}
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
					(horizontalNodeStep * dungeonNodes[i][x].Depth + horizontalNodeStep * .25f,
					verticalNodeStep * .5f + x * verticalNodeStep - (dungeonNodes[i].Count * verticalNodeStep * .5f));
	}

	public void MoveToNextNode(DungeonNode newNode)
	{
		currentNode.GetComponent<Image>().color = currentNodeColorBuffer;

		currentNode = newNode;
		var image = currentNode.GetComponent<Image>();
		currentNodeColorBuffer = image.color;
		image.color = currentStandingColor;
	}

	public void ReceiveNodePress(DungeonNode node)
	{
		if (node.Depth == currentNode.Depth + 1)
			MoveToNextNode(node);
		else Debug.Log("Illegal Movement");
	}
}
