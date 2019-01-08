using UnityEngine;

[CreateAssetMenu(fileName = "New Playable Character", menuName = "Data Container/Playable Character")]
public class PlayableCharacter : Character
{
	[Header("Horizontal Movement")]
	//public GameObject CharacterControllerPrefab;
	public float moveSpeed = 150f;
	public float wallSlideVelocity = .1f;

	[Header("Vertical Movement")]
	public float maxClimbHeight = .9f;
	public int maxJumps = 2;
	public float jumpForce = 7f;
}
