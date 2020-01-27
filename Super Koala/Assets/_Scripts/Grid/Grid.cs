using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int rows;
    public int col;
    public int gridNodeSize = 1/2;
    public GameObject gridNodePrefab;
    public Vector2 startLocation = new Vector2(0, 0);

    public GameObject[,] gridNodes;

    private void Awake()
    {
        gridNodes = new GameObject[col, rows];
        GenerateGrid();
    }
    void Start()
    {
        foreach(GameObject obj in gridNodes)
        {
            obj.GetComponent<GridNode>().visited = -1;
        }
    }

    void Update()
    {
        foreach(GameObject obj in gridNodes)
        {
            if (obj.GetComponent<GridNode>().visited == 0)
            {
                Debug.Log(obj.GetComponent<GridNode>().xPos + ", " + obj.GetComponent<GridNode>().yPos);
            }
        }
    }

    void GenerateGrid()
    {
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject obj = Instantiate(gridNodePrefab, new Vector2(startLocation.x + gridNodeSize * i, startLocation.y + gridNodeSize * j), Quaternion.identity);
                obj.transform.SetParent(gameObject.transform);
                obj.GetComponent<GridNode>().xPos = j;
                obj.GetComponent<GridNode>().yPos = i;
                gridNodes[i, j] = obj;
            }
        }
    }
}
