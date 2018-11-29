using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Playable Characters", menuName = "Scriptable Object/Playable Characters", order = 1)]
public class PlayableCharacters : ScriptableObject
{
    public Character Knight;
    public Character Gunwoman;
    public Character Mage;

    [Serializable]
    public struct Character
    {
        public GameObject Prefab;
        public Sprite Portrait;
        public bool SkilltreeTODO;

        public float MovespeedMod;
        public float JumpMod;
        public float GroundDrag;
        public float AirDrag;
        public float AttackDelay;
    }
}
