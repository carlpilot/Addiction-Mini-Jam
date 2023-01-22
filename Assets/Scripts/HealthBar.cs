using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header ("Image References")]
    public Image[] hearts;
    [Header ("Sprites")]
    public Sprite empty;
    public Sprite red, halfRed;

    public void SetHealth (int health) {
        for(int i = 0; i < hearts.Length; i++) {
            hearts[i].sprite = health >= 2 * (i + 1) ? red : health >= 2 * i + 1 ? halfRed : empty;
        }
    }
}
