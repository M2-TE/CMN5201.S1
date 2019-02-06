using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable] public class StorageSlot
{
    public int amount;
    public string content;

    public ItemContainer Item => content != "" ? AssetManager.Instance.LoadBundle<ItemContainer>(AssetManager.Instance.Paths.ItemsPath, content) : null;

    public StorageSlot(int amount, string item)
    {
        this.amount = amount;
        content = item;
    }
}
