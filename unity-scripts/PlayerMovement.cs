using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    [SerializeField] public PlayerAnimation animation_handler;

    // <combat>
    [SerializeField] public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attack_timer = 0;
    // I know this is miserable, but I don't know a better way to do it.
    //  IMPORTANT: Keep this synced up with windup_duration in PlayerAnimation.cs
    public const int attack_windup_duration = 15;
    public const int attack_duration = 20;
    public int attack_damage = 10;
    public int attack_id;
    public int hp = 3;
    [SerializeField] public Text hpDisplay;
    // </combat>

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;
    bool stop_jump = false;
    bool crouch = false;
    bool dash = false;
    bool attack = false;

    bool damaged = false;
    int damage_timer = 0;
    const int iframe_duration = 17;
    const float launchX = 15f;
    const float launchY = 15f;

    // Update is called once per frame
    void Update()
    {
        // if we are damaged right now, ignore all of this stuff. 
        if (damaged) {
            return;
        }
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            stop_jump = true;
        }
        if (Input.GetButtonDown("Crouch")) 
        {
            crouch = true;
        } else if (Input.GetButtonUp("Crouch")) {
            crouch = false;
        }

        if (Input.GetButtonDown("Dash")) {
            dash = true;
        }

        // check for attack
        if (Input.GetMouseButtonDown(0)) {
            begin_attack();
        }
    }

    void FixedUpdate () {
        // only move if we aren't in the middle of attacking
        if (attack_timer > 0) {
            // attack_timer is set to 0 when the attack is over.
            jump = false;
            dash = false;
            attack = false;
            horizontalMove = 0f;
            // you have to un-crouch to attack.
            crouch = false;
        }
        
        handle_damage_timer();
        // put our HP to the screen
        hpDisplay.text = hp.ToString();
        // attack (maybe)
        commit_attack();
        // move the character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump, stop_jump, dash, damaged);
        animation_handler.update_image(crouch, jump, controller.m_Rigidbody2D.velocity, controller.m_Grounded, controller.dash_timer, damaged);
        attack_timer = animation_handler.update_combat_image(attack, attack_timer);
        // attack images have priority here, so they are called last.
        //  that way, we don't undo them with the idle or some other image.
        jump = false;
        stop_jump = false;
        dash = false;
        attack = false;
        
    }

    // player combat
    void begin_attack () {
        // set these up for the animation handler
        attack = true;
        attack_timer = 1;
    }

    void commit_attack () {
        // we need to delay the hit until we have the active image
        if ((attack_duration > attack_timer)
         && (attack_timer > attack_windup_duration)) {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach(Collider2D enemy in hitEnemies) {
                enemy.GetComponent<EnemyController>().take_damage(attack_damage);
                attack_id++;
            }
        }
    }

    public void take_damage (int direction) {
        // direction should be either -1 or 1
        //  -1 for left and 1 for right
        if (!damaged) {
            hp -= 1;
            damaged = true;
            damage_timer = 1;
            controller.m_Rigidbody2D.velocity = (new Vector2(launchX*direction, launchY));
        }

    }

    void handle_damage_timer () {
        if (damage_timer > 0) {
            damage_timer++;
            damaged = true;
        }
        // not else
        if (damage_timer > iframe_duration) {
            damage_timer = 0;
            damaged = false;
        }
    }

    void OnDrawGizmosSelected() {
        if (attackPoint == null) {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
