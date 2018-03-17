using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour {
    
    public void LoadGameScene()
    {
        LoadScene(1);
    }

    public void LoadMainMenuScene()
    {
        LoadScene(0);
    }

    // TODO Load async if needed.

    private void LoadScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
}
