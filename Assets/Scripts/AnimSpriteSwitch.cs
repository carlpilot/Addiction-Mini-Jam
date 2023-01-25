using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimSpriteSwitch : MonoBehaviour
{

    public Sprite[] sprites;
    int spriteIdx = 0;
    public float animDuration;

    float counter = 0.0f;

    private void Update () {
        if (counter > animDuration / sprites.Length) {
            spriteIdx++;
            if (spriteIdx >= sprites.Length) spriteIdx = 0;
            GetComponent<SpriteRenderer> ().sprite = sprites[spriteIdx];
            counter = 0;
        }
        counter += Time.deltaTime;
    }
}
