using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private bool completeRandomness = true;
    [SerializeField] private int[] selectedPools;
    [SerializeField] private int[] probability;
    [SerializeField] private ItemPool allItems;

    [SerializeField] private int minItems;
    [SerializeField] private int maxItems;

    public List<StorageSlot> Items;

    void Awake()
    {
        if (completeRandomness || (selectedPools != null && probability != null))
            SelectRandomItems();
        else
            Debug.LogError("A Chest has a problem choosing items from the Pools");

    }


    private void SelectRandomItems()
    {
        Items = new List<StorageSlot>();

        int itemCount = (int)((maxItems-minItems)*Random.value)+minItems;
        int[] poolValues = new int[itemCount];
        int[] itemValues = new int[itemCount];


        if (completeRandomness)
        {
            for (int i = 0; i < itemCount; i++)
            {
                poolValues[i] = (int)(Mathf.Max(0,allItems.Pools.Count-1)*Random.value);
                itemValues[i] = (int)(Mathf.Max(0, allItems.Pools[poolValues[i]].Count-1)* Random.value);

                Items.Add(new StorageSlot(1,allItems.Pools[poolValues[i]][itemValues[i]]));
            }
        }
        else
        {
            List<int> pools = new List<int>();
            for (int j = 0; j < selectedPools.Length; j++)
            {
                for (int k = 0; k < probability[j]; k++)
                {
                    pools.Add(selectedPools[j]);
                }
            }
            for (int i = 0; i < itemCount; i++)
            {
                poolValues[i] = pools[(int)(Mathf.Max(0, (pools.Count-1)* Random.value))];
                itemValues[i] = (int)(Mathf.Max(0, allItems.Pools[poolValues[i]].Count - 1) * Random.value);
                Items.Add(new StorageSlot(1,allItems.Pools[poolValues[i]][itemValues[i]]));
            }
        }

        allItems = null;
    }
}
