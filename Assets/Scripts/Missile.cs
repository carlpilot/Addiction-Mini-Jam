using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Missile : MonoBehaviour {

    public Transform target;
    public float blastRadius = 1.0f;
    public int maxDamage = 3;
    public int directHitBonus = 1;
    public float speed = 1.0f;
    public float turnRateDPS = 30.0f;
    bool live = true;

    public Light2D light2d;
    public ParticleSystem explosionParticles;

    private void Start () {
        
    }

    private void FixedUpdate () {
        if (!live) return;

        GetComponent<Rigidbody2D> ().velocity = transform.up * speed;
        float worldAngleToTarget = Mathf.Atan2 (target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;
        
        float turnAngle = transform.rotation.z - worldAngleToTarget - 90.0f;
        while (turnAngle > 180.0f) turnAngle -= 360.0f;
        while (turnAngle < -180.0f) turnAngle += 360.0f;
        transform.Rotate (Vector3.forward * Mathf.Clamp (turnAngle, -turnRateDPS * Time.deltaTime, turnRateDPS * Time.deltaTime));
        
        //transform.rotation = Quaternion.Euler (Vector3.forward * (worldAngleToTarget-90.0f));
    }

    private void OnCollisionEnter2D (Collision2D collision) {
        if (!live) return;
        live = false;

        Explode ();

        if (collision.collider.tag == "Sword") return;

        if (collision.gameObject.tag == "Player") {
            // fireball directly hit the player
            collision.gameObject.GetComponent<PlayerController> ().AddHealth (-(maxDamage + directHitBonus));
        } else {
            // check for indirect hit (player within blast radius)
            foreach (Collider2D col in Physics2D.OverlapCircleAll (transform.position, blastRadius)) {
                if (col.GetComponent<PlayerController> () != null) {
                    float damage = (col.transform.position - transform.position).magnitude / blastRadius * maxDamage;
                    col.GetComponent<PlayerController> ().AddHealth (-Mathf.RoundToInt (damage - 1));
                    break;
                }
            }
        }
    }

    public void MultiplySpeed (float factor) {
        speed *= factor;
    }

    void Explode () {
        // start explosion particles
        explosionParticles.Play ();
        // flash
        light2d.pointLightInnerAngle = 0.0f;
        light2d.pointLightOuterAngle = 360.0f;
        light2d.pointLightOuterRadius = 3.0f;
        InvokeRepeating ("DimLight", 0.15f, 0.04f);
        // hide sprite and disable collisions
        Destroy (GetComponent<Rigidbody2D> ());
        Destroy (GetComponent<Collider2D> ());
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
