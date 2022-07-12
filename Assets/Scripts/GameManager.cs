using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Collection Infos
    public float[] collectionProgress = new float[2];
    private int[] collectedAmount= new int[2];
    private int[] collectableAmount = new int[2];

    [SerializeField] private Transform CollectablesParent;
    [SerializeField] private Transform ObstaclesParent;

    //Collection Progress UIs
    [SerializeField] private Image[] collectionBars;

    [SerializeField] private HoleMovement holeMovement;

    [SerializeField] private CanvasManager canvasManager;

    private void Start()
    {
        for(int i = 0; i < 2; i++)
        {
            collectableAmount[i] = CollectablesParent.GetChild(i).childCount;
        }

        for(int i = 0; i < CollectablesParent.GetChild(1).childCount; i++)
        {
            CollectablesParent.GetChild(1).GetChild(i).GetComponent<Rigidbody>().useGravity = false;
        }

        for(int i = 0; i < ObstaclesParent.GetChild(0).childCount; i++)
        {
            ObstaclesParent.GetChild(0).GetChild(i).GetComponent<Rigidbody>().useGravity = true;
        }

        for (int i = 0; i < ObstaclesParent.GetChild(1).childCount; i++)
        {
            ObstaclesParent.GetChild(1).GetChild(i).GetComponent<Rigidbody>().useGravity = false;
        }
    }

    private void Update()
    {
        TrackCollection();
    }

    private void TrackCollection()
    {
        for (int i = 0; i < 2; i++)
        {
            collectionProgress[i] = ((float)collectedAmount[i]) / ((float)collectableAmount[i]);
            collectionBars[i].fillAmount = collectionProgress[i];
        }
    }

    public void Collect()
    {
        if(collectionProgress[0] < 1)
        {
            collectedAmount[0]++;

            if(collectedAmount[0] == collectableAmount[0])
            {
                holeMovement.MoveToSecondPart();
            }
        }else
        {
            collectedAmount[1]++;

            if(collectedAmount[1] == collectableAmount[1])
            {
                FinishLevel();
            }
        }
    }

    public void EnableSecondPartGravity()
    {
        for (int i = 0; i < CollectablesParent.GetChild(1).childCount; i++)
        {
            CollectablesParent.GetChild(1).GetChild(i).GetComponent<Rigidbody>().useGravity = true;
           
        }

        for (int i = 0; i < ObstaclesParent.GetChild(1).childCount; i++)
        {
            ObstaclesParent.GetChild(1).GetChild(i).GetComponent<Rigidbody>().useGravity = true;
        }
    }

    public void DisableFirstPartObstacleGravity()
    {
        for (int i = 0; i < ObstaclesParent.GetChild(0).childCount; i++)
        {
            ObstaclesParent.GetChild(0).GetChild(i).GetComponent<Rigidbody>().useGravity = false;
            ObstaclesParent.GetChild(0).GetChild(i).GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    public void FailLevel()
    {
        canvasManager.failedPanel.gameObject.SetActive(true);
        Game.isGameOver = true;
    }

    public void FinishLevel()
    {
        canvasManager.finishedPanel.gameObject.SetActive(true);
        Game.isGameOver = true;
    }
}
