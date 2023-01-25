using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VersionChecker : MonoBehaviour {

    public static string path = "https://raw.githubusercontent.com/carlpilot/Addiction-Mini-Jam/main/version.txt";
    public static string changelogLink = "https://github.com/carlpilot/Addiction-Mini-Jam/blob/main/changelog.md";

    public static string CurrentVersion;

    public GameObject newVersionNotification;
    public TMP_Text newVersionMessage;
    public TMP_Text versionDisplay;
    public TMP_Text staticVersionDisplay;

    private void Awake () {
        CurrentVersion = Application.version;
        staticVersionDisplay.text = "Version v" + CurrentVersion;
    }

    private void Start () {
        print ("Version checker active for version " + CurrentVersion);
        StartCoroutine (WwwRequestVersion ());
    }

    IEnumerator WwwRequestVersion () {
        WWW www = new WWW (path);

        yield return www; // wait until results

        string[] lines = www.text.Split (new char[] { '\n' }, 3);

        if (lines[0] != CurrentVersion) {
            Debug.Log ("Update needed: version available: (" + lines[0] + ") vs current version: (" + CurrentVersion + ")");
            newVersionNotification.SetActive (true);
            newVersionMessage.text = lines[1];
            versionDisplay.text = string.Format ("Currently: v{0}\nAvailable:  v{1}", CurrentVersion, lines[0]);
        } else {
            Debug.Log ("Up to date");
        }
    }

    public void OpenDownloadPage () {
        Application.OpenURL ("https://carlpilot.itch.io/addicted");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit (); 
        #endif
    }

    public void OpenChangelog () {
        Application.OpenURL (changelogLink);
    }
}
