using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageSlot
{
    #region Fields & Properties
    private Vector2 position;
    private Item content;

    public bool IsEmpty { get { return content == null; } }
    public Item Content { get { return content; } }
    public Vector2 Position { get { return position; } }
    public int Amount { get { return content ? content.currentlyStacked : 0 ; } }
    #endregion

    #region Constructor
    public StorageSlot(int posX, int posY) : this(posX, posY, null) { }
    public StorageSlot(int posX, int posY, Item content)
    {
        position = new Vector2(posX, posY);
        this.content = content;
    }
    #endregion

    #region SlotManagement
    public Item RemoveAllContent()
    {
        return SwitchItems(null);
    }

    public Item SwitchItems(Item item)
    {
        Item temp = content;
        content = item;
        return content;
    }

    public bool TryAddStacksToContent(int amount)
    {
        return content.TryAddItemAmount(amount);
    }

    public bool TryRemoveStacksFromContent(int amount, out Item removedContent)
    {
        if (content.TryRemoveItemAmount(amount))
        {
            removedContent = content;
            removedContent.currentlyStacked = amount;
            if (content.currentlyStacked == 0)
                content = null;
            return true;
        }
        removedContent = null;
        return false;
    }

    public StorageSlot ChangeToPosition(int posX,int posY)
    {
        position = new Vector2(posX, posY);
        return this;
    }
    #endregion
}
