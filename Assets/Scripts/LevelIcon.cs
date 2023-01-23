using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelIcon : MonoBehaviour
{

    MainMenu mm;

    public TMP_Text title;
    public Image levelMap;
    public GameObject blocker;
    public GameObject tick;
    int level = 0;

    public void Setup (int levelNumber, Sprite map) {
        mm = FindObjectOfType<MainMenu> ();
        level = levelNumber;
        title.text = "Level " + level;
        levelMap.sprite = map;
        int lastLevelCompleted = PlayerPrefs.GetInt ("LastLevelCompleted");
        blocker.SetActive (level > lastLevelCompleted + 1);
        tick.SetActive (level <= lastLevelCompleted);
    }

    public void GoToLevel () {
        mm.LoadScene (level);
    }
}
