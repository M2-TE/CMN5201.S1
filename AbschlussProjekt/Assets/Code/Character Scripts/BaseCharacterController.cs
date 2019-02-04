using System;
using System.Collections;
using UnityEngine;
using Utilities;

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
		if (collision.CompareTag("Climbable"))
		{
			jumpCount = 0;
			if (hMovement != 0)
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
		//if(ownRigidbody.velocity.y != 0f) ownAnimator.SetFloat("Movespeed", 0f);
		//else 
		ownAnimator.SetFloat("Movespeed", Mathf.Abs(hMovement));

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
}
