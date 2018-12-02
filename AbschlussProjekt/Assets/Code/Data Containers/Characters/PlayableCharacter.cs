using UnityEngine;

[CreateAssetMenu(fileName = "New Playable Character", menuName = "Data Container/Playable Character")]
public class PlayableCharacter : Character
{
    [Header("CharContr Values")]
    public float MovespeedMod;
    public float JumpMod;
    public float GroundDrag;
    public float AirDrag;
    public float AttackDelay;
}
