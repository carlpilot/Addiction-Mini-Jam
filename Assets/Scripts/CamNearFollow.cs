using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamNearFollow : MonoBehaviour
{
    public Transform target;

    public float distMultiplier = 1.0f;
    float minDist = 1.0f;
    
    public float springStrength = 1.0f;

    private void Start () {
        minDist = Camera.main.orthographicSize / 2f;
    }

    private void FixedUpdate () {
        float dist = Vector2.Distance (transform.position, target.position);
        if (dist > minDist * distMultiplier) {
            Vector2 translation = ((Vector2) target.position - (Vector2) transform.position).normalized * (dist - minDist * distMultiplier) * springStrength;
            if (translation.magnitude > 0.002f) transform.Translate (translation); // eliminate pixel weirdness from small movements
        }
    }

    /*
    private void OnDrawGizmos () {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere ((Vector2) transform.position, minDist * distMultiplier);
        Gizmos.DrawLine ((Vector2) transform.position, target.position);
    }
    */
}
