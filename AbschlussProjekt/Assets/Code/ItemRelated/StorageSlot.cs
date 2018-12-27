using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageSlot
{
    private Vector2 position;
    private Item content;

    public bool IsEmpty { get { return content == null; } }
    public Item Content { get { return content; } }
    public int Amount { get { return content ? content.currentlyStacked : 0 ; } }

    public StorageSlot(int posX, int posY) : this(posX, posY, null) { }
    public StorageSlot(int posX, int posY, Item content)
    {
        position = new Vector2(posX, posY);
        this.content = content;
    }

    public Item RemoveAllContent()
    {
        return AddNewContent(null);
    }

    public Item AddNewContent(Item newContent)
    {
        Item temp = content;
        content = newContent;
        return content;
    }

    public bool TryAddContent(int amount)
    {
        return content.TryAddItemAmount(amount);
    }

    public bool TryRemoveContent(int amount, out Item removedContent)
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

}
