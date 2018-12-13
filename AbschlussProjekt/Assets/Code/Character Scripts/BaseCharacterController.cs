using System;
using UnityEngine;

public class BaseCharacterController : MonoBehaviour {
    #region Variables
    [NonSerialized] public static bool characterControlEnabled;


    [SerializeField]
    protected PlayableCharacter charDataContainer;
    protected MiscSettings miscSettings;

    #region Components
    protected Animator ownAnimator;
    protected Rigidbody2D ownRigidbody;
    protected SpriteRenderer ownSpriteRenderer;
    protected BoxCollider2D ownCollider;
    #endregion

    #region Ground Detection
    protected Vector2 localGroundPosLeft;
    protected Vector2 localGroundPosRight;
    protected bool grounded = false;
    #endregion

    protected float commandDelayCounter = 0f;
    #endregion

    #region Engine Calls
    protected virtual void Start ()
    {
        ownAnimator = GetComponent<Animator>();
        ownRigidbody = GetComponent<Rigidbody2D>();
        ownSpriteRenderer = GetComponent<SpriteRenderer>();
        ownCollider = GetComponent<BoxCollider2D>();

        miscSettings = AssetManager.Instance.Settings.LoadAsset<MiscSettings>("Misc Settings");
        
        localGroundPosLeft = new Vector2(ownCollider.offset.x - ownCollider.size.y * .5f, ownCollider.offset.y - ownCollider.size.y * .5f);
        localGroundPosRight = new Vector2(ownCollider.offset.x + ownCollider.size.x * .5f, ownCollider.offset.y - ownCollider.size.y * .5f);
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
        RaycastHit2D hitLeft = Physics2D.Raycast((Vector2)transform.position + localGroundPosLeft, Vector2.down, .05f, miscSettings.GroundLayers);
        RaycastHit2D hitRight = Physics2D.Raycast((Vector2)transform.position + localGroundPosRight, Vector2.down, .05f, miscSettings.GroundLayers);

        if (hitLeft.collider != null || hitRight.collider != null)
        {
            grounded = true;
            ownRigidbody.velocity = new Vector2(ownRigidbody.velocity.x, 0f);
            ownRigidbody.drag = charDataContainer.GroundDrag;
        }
        else
        {
            ownRigidbody.drag = charDataContainer.AirDrag;
            grounded = false;
        }
    }
    #endregion

    #region Actions
    private void HandleActions()
    {
        if (!characterControlEnabled) return;

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
        if (hForce < 0) ownSpriteRenderer.flipX = true;
        else if (hForce > 0) ownSpriteRenderer.flipX = false;

        if (grounded)
        {

            ownRigidbody.AddForce(new Vector2(hForce * charDataContainer.MovespeedMod, 0f));
            ownAnimator.SetFloat("Movespeed", Mathf.Abs(ownRigidbody.velocity.x));

            if (Input.GetAxis("Jump") > 0)
            {
                ownRigidbody.AddForce(new Vector2(0f, charDataContainer.JumpMod), ForceMode2D.Impulse);
                grounded = false;
            }
        }
        else ownAnimator.SetFloat("Movespeed", 0f);
    }

    private void TryBaseAttack()
    {
        if (Input.GetAxis("Fire1") > 0)
        {
            commandDelayCounter = charDataContainer.AttackDelay;
            ownAnimator.SetFloat("Movespeed", 0f);
            ownAnimator.SetTrigger("Attack");
        }
    }
    #endregion
}
