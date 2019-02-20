using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chest", menuName = "Data Container/Items/Chest")]
public class Chest : DataContainer
{
    public bool CustomChest = false;

    public bool CompleteRandomness = true;
    public bool[] SelectedPools;
    public int[] Probability;
    public ItemPool AllItems;

    [SerializeField] private int minItems;
    [SerializeField] private int maxItems;

    public List<StorageSlot> Items;

    public void SelectRandomItems()
    {
        Items = new List<StorageSlot>();

        int itemCount = (int)((maxItems-minItems)*Random.value)+minItems;
        int[] poolValues = new int[itemCount];
        int[] itemValues = new int[itemCount];


        if (CompleteRandomness)
        {
            for (int i = 0; i < itemCount; i++)
            {
                poolValues[i] = (int)(Mathf.Max(0,AllItems.Pools.Count-1)*Random.value);
                itemValues[i] = (int)(Mathf.Max(0, AllItems.Pools[poolValues[i]].Count-1)* Random.value);
                Items.Add(new StorageSlot(1,AssetManager.Instance.LoadBundle<ItemContainer>(AssetManager.Instance.Paths.ItemsPath,AllItems.Pools[poolValues[i]][itemValues[i]].ItemName)));
            }
        }
        else
        {
            List<int> pools = new List<int>();
            for (int j = 0; j < SelectedPools.Length; j++)
            {
                for (int k = 0; k < Probability[j]; k++)
                {
                    if (SelectedPools[j])
                        pools.Add(j);
                }
            }
            for (int i = 0; i < itemCount; i++)
            {
                if(pools.Count <= 0)
                    poolValues[i] = (int)(Mathf.Max(0, AllItems.Pools.Count - 1) * Random.value);
                else
                    poolValues[i] = pools[(int)(Mathf.Max(0, (pools.Count-1)* Random.value))];
                itemValues[i] = (int)(Mathf.Max(0, AllItems.Pools[poolValues[i]].Count - 1) * Random.value);
                Items.Add(new StorageSlot(1, AssetManager.Instance.LoadBundle<ItemContainer>(AssetManager.Instance.Paths.ItemsPath, AllItems.Pools[poolValues[i]][itemValues[i]].ItemName)));
            }
        }
    }

}
