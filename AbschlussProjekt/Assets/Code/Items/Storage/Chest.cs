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

    public List<ItemContainer> Items;

    void Awake()
    {
        if (completeRandomness || (selectedPools != null && probability != null))
            SelectRandomItems();
        else
            Debug.LogError("A Chest has a problem choosing items from the Pools");

    }


    private void SelectRandomItems()
    {
        Items = new List<ItemContainer>();

        int itemCount = (int)((maxItems-minItems)/Random.value)+minItems;
        int[] poolValues = new int[itemCount];
        int[] itemValues = new int[itemCount];

        if (completeRandomness)
        {
            for (int i = 0; i < itemCount; i++)
            {
                poolValues[i] = CalcRuleOfThree(0, allItems.Pools.Count, Random.value);
                itemValues[i] = CalcRuleOfThree(0, allItems.Pools[poolValues[i]].Count, Random.value);

                Items.Add(allItems.Pools[poolValues[i]][itemValues[i]]);
            }
        }
        else
        {
            List<int> pools = new List<int>();
            for (int j = 0; j < poolValues.Length; j++)
            {
                for (int k = 0; k < probability[j]; k++)
                {
                    pools.Add(poolValues[j]);
                }
            }
            for (int i = 0; i < itemCount; i++)
            {
                ShuffleListsRandom.ShuffleList<int>(pools);
                poolValues[i] = pools[CalcRuleOfThree(0, pools.Count, Random.value)];
                itemValues[i] = CalcRuleOfThree(0, allItems.Pools[poolValues[i]].Count, Random.value);
                Items.Add(allItems.Pools[poolValues[i]][itemValues[i]]);
            }
        }

        allItems = null;
    }

    private int CalcRuleOfThree(int first, int second, float percentage)
    {
        Debug.Log("Percentage: " + percentage);
        return (int)((first - second) / (percentage > 0 ? percentage : 0.1f)) + second;
    }
}
