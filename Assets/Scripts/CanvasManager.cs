using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private Image startPanel;
    public Image failedPanel;
    public Image finishedPanel;

    [SerializeField] private LevelManager levelManager;

    [SerializeField] private Text[] levelInfoTexts;
    private void Start()
    {
        startPanel.gameObject.SetActive(true);
        failedPanel.gameObject.SetActive(false);
        finishedPanel.gameObject.SetActive(false);

        levelInfoTexts[0].text = (PlayerPrefs.GetInt(Tags.currentLevelTag) + 1).ToString();
        levelInfoTexts[1].text = (PlayerPrefs.GetInt(Tags.currentLevelTag) + 2).ToString();
    }

    public void StartGame() //EventTrigger
    {
        startPanel.gameObject.SetActive(false);
    }

    public void RestartLevel() //Button
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void NextLevel() //Button
    {
        levelManager.IncreaseLevel();
    }

}
