using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneMenu : MonoBehaviour
{
    [MenuItem("Scenes/Main menu")]
    static void MainMenu() => LoadScene("Assets/Menu/main menu.unity");

    [MenuItem("Scenes/Track test")]
    static void TrackTest() => LoadScene("Assets/Maps/test track/test track scene.unity");

    private static void LoadScene(string scenePath)
	{
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene(scenePath);
    }
}
