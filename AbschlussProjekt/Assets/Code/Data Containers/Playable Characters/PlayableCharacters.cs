using UnityEngine;

public abstract class PlayableCharacter : DataContainer
{
    public GameObject Prefab;
    public Sprite Portrait;
    public GameObject ImpactPrefab;
    public bool SkilltreeTODO;

    public float MovespeedMod;
    public float JumpMod;
    public float GroundDrag;
    public float AirDrag;
    public float AttackDelay;
}
