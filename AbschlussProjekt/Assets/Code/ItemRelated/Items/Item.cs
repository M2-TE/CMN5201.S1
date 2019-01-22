using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour
{

    #region Fields

    [SerializeField] private ItemContainer container;

    [Range(2, 4)][SerializeField]private float itemSize = 3;

    private int itemID;

    [SerializeField] private int currentlyStacked;

    public int CurrentlyStacked {  get { return currentlyStacked; } set { currentlyStacked = (value > container.StackingLimit) || (value < 1) ? currentlyStacked : value; } }

    #endregion

    #region DataContainerFieldsForCustomEditorOnly

    #region ItemContainer
    [SerializeField] private string ItemName { get { return container == null ? null : container.ItemName; } set { if (container != null) container.ItemName = value; } }
    [SerializeField] private Sprite ItemIcon { get { return container == null ? null : container.ItemIcon; } set { if (container != null) container.ItemIcon = value; } }
    [SerializeField] private string ItemDescription { get { return container == null ? null : container.ItemDescription; } set { if (container != null) container.ItemDescription = value; } }

    [SerializeField] private int EncumbranceValue { get { return container == null ? -1 : container.EncumbranceValue; } set { if (container != null) container.EncumbranceValue = value; } }
    [SerializeField] private int LevelRequirement { get { return container == null ? -1 : container.LevelRequirement; } set { if (container != null) container.LevelRequirement = value; } }

    [SerializeField] private bool IsIndestructible { get { return container == null ? false : container.IsIndestructible; } set { if (container != null) container.IsIndestructible = value; } }
    [SerializeField] private bool IsQuestItem { get { return container == null ? false : container.IsQuestItem; } set { if (container != null) container.IsQuestItem = value; } }
    [SerializeField] private bool IsUnique{ get { return container == null ? false : container.IsUnique; } set { if (container != null) container.IsUnique = value; } }

    [SerializeField] private int StackingLimit { get { return container == null ? -1 : container.StackingLimit; } set { if (container != null) container.StackingLimit = value; } }
    #endregion

    #region EquipmentContainer
//------------
    #endregion

    #endregion

    private void Start()
    {
        SetSprite();
        SetID();
        SetCorrectSize();
        SetEditorName();
    }

    private void SetSprite()
    {
        if(container !=null)
            GetComponent<SpriteRenderer>().sprite = container.ItemIcon;
    }

    private void SetID()
    {
        itemID = GetInstanceID();
    }

    private void SetCorrectSize()
    {
        transform.localScale = Vector3.one * itemSize;
    }

    private void SetEditorName()
    {
        name = container.name + " ID[" + itemID + "]";
    }
}
