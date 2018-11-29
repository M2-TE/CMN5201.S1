using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Character Container", menuName = "Asset Containers/Characters", order = 1)]
public class AssetContainer : ScriptableObject
{
    public Character Knight;
    public Character Gunwoman;
    public Character Mage;
    public float checkValue;

    [Serializable]
    public struct Character
    {
        public GameObject Prefab;
        public Sprite Portrait;
        public bool SkilltreeTODO;
    }
}
