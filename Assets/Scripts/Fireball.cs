using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Fireball : MonoBehaviour {

    public float blastRadius = 1.0f;
    public int maxDamage = 3;
    public int directHitBonus = 1;
    bool live = true;

    public Light2D light2d;
    public ParticleSystem particles;
    public ParticleSystem explosionParticles;

    private void Update () {
        
    }

    public void Propel (Vector2 velocity) {
        GetComponent<Rigidbody2D> ().velocity = velocity;
    }

    private void OnCollisionEnter2D (Collision2D collision) {
        if (!live) return;
        live = false;

        Explode ();

        if (collision.gameObject.GetComponent<PlayerController> () != null) {
            // fireball directly hit the player
            collision.gameObject.GetComponent<PlayerController> ().AddHealth (-(maxDamage + directHitBonus));
        } else {
            // check for indirect hit (player within blast radius)
            foreach (Collider2D col in Physics2D.OverlapCircleAll (transform.position, blastRadius)) {
                if (col.GetComponent<PlayerController> () != null) {
                    float damage = (col.transform.position - transform.position).magnitude / blastRadius * maxDamage;
                    col.GetComponent<PlayerController> ().AddHealth (-Mathf.RoundToInt (damage));
                    break;
                }
            }
        }
    }

    void Explode () {
        // start explosion particles
        explosionParticles.Play ();
        // flash
        light2d.pointLightOuterRadius = 3.0f;
        InvokeRepeating ("DimLight", 0.15f, 0.04f);
        // hide sprite, disable collisions and remove movement particles
        Destroy (GetComponent<Rigidbody2D> ());
        Destroy (GetComponent<Collider2D> ());
        Destroy (particles);
        Destroy (GetComponent<SpriteRenderer> ());
        // destroy fireball gameobject after delay
        Invoke ("DestroySelf", explosionParticles.main.duration);
    }

    void DimLight () {
        light2d.intensity *= 0.6f;
    }

    void DestroySelf () {
        Destroy (this.gameObject);
    }
}
