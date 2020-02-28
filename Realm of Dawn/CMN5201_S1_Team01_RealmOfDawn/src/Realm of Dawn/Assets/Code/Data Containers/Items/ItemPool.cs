using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "New Pool", menuName = "Data Container/Items/Pool")]
public class ItemPool : DataContainer
{ 
    public MyDictionary[] m_Pools;

    public List<List<ItemContainer>> Pools
    {
        get
        {
            List<List<ItemContainer>> result = new List<List<ItemContainer>>();
            foreach (MyDictionary item in m_Pools)
            {
                result.Add( new List<ItemContainer>(item.items));
            }
            return result;
        }
    }

    public string[] GetPoolNames()
    {
        string[] poolsStrings = new string[m_Pools.Length];
        for (int i = 0; i < m_Pools.Length; i++)
        {
            poolsStrings[i] = m_Pools[i].poolName;
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
