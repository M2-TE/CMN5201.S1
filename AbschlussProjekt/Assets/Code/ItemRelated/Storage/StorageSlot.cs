using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageSlot
{

    [SerializeField] private int position;
    [SerializeField] private int amount;
    [SerializeField] private string content;
    [SerializeField] private SlotHolder slot;

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
            if (slot != null)
            {
                slot.Icon.enabled = value == null ? false : true;
                slot.Icon.sprite = LoadContentSprite();
            }
            content = value;
        }
    }
    public SlotHolder Slot { get { return slot; } set { slot = value; } }

    private Sprite LoadContentSprite()
    {
        return ((ItemContainer)AssetManager.Instance.Items.LoadAsset(content)).ItemIcon;
    }
}
