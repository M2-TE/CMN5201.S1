using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {
    [SerializeField] private float movespeedMod;
    [SerializeField] private float jumpMod;
    [SerializeField] private float groundCheckDst;
    [SerializeField] private float groundDrag;
    [SerializeField] private float airDrag;
    [SerializeField] LayerMask groundLayers;

    private Animator ownAnimator;
    private Rigidbody2D ownRigidbody;
    private SpriteRenderer ownSpriteRenderer;

    private bool grounded = false;

    void Start ()
    {
        ownAnimator = GetComponent<Animator>();
        ownRigidbody = GetComponent<Rigidbody2D>();
        ownSpriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Update ()
    {
        UpdateEnvValues();
        HandleMovement();

	}

    private void UpdateEnvValues()
    {
        Debug.Log(ownRigidbody.velocity);
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

    private void HandleMovement()
    {
        float hForce = Input.GetAxis("Horizontal");
        float vForce = Input.GetAxis("Jump");

        if (grounded)
        {
            //ownRigidbody.MovePosition(new Vector2(hSpeed * movespeedMod + transform.position.x, transform.position.y));
            ownRigidbody.AddForce(new Vector2(hForce * movespeedMod, 0f));
            ownAnimator.SetFloat("Movespeed", Mathf.Abs(ownRigidbody.velocity.x));

            if (vForce > 0)
            {
                ownRigidbody.AddForce(new Vector2(0f, jumpMod), ForceMode2D.Impulse);
                grounded = false;
            }
        }
        else
        {
            ownAnimator.SetFloat("Movespeed", 0f);
        }

        if (hForce < 0) ownSpriteRenderer.flipX = true;
        else if (hForce > 0) ownSpriteRenderer.flipX = false;
    }
}
