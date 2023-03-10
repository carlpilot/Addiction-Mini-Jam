using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUISlideSelect : MonoBehaviour
{
    public RectTransform[] slides;
    public GameObject clickThroughBlocker;

    public Vector3[] cameraPositions;

    public float slideDist = 1920f;
    public float speed = 1f;

    public AnimationCurve speedMultiplierByPosition;
    public AnimationCurve cameraTransitionCurve;

    int originSlide = 0;

    public int CurrentSlide { get; private set; } = 0;

    public void Transition (int slide) {
        originSlide = CurrentSlide;
        CurrentSlide = slide;
        StartCoroutine ("transitionSlide");
    }

    IEnumerator transitionSlide () {
        clickThroughBlocker.SetActive (true);
        float distTravelled = 0f;
        bool right = (CurrentSlide != 0); // sliding direction: True = right, false = left

        // make right slides active
        for (int i = 0; i < slides.Length; i++) {
            slides[i].gameObject.SetActive (i == 0 || i == CurrentSlide || i == originSlide);
        }

        // set up start positions
        Vector2[] startPositions = new Vector2[slides.Length];
        for(int i = 0; i < slides.Length; i++) {
            startPositions[i] = slides[i].anchoredPosition;
        }

        Debug.Log ("Transitioning");

        // animate
        while (distTravelled < slideDist) {
            float d = (right ? -1f : 1f) * speed / Time.fixedDeltaTime * speedMultiplierByPosition.Evaluate (distTravelled / slideDist);
            foreach (RectTransform r in slides) {
                r.anchoredPosition += Vector2.right * d;
            }
            distTravelled += Mathf.Abs (d);
            yield return new WaitForEndOfFrame ();

            Camera.main.transform.position = Vector3.Lerp (cameraPositions[originSlide], cameraPositions[CurrentSlide], cameraTransitionCurve.Evaluate (distTravelled / slideDist));
        }

        // lock final positions (eliminates frame gaps)
        for (int i = 0; i < slides.Length; i++) {
            slides[i].anchoredPosition = startPositions[i] + Vector2.right * (right ? -1f : 1f) * slideDist;
        }

        clickThroughBlocker.SetActive (false);
    }
}
