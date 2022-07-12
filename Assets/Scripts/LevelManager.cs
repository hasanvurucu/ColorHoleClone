using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    void Awake()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            LoadLevel();
        }
    }

    public void IncreaseLevel()
    {
        int currentLevel = PlayerPrefs.GetInt(Tags.currentLevelTag);
        currentLevel++;
        PlayerPrefs.SetInt(Tags.currentLevelTag, currentLevel);

        LoadLevel();
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene((PlayerPrefs.GetInt(Tags.currentLevelTag) % (SceneManager.sceneCountInBuildSettings - 1)) + 1);
    }
}
