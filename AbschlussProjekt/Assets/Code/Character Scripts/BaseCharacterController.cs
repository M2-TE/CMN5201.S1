using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class BaseCharacterController : MonoBehaviour {
	[SerializeField] private PlayableCharacter charDataContainer;

	private SpriteRenderer ownSpriteRenderer;
	private Animator ownAnimator;
	private Rigidbody2D ownRigidbody;
	private CapsuleCollider2D mainCollider;

	private InputBuffer inputBuffer;
	private float hMovement;

	private int jumpCount;
	private bool grounded;

	protected virtual void Awake()
	{
		ownSpriteRenderer = GetComponent<SpriteRenderer>();
		ownAnimator = GetComponent<Animator>();
		ownRigidbody = GetComponent<Rigidbody2D>();
		mainCollider = GetComponent<CapsuleCollider2D>();

		inputBuffer = new InputBuffer(this, .1f);
	}

	protected virtual void Update()
	{
		HandleInput();
		AdjustSprite();
	}

	protected virtual void FixedUpdate()
	{
		HandleMovement();
	}

	protected virtual void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Climbable") && hMovement != 0)
		{
			bool climbDirection = hMovement > 0; // false => left, true => right
			Vector3 targetPos = collision.bounds.center + new Vector3
					   (collision.bounds.extents.x * (climbDirection ? -1 : 1),
					   collision.bounds.extents.y + mainCollider.size.y * .5f,
					   0f);
			float climbHeight = targetPos.y - transform.position.y;

			if (climbHeight < charDataContainer.maxClimbHeight &&
					(climbDirection && targetPos.x > transform.position.x ||
					!climbDirection && targetPos.x < transform.position.x))
			{
				// climb
				ownRigidbody.MovePosition(targetPos);
				ownRigidbody.velocity = Vector2.zero;
			}
		}
	}

	protected virtual void OnCollisionEnter2D(Collision2D collision)
	{
		for(int i = 0; i < collision.contactCount; i++)
		{
			// ground check
			if(jumpCount != 0 && collision.GetContact(i).point.y < transform.position.y - (mainCollider.size.y * .5f - mainCollider.size.x * .5f))
			{
				jumpCount = 0;
				grounded = true;
			}
		}
	}

	protected virtual void OnCollisionStay2D(Collision2D collision)
	{
		for(int i = 0; i < collision.contactCount; i++)
		{
			if (ownRigidbody.velocity.y < 0f && collision.GetContact(i).point.y > transform.position.y - (mainCollider.size.y * .5f - mainCollider.size.x * .5f))
			{
				ownRigidbody.velocity = new Vector2(0f, -charDataContainer.wallSlideVelocity);
			}
		}
	}

	protected virtual void OnDrawGizmos()
	{
		if (mainCollider == null) mainCollider = GetComponent<CapsuleCollider2D>();
		Vector3 startPos = transform.position + new Vector3(0f, mainCollider.offset.y - mainCollider.size.y * .5f, 0f);
		Gizmos.DrawLine
			(startPos, startPos + new Vector3(0f, mainCollider.offset.y + charDataContainer.maxClimbHeight, 0f));
	}
	
	private void HandleInput()
	{
		hMovement = Input.GetAxis("Horizontal") * Time.deltaTime * charDataContainer.moveSpeed;
		if (Input.GetKeyDown(KeyCode.Space)) inputBuffer.BufferInput(KeyCode.Space);
	}

	private void AdjustSprite()
	{
		if(ownRigidbody.velocity.y != 0f) ownAnimator.SetFloat("Movespeed", 0f);
		else ownAnimator.SetFloat("Movespeed", Mathf.Abs(hMovement));

		if (hMovement != 0)
		{
			ownSpriteRenderer.flipX = hMovement < 0;
		}
	}

	private void HandleMovement()
	{
		ownRigidbody.velocity = new Vector2(hMovement, ownRigidbody.velocity.y);

		if (jumpCount < charDataContainer.maxJumps && inputBuffer.GetKey(KeyCode.Space))
		{
			jumpCount++;
			grounded = false;
			ownRigidbody.velocity = new Vector2(ownRigidbody.velocity.x, 0f);
			ownRigidbody.AddForce(new Vector2(0f, charDataContainer.jumpForce), ForceMode2D.Impulse);
		}
	}

	//   #region Variables
	//   [SerializeField]
	//   protected PlayableCharacter charDataContainer;
	//   protected MiscSettings miscSettings;

	//protected Animator ownAnimator;
	//protected Rigidbody2D ownRigidbody;
	//protected SpriteRenderer ownSpriteRenderer;
	//protected BoxCollider2D ownCollider;

	//protected Vector2 localGroundPosLeft;
	//protected Vector2 localGroundPosRight;
	//protected bool grounded = false;

	//protected bool isJumping;
	//protected float currentJumpGauge;
	//protected float maxJumpGauge = 1f;

	//protected int remainingJumps;

	//protected float commandDelayCounter = 0f;
	//   #endregion

	//   #region Engine Calls
	//   protected virtual void Start ()
	//   {
	//       ownAnimator = GetComponent<Animator>();
	//       ownRigidbody = GetComponent<Rigidbody2D>();
	//       ownSpriteRenderer = GetComponent<SpriteRenderer>();
	//       ownCollider = GetComponent<BoxCollider2D>();

	//       miscSettings = AssetManager.Instance.Settings.LoadAsset<MiscSettings>("Misc Settings");

	//       localGroundPosLeft = new Vector2(ownCollider.offset.x - ownCollider.size.y * .5f, ownCollider.offset.y - ownCollider.size.y * .5f);
	//       localGroundPosRight = new Vector2(ownCollider.offset.x + ownCollider.size.x * .5f, ownCollider.offset.y - ownCollider.size.y * .5f);
	//   }

	//   protected virtual void Update ()
	//   {
	//       UpdateEnvValues();
	//	HandleActions();
	//}
	//   #endregion

	//   #region Env Values
	//   private void UpdateEnvValues()
	//   {
	//       CheckForGround();
	//   }

	//   private void CheckForGround()
	//   {
	//       RaycastHit2D hitLeft = Physics2D.Raycast((Vector2)transform.position + localGroundPosLeft, Vector2.down, .05f, miscSettings.GroundLayers);
	//       RaycastHit2D hitRight = Physics2D.Raycast((Vector2)transform.position + localGroundPosRight, Vector2.down, .05f, miscSettings.GroundLayers);

	//       if (!isJumping && (hitLeft.collider != null || hitRight.collider != null))
	//       {
	//		remainingJumps = charDataContainer.MaxJumps;
	//           grounded = true;
	//           ownRigidbody.drag = charDataContainer.GroundDrag;
	//       }
	//       else
	//       {
	//           ownRigidbody.drag = charDataContainer.AirDrag;
	//           grounded = false;
	//       }
	//   }
	//#endregion

	//#region Actions
	//private void HandleActions()
	//   {
	//       if (commandDelayCounter < 0f) CheckRequestedActions();
	//       else commandDelayCounter -= Time.deltaTime;
	//   }

	//   protected virtual void CheckRequestedActions()
	//   {
	//	HandleMovement();

	//	// do this last
	//	HandleBaseAttack();
	//}

	//   private void HandleBaseAttack()
	//   {
	//       if (Input.GetAxis("Fire1") > 0)
	//	{
	//		commandDelayCounter = charDataContainer.AttackDelay;
	//		ownAnimator.SetFloat("Movespeed", 0f);
	//		ownAnimator.SetTrigger("Attack");
	//	}
	//}

	//private void HandleMovement()
	//{
	//	float hInput = Input.GetAxis("Horizontal");
	//	float vInput = Input.GetAxis("Vertical");

	//	// set sprite orientation based on movement direction
	//	if (hInput < 0) ownSpriteRenderer.flipX = true;
	//	else if (hInput > 0) ownSpriteRenderer.flipX = false;

	//	#region Horizontal Movement
	//	if (grounded)
	//	{
	//		ownRigidbody.AddForce(new Vector2(hInput * charDataContainer.MovespeedMod, 0f));
	//		ownAnimator.SetFloat("Movespeed", Mathf.Abs(ownRigidbody.velocity.x));
	//	}
	//	else ownAnimator.SetFloat("Movespeed", 0f);
	//	#endregion

	//	// jumping
	//	if (Input.GetKeyDown(KeyCode.Space))
	//	{
	//		if (grounded) StartCoroutine(Jump(false));
	//		else if ((hInput == 0 && vInput == 0) || vInput > 0 && remainingJumps > 0) StartCoroutine(Jump(true));
	//		else if (hInput != 0 && remainingJumps > 0) StartCoroutine(Dash());
	//	}
	//}

	//private IEnumerator Dash()
	//{
	//	remainingJumps--;
	//	ownRigidbody.velocity = new Vector2(0f, 0f);
	//	switch (ownSpriteRenderer.flipX)
	//	{
	//		case true:
	//			ownRigidbody.AddForce(new Vector2(-charDataContainer.DashMod, 0f), ForceMode2D.Impulse);
	//			break;

	//		case false:
	//			ownRigidbody.AddForce(new Vector2(charDataContainer.DashMod, 0f), ForceMode2D.Impulse);
	//			break;
	//	}

	//	float counter = charDataContainer.DashDuration;
	//	while (counter > 0)
	//	{
	//		ownRigidbody.velocity = new Vector2(ownRigidbody.velocity.x, 0f);
	//		counter -= Time.deltaTime;
	//		yield return null;
	//	}
	//}

	//private IEnumerator Jump(bool isAirJump)
	//{
	//	float actualJumpModifier = (isAirJump) ? charDataContainer.AirJumpMod : charDataContainer.JumpMod;
	//	currentJumpGauge = maxJumpGauge;
	//	grounded = false;
	//	isJumping = true;

	//	if (isAirJump)
	//	{
	//		remainingJumps--;
	//		ownRigidbody.velocity = Vector2.zero;
	//		switch (ownSpriteRenderer.flipX)
	//		{
	//			case true:
	//				ownRigidbody.AddForce(new Vector2(-charDataContainer.AirJumpDirMod, 0f), ForceMode2D.Impulse);
	//				break;

	//			case false:
	//				ownRigidbody.AddForce(new Vector2(charDataContainer.AirJumpDirMod, 0f), ForceMode2D.Impulse);
	//				break;
	//		}
	//	}

	//	while (Input.GetKey(KeyCode.Space) && currentJumpGauge > 0)
	//	{
	//		ownRigidbody.AddForce(new Vector2(0f, (currentJumpGauge * currentJumpGauge) * actualJumpModifier), ForceMode2D.Impulse);
	//		currentJumpGauge = Mathf.MoveTowards(currentJumpGauge, 0f, charDataContainer.GaugePerSecond * Time.deltaTime);
	//		yield return null;
	//	}

	//	isJumping = false;
	//}
	//#endregion
}
