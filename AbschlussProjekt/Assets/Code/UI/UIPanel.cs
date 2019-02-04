using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UIPanel : MonoBehaviour
{
	[NonSerialized] public EventSystem EventSystem;
	[SerializeField] protected GameObject firstSelectedElement;
	[SerializeField] protected GameObject visibilityToggleNode;
	[SerializeField] private bool visibilityOnAwake;

	protected virtual void Awake()
	{
		EventSystem = transform.parent.GetComponentInChildren<EventSystem>();
		ToggleVisibility(visibilityOnAwake);
	}

	public virtual void ToggleVisibility()
	{
		ToggleVisibility(!visibilityToggleNode.activeInHierarchy);
	}
	public virtual void ToggleVisibility(bool visibleState)
	{
		visibilityToggleNode.SetActive(visibleState);

		if (visibleState)
		{
			EventSystem.SetSelectedGameObject(null);
			EventSystem.SetSelectedGameObject(firstSelectedElement);
		}
	}
}
