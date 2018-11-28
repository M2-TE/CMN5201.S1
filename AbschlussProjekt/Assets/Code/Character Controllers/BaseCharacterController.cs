using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterController : MonoBehaviour {
    #region Variables
    //[Header("Movement")]
    [SerializeField] protected float movespeedMod, jumpMod, groundDrag, airDrag;

    //[Header("Combat")]
    [SerializeField] protected float attackDelay;

    //[Header("Ground Stuff")]
    [SerializeField] protected float groundCheckDst;
    [SerializeField] protected LayerMask groundLayers;

    protected Animator ownAnimator;
    protected Rigidbody2D ownRigidbody;
    protected SpriteRenderer ownSpriteRenderer;

    protected bool grounded = false;
    protected float commandDelayCounter = 0f;
    #endregion

    #region Engine Calls
    protected virtual void Start ()
    {
        ownAnimator = GetComponent<Animator>();
        ownRigidbody = GetComponent<Rigidbody2D>();
        ownSpriteRenderer = GetComponent<SpriteRenderer>();
	}

    protected virtual void Update ()
    {
        UpdateEnvValues();
        HandleActions();
	}
    #endregion

    #region Env Values
    private void UpdateEnvValues()
    {
        CheckForGround();
    }

    private void CheckForGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDst, groundLayers);

        if (hit.collider != null)
        {
            grounded = true;
            ownRigidbody.velocity = new Vector2(ownRigidbody.velocity.x, 0f);
            ownRigidbody.drag = groundDrag;
        }
        else
        {
            ownRigidbody.drag = airDrag;
            grounded = false;
        }
    }
    #endregion

    #region Actions
    private void HandleActions()
    {
        if (commandDelayCounter < 0f) CheckRequestedActions();
        else commandDelayCounter -= Time.deltaTime;
    }

    protected virtual void CheckRequestedActions()
    {
        TryMovement();
        TryBaseAttack();
    }

    private void TryMovement()
    {
        float hForce = Input.GetAxis("Horizontal");
        if (grounded)
        {
            ownRigidbody.AddForce(new Vector2(hForce * movespeedMod, 0f));
            ownAnimator.SetFloat("Movespeed", Mathf.Abs(ownRigidbody.velocity.x));

            if (Input.GetAxis("Jump") > 0)
            {
                ownRigidbody.AddForce(new Vector2(0f, jumpMod), ForceMode2D.Impulse);
                grounded = false;
            }
        }
        else ownAnimator.SetFloat("Movespeed", 0f);
        if (hForce < 0) ownSpriteRenderer.flipX = true;
        else if (hForce > 0) ownSpriteRenderer.flipX = false;
    }

    private void TryBaseAttack()
    {
        if (Input.GetAxis("Fire1") > 0)
        {
            commandDelayCounter = attackDelay;
            ownAnimator.SetFloat("Movespeed", 0f);
            ownAnimator.SetTrigger("Attack");
        }
    }
    #endregion
}
