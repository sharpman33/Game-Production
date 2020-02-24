using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Guards To Spawn")]
    public GameObject guard1;
    public GameObject guard2;

    void Start()
    {
        Instantiate(guard1, guard1.GetComponent<LineRenderer>().GetPosition(0), Quaternion.identity);
        Instantiate(guard2, guard2.GetComponent<LineRenderer>().GetPosition(0), Quaternion.identity);
    }
}
