using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageSlot
{
    #region Fields & Properties
    private int position;
    private Item content;

    public bool IsEmpty { get { return content == null; } }
    public Item Content { get { return content; } set { content = value; } }
    public int Position { get { return position; } }
    public int Amount { get { return content ? content.CurrentlyStacked : 0 ; } }
    #endregion

    #region Constructor
    public StorageSlot(int position) : this(position, null) { }
    public StorageSlot(int position, Item content)
    {
        this.position = position;
        this.content = content;
    }
    #endregion

    #region SlotManagement
    public Item RemoveContent()
    {
        return SwitchItems(null);
    }

    public Item SwitchItems(Item item)
    {
        Item temp = content;
        content = item;
        return content;
    }

    public StorageSlot ChangePosition(int position)
    {
        this.position = position;
        return this;
    }
    #endregion
}
