using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public bool cornerNode;
    public Directions[] nodeDirections;
    void Update()
    {
        
    }

    Node(Directions[] directions)
    {
        for (int i = 0; i < directions.Length; i++)
        {
            nodeDirections[i] = directions[i]; 
        }
    }
}
