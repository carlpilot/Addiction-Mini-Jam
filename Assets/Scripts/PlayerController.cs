using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public enum HarmType {
        Drug, Damage
    }

    public HarmType lastHarm = HarmType.Damage;

    public Sprite base1, base2;
    bool sprite1 = true;
    public float walkSpriteCycleSpeed = 0.5f;
    float walkTimer = 0.0f;

    public float walkSpeed = 1.0f;
    public float walkInertia = 0.4f;

    HealthBar healthBar;
    int health;

    private void Start () {
        healthBar = FindObjectOfType<HealthBar> ();
        health = maxHealth;
    }

    private void Update () {
        GetComponent<Rigidbody2D>().velocity = (Vector3.up * Input.GetAxis ("Vertical") * walkSpeed + Vector3.right * Input.GetAxis ("Horizontal") * walkSpeed);

        if(Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f) {
            walkTimer += Time.deltaTime;
            if (walkTimer > walkSpriteCycleSpeed) {
                sprite1 = !sprite1;
                GetComponent<SpriteRenderer> ().sprite = sprite1 ? base1 : base2;
                walkTimer = 0;
            }
        } else walkTimer = 0;

        if (Time.time % 5 < Time.deltaTime) AddHealth (1);
    }

    public void SetFullHealth () { SetHealth (maxHealth); }
    public void SetHealth (int newHealth, HarmType harm) {
        health = Mathf.Clamp(newHealth, 0, maxHealth);
        healthBar.SetHealth (newHealth);
        lastHarm = harm;
    }
    public void SetHealth (int newHealth) => SetHealth (newHealth, HarmType.Damage);
    public void AddHealth (int inc) => SetHealth (health + inc);
    public void AddHealth (int inc, HarmType harm) => SetHealth (Health + inc, harm);
    public int Health { get => health; }
    public int maxHealth { get => healthBar.hearts.Length * 2; }
}
