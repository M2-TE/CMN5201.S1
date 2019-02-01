using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UIPanel : MonoBehaviour
{
	[NonSerialized] public EventSystem EventSystem;
	[SerializeField] protected GameObject firstSelectedElement;
	[SerializeField] protected GameObject visibilityToggleNode;

	private void Awake()
	{
		EventSystem = transform.parent.GetComponentInChildren<EventSystem>();
	}
	
	protected void ToggleVisibility()
	{
		ToggleVisibility(!visibilityToggleNode.activeInHierarchy);
	}
	protected virtual void ToggleVisibility(bool visibleState)
	{
		visibilityToggleNode.SetActive(visibleState);

		if (visibleState)
		{
			EventSystem.SetSelectedGameObject(null);
			EventSystem.SetSelectedGameObject(firstSelectedElement);
		}
	}
}
