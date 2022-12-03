using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

// get reference to our sprite renderer
    [SerializeField] public SpriteRenderer sprite_renderer;

    // there's probably a better way to assign these images,
    //  but I don't know what it is.
    [SerializeField] public Sprite idle;
    [SerializeField] public Sprite crouch;
    [SerializeField] public Sprite walk_0;
    [SerializeField] public Sprite walk_1;
    [SerializeField] public Sprite jump_up;
    [SerializeField] public Sprite jump_down;
    [SerializeField] public Sprite jump_land;
    [SerializeField] public Sprite dash_active;
    [SerializeField] public Sprite dash_cooldown;
    [SerializeField] public Sprite attack1_windup;
    [SerializeField] public Sprite attack1_active;
    [SerializeField] public Sprite attack2_windup;
    [SerializeField] public Sprite attack2_active;
    [SerializeField] public Sprite attack3_windup;
    [SerializeField] public Sprite attack3_active;
    [SerializeField] public Sprite attack4_windup;
    [SerializeField] public Sprite attack4_active;
    [SerializeField] public Sprite damage;

    // variables we'll need
    public int walking_timer = 0;
    int walking_duration = 15;
    float movement_deadzone = 0.5f;
    bool grounded_last_run = true;
    int land_timer = 0;
    int fall_time = 0;

    public int attack_timer = 0;
    const int attack_windup_duration = 15;
    const int attack_duration = 20;

    public int attack_index = 0;

    Sprite[] windup_array;
    Sprite[] active_array;

    // image arrays
    // Start is called before the first frame update
    void Start()
    {
        windup_array = new Sprite[] {attack1_windup, attack2_windup, attack3_windup, attack4_windup};
        active_array = new Sprite[] {attack1_active, attack2_active, attack3_active, attack4_active};
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void update_image (bool should_crouch, bool should_jump, Vector2 player_speed, bool grounded, int dash_timer, bool damaged) {
        if (damaged) {
            sprite_renderer.sprite = damage;
        }
        else if (should_crouch) {
            sprite_renderer.sprite = crouch;
            return;
        }
        else if (dash_timer > 0) {
            sprite_renderer.sprite = dash_active;
        }
        else if (dash_timer > -10) {
            sprite_renderer.sprite = dash_cooldown;
        }
        else if (!grounded) {
            if (player_speed.y > -0.5) {
                sprite_renderer.sprite = jump_up;
            }
            else if (player_speed.y < -0.5) {
                sprite_renderer.sprite = jump_down;
                fall_time++;
            }
        }
        else if ((grounded && !grounded_last_run) || land_timer > 0) {
            if (land_timer == 0) {
                sprite_renderer.sprite = jump_land;
                land_timer = fall_time / 2;
                fall_time = 0;
            }
            if (land_timer > 0) {
                land_timer--;
                sprite_renderer.sprite = jump_land;
            }
            
        }
        else if (Math.Abs(player_speed.x) > movement_deadzone) {
            walking_timer++;
            if (walking_timer > 2*walking_duration) {
                walking_timer = 0;
            }
            if (walking_timer <= walking_duration) {
                sprite_renderer.sprite = walk_0;
            }
            if (walking_timer > walking_duration) {
                sprite_renderer.sprite = walk_1;
            }            
        }
        
        // ...
        else {
            sprite_renderer.sprite = idle;
        }
        grounded_last_run = grounded; // remember grounded state for next run
    }

    public int update_combat_image(bool attack, int attack_timer) {
        if ((attack_windup_duration >= attack_timer)
            && (attack_timer > 0)) {
            attack_timer++;
            sprite_renderer.sprite = windup_array[attack_index];
        }
        if (attack_timer > attack_windup_duration) {
            attack_timer++;
            sprite_renderer.sprite = active_array[attack_index];
        }
        if (attack_timer >= (attack_windup_duration + attack_duration)) {
            // reset
            attack_timer = 0;
            // increment image for next attack
            attack_index += 1;
            if (attack_index > 3) {
                attack_index = 0;
            }
        }
        return attack_timer;
    }
}
