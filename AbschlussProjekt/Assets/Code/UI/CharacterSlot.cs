using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
	public enum SlotType { OwnedCharacter, EquippedCharacter }
	[NonSerialized] public SlotType OwnSlotType;
	[NonSerialized] public int SlotPos;
	[SerializeField] private Image portraitImage;

	private GeneralCityManager m_manager;
	private GeneralCityManager manager
	{
		get { return m_manager ?? (m_manager = AssetManager.Instance.GetManager<GeneralCityManager>()); }
	}

	private Entity storedEntity;
	public Entity StoredEntity
	{
		get { return storedEntity; }
		set
		{
			storedEntity = value;
			if (value != null)
			{
				portraitImage.sprite = value.CharDataContainer.Portrait;
				portraitImage.color = new Color(1f, 1f, 1f, 1f);
			}
			else
			{
				portraitImage.sprite = null;
				portraitImage.color = new Color(1f, 1f, 1f, .2f);
			}
		}
	}

	public override string ToString()
	{
		return StoredEntity.Name + " " + OwnSlotType + " " + SlotPos;
	}

	#region Events
	public void OnBeginDrag(PointerEventData eventData)
	{
		if(storedEntity != null) manager.OnStartDrag(this);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (storedEntity != null) manager.DuringDrag(this);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (storedEntity != null) manager.OnEndDrag(this);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		manager.OnMouseEnter(this);
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		manager.OnMouseExit(this);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (storedEntity != null) manager.OnPointerClick(this, eventData);
	}
	#endregion
}
