using UnityEngine;
using UnityEditor;

public static class PlayerPrefsResetter
{
    [MenuItem("Tools/Reset PlayerPrefs")]
    public static void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("reset");
    }
}