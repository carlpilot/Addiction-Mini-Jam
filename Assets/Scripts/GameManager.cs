using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {

    PlayerController pc;
    PlayerAttack pa;
    HealthBar hb;

    float gameTimer = 0.0f;

    [Header ("Menus")]
    public GameObject winMenu;
    public TMP_Text winText;
    public GameObject nextLevelButton;
    public GameObject loseMenu;
    public TMP_Text loseTitle;
    public GameObject pauseMenu;
    public GameObject pauseBlocker;

    [Header ("Status Effects")]
    public float speedDuration = 15f;
    public float strengthDuration = 30f;
    public float nightVisDuration = 20f;
    public float speedMultiplier = 2.0f;
    public int strengthBoost = 3;

    [Header ("Icons")]
    public GameObject speedIcon, strengthIcon, nightVisIcon;
    public Image speedFill, strengthFill, nightVisFill;

    [Header ("World Objects")]
    public GameObject nightLight;
    public TMP_Text monsterKillText;

    [Header ("Level Specific")]
    public int minMonstersToKill;
    int numMonstersKilled = 0;
    public int monsterCap = 20;

    [HideInInspector]
    bool gameWinnable = false;
    public bool Winnable { get => gameWinnable; }

    public int numMonstersCurrentlyInScene { get; private set; }

    private void Start () {
        Unpause ();
        pc = FindObjectOfType<PlayerController> ();
        pa = FindObjectOfType<PlayerAttack> ();
        hb = FindObjectOfType<HealthBar> ();
        UpdateWinMonsterText ();
    }

    private void Update () {

        numMonstersCurrentlyInScene = FindObjectsOfType<FireMonster> ().Length;

        if (pc.Health <= 0) Lose ();

        if (Input.GetKeyDown (KeyCode.Escape) && !loseMenu.activeInHierarchy && !winMenu.activeInHierarchy) {
            if (!pauseMenu.activeInHierarchy) Pause ();
            else Unpause ();
        }

        gameWinnable = numMonstersKilled >= minMonstersToKill && !loseMenu.activeInHierarchy && !winMenu.activeInHierarchy && !pauseMenu.activeInHierarchy;

        gameTimer += Time.deltaTime;
    }

    public void Win () {
        Time.timeScale = 0;
        pauseBlocker.SetActive (true);
        if (level == SceneManager.sceneCountInBuildSettings - 1) nextLevelButton.SetActive (false);
        if (PlayerPrefs.GetInt ("LastLevelCompleted") < level) PlayerPrefs.SetInt ("LastLevelCompleted", level);
        float best;
        string bestKeyName = "Level" + level + "Best";
        if (PlayerPrefs.HasKey(bestKeyName)) {
            // not the first play of this level
            best = PlayerPrefs.GetFloat (bestKeyName);
            if (gameTimer < best) {
                // new best
                best = gameTimer;
                PlayerPrefs.SetFloat (bestKeyName, gameTimer);
            }
        } else {
            // first play of this level
            best = gameTimer;
            PlayerPrefs.SetFloat (bestKeyName, best);
        }
        winText.text = "LEVEL COMPLETE!\nTime:\t" + gameTimer.ToString ("F3") + " s\nBest:\t" + best.ToString ("F3") + " s";
        winMenu.SetActive (true);
    }

    public void Lose () {
        Time.timeScale = 0;
        pauseBlocker.SetActive (true);
        loseMenu.SetActive (true);
        if (pc.lastHarm == PlayerController.HarmType.Drug) loseTitle.text = "OVERDOSE"; // differentiate enemy vs overdose deaths
    }

    public void Pause () {
        Time.timeScale = 0;
        pauseMenu.SetActive (true);
        pauseBlocker.SetActive (true);
    }

    public void Unpause () {
        Time.timeScale = 1;
        pauseMenu.SetActive (false);
        pauseBlocker.SetActive (false);
    }

    public void SusSnack () {
        Strength ();
        pc.AddHealth (-4, PlayerController.HarmType.Drug);
    }

    public void Perplex () {
        Slowdown ();
        pc.AddHealth (-6, PlayerController.HarmType.Drug);
    }

    public void CrunchCrystals () {
        Strength ();
        Slowdown ();
        NightVision ();
        pc.AddHealth (-8, PlayerController.HarmType.Drug);
    }

    public void Slowdown () {
        speedIcon.SetActive (true);
        MultiplyWorldSpeed (1f / speedMultiplier);
        StartCoroutine (ExpireSpeed ());
    }

    IEnumerator ExpireSpeed () {
        for(float t = 0; t < speedDuration; t += Time.deltaTime) {
            yield return new WaitForEndOfFrame ();
            speedFill.fillAmount = (1.0f - t / speedDuration);
        }
        MultiplyWorldSpeed (speedMultiplier);
        speedIcon.SetActive (false);
    }

    public void Strength () {
        strengthIcon.SetActive (true);
        pa.hitDamage += strengthBoost;
        StartCoroutine (ExpireStrength ());
    }

    IEnumerator ExpireStrength () {
        for (float t = 0; t < strengthDuration; t += Time.deltaTime) {
            yield return new WaitForEndOfFrame ();
            strengthFill.fillAmount = (1.0f - t / strengthDuration);
        }
        pa.hitDamage -= strengthBoost;
        strengthIcon.SetActive (false);
    }

    public void NightVision() {
        nightLight.SetActive (true);
        nightVisIcon.SetActive (true);
        StartCoroutine (ExpireNightVision ());
    }

    IEnumerator ExpireNightVision () {
        for (float t = 0; t < nightVisDuration; t += Time.deltaTime) {
            yield return new WaitForEndOfFrame ();
            nightVisFill.fillAmount = (1.0f - t / nightVisDuration);
        }
        nightLight.SetActive (false);
        nightVisIcon.SetActive (false);
    }

    void MultiplyWorldSpeed (float factor) {
        foreach(FireMonster f in FindObjectsOfType<FireMonster>()) {
            f.MultiplySpeed (factor);
        }
        foreach(Fireball f in FindObjectsOfType<Fireball>()) {
            f.MultiplySpeed (factor);
        }
    }

    public void ReportMonsterDeath () {
        numMonstersKilled++;
        UpdateWinMonsterText ();
    }

    void UpdateWinMonsterText () {
        int numRemaining = minMonstersToKill - numMonstersKilled;
        monsterKillText.text = numRemaining > 0 ? ("Kill " + numRemaining + " more " + (numRemaining != 1 ? "monsters" : "monster")) : "";
    }

    public int level { get => SceneManager.GetActiveScene ().buildIndex; }

    public void NextLevel () => LoadScene (level + 1);
    public void ToMainMenu () => LoadScene (0);
    public void ReloadLevel () => LoadScene (level);

    void LoadScene (int scene) {
        SceneManager.LoadScene (scene);
    }
}
