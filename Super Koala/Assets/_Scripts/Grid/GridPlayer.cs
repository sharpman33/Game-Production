using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlayer : MonoBehaviour
{
    private Rigidbody2D rb;
    public Vector2 currentNode;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("node"))
        {
            currentNode = new Vector2(other.GetComponent<GridNode>().xPos, other.GetComponent<GridNode>().yPos);
            Debug.Log(currentNode);
            other.GetComponent<GridNode>().visited = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("node"))
        {
            other.GetComponent<GridNode>().visited = -1;
        }
    }
}
