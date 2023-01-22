using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMonster : MonoBehaviour {

    public GameObject fireballPrefab;

    Transform target;

    public int health = 6;

    public float targetDist = 2.0f;
    public float maxShootDist = 3.5f;
    public float speed = 1.5f;
    public float fireRate = 0.5f;
    public int fireballsPerBurst = 3;
    public float breakBetweenBursts = 3.0f;
    public float shootSpeed = 3.0f;
    public float repulsion = 3.0f;

    bool inCooldown = true;
    float cooldown = 0.0f;
    int numFiredSinceCooldown = 0;
    float timeSinceLastFire = 0.0f;

    float distToPlayer = float.MaxValue; // initialise, then updated in Update

    public float speedMultiplier = 1;

    PlayerAttack pa;
    GameManager gm;

    private void Start () {
        target = GameObject.FindGameObjectWithTag ("Player").transform;
        pa = FindObjectOfType<PlayerAttack> ();
        gm = FindObjectOfType<GameManager> ();
        breakBetweenBursts *= Random.Range (0.9f, 1.1f);
        fireRate *= Random.Range (0.9f, 1.1f);
        cooldown = Random.Range (0f, breakBetweenBursts);
    }

    private void Update () {

        if (health <= 0) Die ();

        distToPlayer = (target.position - transform.position).magnitude;
        float distFromCircle = distToPlayer - targetDist;
        // attract to player circle
        Vector3 dir = target.position - transform.position;
        
        // repel from other monsters
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Monster")) {
            if (g == this.gameObject) continue; // don't repel from self
            dir -= (g.transform.position - transform.position) / (g.transform.position - transform.position).sqrMagnitude * repulsion;
        }
        
        GetComponent<Rigidbody2D> ().velocity = (dir).normalized * Mathf.Clamp (speed * speedMultiplier * distFromCircle * distFromCircle, -speed * speedMultiplier, speed * speedMultiplier);

        if (inCooldown) cooldown += Time.deltaTime;
        else {
            if (timeSinceLastFire >= fireRate / speedMultiplier && shootCondition) {
                ShootFireball ();
            }
            if (numFiredSinceCooldown >= fireballsPerBurst) {
                inCooldown = true;
                numFiredSinceCooldown = 0;
            }
        }

        if(cooldown > breakBetweenBursts / speedMultiplier) {
            cooldown = 0.0f;
            inCooldown = false;
        }

        if (shootCondition) timeSinceLastFire += Time.deltaTime;
    }

    public void Die () {
        gm.ReportMonsterDeath ();
        Destroy (this.gameObject);
    }

    void ShootFireball () {
        float dist = Vector2.Distance (transform.position, target.position);
        float flightTime = dist / shootSpeed;

        Vector3 leadPos = Vector3.Lerp (target.position, target.position + (Vector3) target.GetComponent<Rigidbody2D> ().velocity * flightTime, 1 / dist);

        Vector3 dir = (leadPos + (Vector3) Random.insideUnitCircle * 0.5f - transform.position).normalized;

        RaycastHit2D h = Physics2D.Raycast (transform.position, target.position - transform.position, 2.0f);
        if (h.collider != null && h.collider.GetComponent<PlayerController> () == null) return; // don't shoot at inanimate objects

        GameObject f = Instantiate (fireballPrefab, transform.position + dir, Quaternion.identity);
        f.GetComponent<Fireball> ().Propel (dir * shootSpeed * speedMultiplier);

        timeSinceLastFire = 0;
        numFiredSinceCooldown++;
    }

    public bool shootCondition {
        get {
            Vector2 sp = Camera.main.WorldToScreenPoint (transform.position);
            return distToPlayer <= maxShootDist && sp.x >= 0 && sp.y >= 0 && sp.x < Screen.width && sp.y < Screen.height;
        }
    }

    private void OnCollisionEnter2D (Collision2D collision) {
        if(collision.collider.tag == "Sword" && pa.isSwinging && !pa.hasStruckThisSwing) {
            health -= pa.hitDamage;
            pa.hasStruckThisSwing = true;
        }
    }

    public void MultiplySpeed (float factor) { speedMultiplier *= factor; }
}
