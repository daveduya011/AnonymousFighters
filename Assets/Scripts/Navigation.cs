using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    public void GoToScene(int index) {
        Time.timeScale = 1;
        SceneManager.LoadScene(index);
    }
    public void GoToScene(string sceneName) {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    public void Exit() {
        Application.Quit();
    }
}
