using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour {

    public GameObject weapon;

    public float swingAngle = 30.0f;
    public float swingTime = 0.5f;
    float angleOffset = 0f;

    public int hitDamage = 3;

    [HideInInspector]
    public bool isSwinging = false;
    [HideInInspector]
    public bool hasStruckThisSwing = false;

    private void Update () {
        weapon.transform.eulerAngles = Vector3.forward * (GetWorldAngle (Input.mousePosition, transform.position) + angleOffset);

        if (Input.GetMouseButtonDown (0) && !EventSystem.current.IsPointerOverGameObject ()) {
            StopAllCoroutines ();
            StartCoroutine (Swing ());
        }
    }

    IEnumerator Swing () {
        isSwinging = true;
        hasStruckThisSwing = false;
        angleOffset = 0f;
        for (float t = 0; t < swingTime; t += Time.deltaTime) {
            angleOffset = -swingAngle / 2f + swingAngle * swingFunction (t / swingTime);
            yield return new WaitForEndOfFrame ();
        }
        angleOffset = 0f;
        isSwinging = false;
    }

    float swingFunction(float t01) {
        return 1.0f / (1.0f + Mathf.Exp (10.0f - 20.0f * t01));
    }

    public float GetWorldAngle (Vector2 mousePos, Vector2 playerPos) {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint (mousePos);
        Vector2 dirVector = mouseWorldPos - playerPos;
        return Mathf.Atan2 (dirVector.y, dirVector.x) * Mathf.Rad2Deg;
    }
}
