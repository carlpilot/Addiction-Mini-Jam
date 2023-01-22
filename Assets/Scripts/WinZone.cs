using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour {

    GameManager gm;

    private void Start () {
        gm = FindObjectOfType<GameManager> ();
    }

    private void OnTriggerStay2D (Collider2D collision) {
        if (gm.Winnable && collision.tag == "Player") gm.Win ();
    }
}
