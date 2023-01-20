using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    PlayerController pc;
    HealthBar hb;

    private void Start () {
        pc = FindObjectOfType<PlayerController> ();
        hb = FindObjectOfType<HealthBar> ();
    }

    private void Update () {
        if (pc.Health <= 0) Lose ();
    }

    public void Win () {

    }

    public void Lose () {

    }
}
