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
            amount = value;
            if(slot != null)
            {
                if (value == 0)
                {
                    slot.Amount.enabled = false;
                    Content = null;
                }
                else
                    slot.Amount.enabled = true;
                slot.Amount.SetText(value.ToString());
            }
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
                slot.itemName = value;
                slot.Icon.enabled = value == null ? false : true;
                if(value != null)
                    slot.Icon.sprite = LoadContentSprite();
            }
        }
    }
    public UIElementHandler Slot { get { return slot; } set { slot = value; slot.SetPositionInInventory(Position); } }

    private Sprite LoadContentSprite()
    {
        Debug.Log(content);
		return AssetManager.Instance.LoadBundle<ItemContainer>(AssetManager.Instance.Paths.ItemsPath, content).ItemIcon;
	}

    public void EmptySlot()
    {
        Amount = 0;
    }
}
