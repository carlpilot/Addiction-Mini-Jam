using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMonster : MonoBehaviour {

    public GameObject fireballPrefab;

    Transform target;

    public float targetDist = 2.0f;
    public float speed = 1.5f;
    public float fireRate = 0.5f;
    public int fireballsPerBurst = 3;
    public float breakBetweenBursts = 3.0f;
    public float shootSpeed = 3.0f;

    public bool inCooldown = true;
    public float cooldown = 0.0f;
    public int numFiredSinceCooldown = 0;
    public float timeSinceLastFire = 0.0f;

    private void Start () {
        target = GameObject.FindGameObjectWithTag ("Player").transform;
    }

    private void Update () {
        float distFromCircle = (target.position - transform.position).magnitude - targetDist;
        GetComponent<Rigidbody2D> ().velocity = (target.position - transform.position).normalized * Mathf.Clamp (speed * distFromCircle, -speed, speed);

        if (inCooldown) cooldown += Time.deltaTime;
        else {
            if (timeSinceLastFire >= fireRate) {
                ShootFireball ();
            }
            if (numFiredSinceCooldown >= fireballsPerBurst) {
                inCooldown = true;
                numFiredSinceCooldown = 0;
            }
        }

        if(cooldown > breakBetweenBursts) {
            cooldown = 0.0f;
            inCooldown = false;
        }

        timeSinceLastFire += Time.deltaTime;
    }

    void ShootFireball () {
        Debug.Log ("Pew");

        float dist = Vector2.Distance (transform.position, target.position);
        float flightTime = dist / shootSpeed;

        Vector3 leadPos = Vector3.Lerp (target.position, target.position + (Vector3) target.GetComponent<Rigidbody2D> ().velocity * flightTime, 1 / dist);

        Vector3 dir = (leadPos + (Vector3) Random.insideUnitCircle * 0.5f - transform.position).normalized;

        RaycastHit2D h = Physics2D.Raycast (transform.position, target.position - transform.position, 2.0f);
        Debug.Log (h.collider);
        if (h.collider != null && h.collider.GetComponent<PlayerController> () == null) return; // don't shoot at inanimate objects

        GameObject f = Instantiate (fireballPrefab, transform.position + dir, Quaternion.identity);
        f.GetComponent<Fireball> ().Propel (dir * shootSpeed);

        timeSinceLastFire = 0;
        numFiredSinceCooldown++;
    }
}
