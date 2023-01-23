using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    GUISlideSelect gss;

    public GameObject mouseTarget;

    [Header ("Level Menu")]
    public Transform levelGridParent;
    public GameObject levelIconPrefab;
    public Sprite[] levelMaps;

    private void Start () {
        Time.timeScale = 1;
        gss = FindObjectOfType<GUISlideSelect> ();
        if (!PlayerPrefs.HasKey ("LastLevelCompleted")) PlayerPrefs.SetInt ("LastLevelCompleted", 0);

        for(int i = 0; i < levelMaps.Length; i++) {
            GameObject newLevelIcon = Instantiate (levelIconPrefab, levelGridParent);
            newLevelIcon.GetComponent<LevelIcon> ().Setup (i + 1, levelMaps[i]);
        }
    }

    private void Update () {
        mouseTarget.transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
    }

    public void SlideTransition (int slide) {
        gss.Transition (slide);
    }

    public void LoadScene (int scene) {
        SceneManager.LoadScene (scene);
    }

    public void Quit () {
        Application.Quit ();
    }

    public void MoreGames () {
        Application.OpenURL ("https://carlpilot.itch.io/");
    }
}
