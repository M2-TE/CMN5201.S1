using UnityEngine;

[CreateAssetMenu(fileName = "New Playable Character", menuName = "Data Container/Playable Character")]
public class PlayableCharacter : Character
{
	[Header("Movespeed")]
	public GameObject CharacterControllerPrefab;
    public float MovespeedMod = 20;
    public float GroundDrag = 10;
    public float AirDrag = .5f;
    public float AttackDelay = 1f;

	[Header("Jumps")]
	public float JumpMod = .6f;
	public int MaxJumps = 2;
	public float GaugePerSecond = 3f;

	[Header("Dashing")]
	public float DashDuration = .1f;
	public float DashMod = 6f;

	[Header("Air Jumps")]
	public float AirJumpDirMod = 3f;
	public float AirJumpMod = .5f;

}
