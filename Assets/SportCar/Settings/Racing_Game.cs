#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class Startup
{
    static Startup()    
    {
        EditorPrefs.SetInt("showCounts_sportcarcgbr10", EditorPrefs.GetInt("showCounts_sportcarcgbr10") + 1);

        if (EditorPrefs.GetInt("showCounts_sportcarcgbr10") == 1)       
        {
            Application.OpenURL("https://assetstore.unity.com/packages/slug/361944");
            // System.IO.File.Delete("Assets/SportCar/Racing_Game.cs");
        }
    }     
}
#endif
