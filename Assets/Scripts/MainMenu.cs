using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    GUISlideSelect gss;

    public GameObject mouseTarget;

    private void Start () {
        Time.timeScale = 1;
        gss = FindObjectOfType<GUISlideSelect> ();
    }

    private void Update () {
        mouseTarget.transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
    }

    public void SlideTransition (int slide) {
        gss.Transition (slide);
    }
}
