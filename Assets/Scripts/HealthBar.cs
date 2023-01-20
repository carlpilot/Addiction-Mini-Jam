using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header ("Image References")]
    public Image RH1;
    public Image RH2, RH3;
    [Header ("Sprites")]
    public Sprite empty;
    public Sprite red, halfRed;

    public void SetHealth (int healthR) {
        RH1.sprite = healthR > 1 ? red : healthR > 0 ? halfRed : empty;
        RH2.sprite = healthR > 3 ? red : healthR > 2 ? halfRed : empty;
        RH3.sprite = healthR > 5 ? red : healthR > 4 ? halfRed : empty;
    }
}
