using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Always have Unequip as the last Action Type since it can only be called on already equiped items unlike the others. 

public abstract class UIElementHandler : MonoBehaviour , IPointerEnterHandler,IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
    [SerializeField] private Image icon;

    public Image Icon => icon;

    protected bool empty = true;

    #region Handlerfunctions

    public abstract void OnSelect(BaseEventData eventData);

    public abstract void OnDeselect(BaseEventData eventData);

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnSelect(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnDeselect(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPrimaryAction();
    }

    protected abstract void OnPrimaryAction();
    #endregion

    public abstract void SetEmpty();
}
