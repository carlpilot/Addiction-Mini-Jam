using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Sprite base1, base2;
    bool sprite1 = true;
    public float walkSpriteCycleSpeed = 0.5f;
    float walkTimer = 0.0f;

    public float walkSpeed = 1.0f;
    public float walkInertia = 0.4f;

    private void Start () {
        
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
    }
}
