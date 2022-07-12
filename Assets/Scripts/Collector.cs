using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == Tags.collectableTag)
        {
            GameManager.Instance.Collect();

            Destroy(other.gameObject);
        }

        if(other.tag == Tags.obstacleTag)
        {
            GameManager.Instance.FailLevel();
        }
    }
}
