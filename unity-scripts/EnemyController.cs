using System.Collections;
using System.Collections.Generic;
using System; // for Math
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform transform_reference;
    [SerializeField] SpriteRenderer sprite_renderer;
    [SerializeField] Rigidbody2D rigidbody_reference;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private Transform m_FlippedCheck;

    [SerializeField] public PlayerMovement player_movement_script;
    // image references
    [SerializeField] Sprite regular;
    [SerializeField] Sprite damage;
    [SerializeField] Transform follow_target;
    const float k_GroundedRadius = .2f;
	
    public bool m_FacingRight = true;
    private const int max_hp = 100;
    public int current_hp;

    private const float max_speed = 4800f;
    private int damage_timer = 0;
    // This is to make sure that each attack only hits once
    private int last_attack_id;

    public const float launchX = 7.5f;
    public const float launchY = 7.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        current_hp = max_hp;
    }

    // Update is called once per frame
    void Update()
    {
        track_player();
        update_facing();
        if (current_hp < 0) {
            Die();
        }
    }

    void FixedUpdate () {
        // check if we are flipped over.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_FlippedCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				FlipOver();
			}
		}

        if (damage_timer > 0) {
            damage_timer--;
        }
        if (damage_timer == 0) {
            sprite_renderer.sprite = regular;
        }
    }

    // stole this from the character controller
    private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    public void take_damage(int damage_amount) {
        // iframes so we don't constantly take damage from the same hitbox.
        if (damage_timer > 0) {
            return;
        }
        // else
        current_hp -= damage_amount;
        sprite_renderer.sprite = damage;
        damage_timer = 12;
        // launch us
        if (transform_reference.position.x < follow_target.position.x) {
            // launch us leftward
            rigidbody_reference.velocity = (new Vector2(-launchX, launchY));
        }
        else if (transform_reference.position.x > follow_target.position.x) {
            // launch us to the right
            rigidbody_reference.velocity = (new Vector2(launchX, launchY));

        }
    }

    private void track_player() {
        // are we to the left of the target?
        if (transform_reference.position.x < follow_target.position.x) {
            // if so, add some rightward force
            //  assuming we aren't already too fast.
            if (Math.Abs(rigidbody_reference.velocity.x) < max_speed) {
                rigidbody_reference.AddForce(new Vector2(500f, 0));
            }
        }
        // are we to the right?
        if (transform_reference.position.x > follow_target.position.x) {
            // if we aren't already too fast, add some leftward force
            if (Math.Abs(rigidbody_reference.velocity.x) < max_speed) {
                rigidbody_reference.AddForce(new Vector2(-500f, 0f));
            }
        }
    }

    private void update_facing () {
        // stole this logic from the character controller
        if (rigidbody_reference.velocity.x > 1 && !m_FacingRight) {
            Flip();
        }
        else if (rigidbody_reference.velocity.x < -1 && m_FacingRight) {
            Flip();
        }
    }

    private void FlipOver () {
        if (160 <= rigidbody_reference.rotation && rigidbody_reference.rotation <= 200) {
            rigidbody_reference.AddForce(new Vector2(0f, 5000f));
            rigidbody_reference.rotation = 0f;
        }
    }
    
    private void Die () {
        // eliminate the object
        Destroy(gameObject);
        this.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            int launch_direction = 1;
            // if we are to the left, launch player to the right
            if (transform_reference.position.x < follow_target.position.x) {
                launch_direction = 1;
            }
            if (transform_reference.position.x > follow_target.position.x) {
                launch_direction = -1;
            }
            player_movement_script.take_damage(launch_direction);
        }
        
    }
}
