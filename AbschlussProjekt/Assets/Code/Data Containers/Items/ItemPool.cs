using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "New Pool", menuName = "Data Container/Items/Pool")]
public class ItemPool : DataContainer
{
    [SerializeField] private MyDictionary[] pools;

    public List<List<ItemContainer>> Pools
    {
        get
        {
            List<List<ItemContainer>> result = new List<List<ItemContainer>>();
            foreach (MyDictionary item in pools)
            {
                result.Add( new List<ItemContainer>(item.items));
            }
            return result;
        }
    }

    public string[] GetPoolNames()
    {
        string[] poolsStrings = new string[pools.Length];
        for (int i = 0; i < pools.Length; i++)
        {
            poolsStrings[i] = pools[i].poolName;
        }
        return poolsStrings;
    }

    [System.Serializable]
    public struct MyDictionary
    {
        public string poolName;
        public ItemContainer[] items;
    }
}
