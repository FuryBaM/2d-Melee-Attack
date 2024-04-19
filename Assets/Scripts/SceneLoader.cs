using UnityEditor;
using UnityEngine;

public class SceneLoader: MonoBehaviour
{
    public void LoadGame()
    {
        Application.LoadLevel("GameScene");
    }
    public void LoadMenu()
    {
        Application.LoadLevel("MenuScene");
    }
    public void Exit()
    {
        Application.Quit();
    }
}
