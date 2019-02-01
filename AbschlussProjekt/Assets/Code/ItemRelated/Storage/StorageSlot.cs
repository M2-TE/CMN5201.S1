using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageSlot
{

    [SerializeField] private int position;
    [SerializeField] private int amount;
    [SerializeField] private string content;
    [SerializeField] private UIElementHandler slot;

    public int Amount
    {
        get
        {
            return amount;
        }
        set
        {
            if(slot != null)
            {
                slot.Amount.enabled = value == 0 ? false : true;
                slot.Amount.SetText(value.ToString());
            }
            amount = value;
        }
    }
    public int Position { get { return position; } set { position = value; } }
    public string Content
    {
        get
        {
            return content;
        }
        set
        {
            content = value;
            if (slot != null)
            {
                slot.Icon.enabled = value == null ? false : true;
                if(value != null)
                    slot.Icon.sprite = LoadContentSprite();
            }
        }
    }
    public UIElementHandler Slot { get { return slot; } set { slot = value; slot.SetPositionInInventory(Position); } }

    private Sprite LoadContentSprite()
    {
        return AssetManager.Instance.Items.LoadAsset<ItemContainer>(content).ItemIcon;
    }

    public void EmptySlot()
    {
        amount = 0;
        Content = null;
    }
}
