using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 20;
    public int rows = 20;
    public Count boulderCount = new Count(20, 25);
    // public Count bombCount = new Count(1, 3);
    // public GameObject[] exitTiles;
    public GameObject[] floorTiles;
    public GameObject[] leftWallTiles;
    public GameObject[] rightWallTiles;
    public GameObject[] topWallTiles;
    public GameObject[] bottomWallTiles;
    public GameObject topLeftWallTile;
    public GameObject topRightWallTile;
    public GameObject bottomLeftWallTile;
    public GameObject bottomRightWallTile;
    public GameObject[] boulderTiles;
    // public GameObject[] bombTiles;
    // public GameObject[] playerTiles;
    public GameObject leftGoalTile;
    public GameObject rightGoalTile;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();
    // private List<int> edgeIndexes = new List<int>();

    void InitializeList()
    {
        // int index = 0;
        gridPositions.Clear();

        // for (int x = 0; x < columns; x++)
        for (int x = 1; x < columns - 1; x++)
        {
            // for (int y = 0; y < rows; y++)
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));

                //if (x == 0 || y == 0 || x == columns - 1 || y == rows - 1)
                //{
                //    edgeIndexes.Add(index);
                //}

                //index++;
            }
        }
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        //building edge around active gameboard
        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                // choose random floor tile
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                // is it an outer edge?
                if (y == -1 && x == -1) // bottom left
                {
                    toInstantiate = bottomLeftWallTile;
                }
                else if (y == -1 && x == columns) // bottom right
                {
                    toInstantiate = bottomRightWallTile;
                }
                else if (y == rows && x == -1) // top left
                {
                    toInstantiate = topLeftWallTile;
                }
                else if (y == rows && x == columns) // top right
                {
                    toInstantiate = topRightWallTile;
                }
                else if (x == -1) // left
                {
                    toInstantiate = leftWallTiles[Random.Range(0, leftWallTiles.Length)];
                }
                else if (x == columns) // right
                {
                    toInstantiate = rightWallTiles[Random.Range(0, rightWallTiles.Length)];
                }
                else if (y == -1) // bottom
                {
                    toInstantiate = bottomWallTiles[Random.Range(0, bottomWallTiles.Length)];
                }
                else if (y == rows) // top
                {
                    toInstantiate = topWallTiles[Random.Range(0, topWallTiles.Length)];
                }

                //Quaternion.identity means no rotation
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition(int min, int max)
    {
        int randomIndex = Random.Range(min, max);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition(0, gridPositions.Count);
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    //void LayoutObjectsAroundEdge(GameObject[] tileArray)
    //{
    //    for (int i = 0; i < tileArray.Length; i++)
    //    {
    //        Vector3 randomPosition = RandomPosition(edgeIndexes.Min(), edgeIndexes.Max());
    //        GameObject tileChoice = tileArray[i];
    //        Instantiate(tileChoice, randomPosition, Quaternion.identity);
    //    }
    //}

    // going to be called by game manager
    public void SetupScene(int level)
    {
        BoardSetup();
        InitializeList();

        // LayoutObjectsAroundEdge(playerTiles);

        // LayoutObjectAtRandom(bombTiles, bombCount.minimum, bombCount.maximum);

        // increase number of boulders based on level 
        int boulderMin = (int)Mathf.Log(level, 2f) + boulderCount.minimum;
        int boulderMax = (int)Mathf.Log(level, 2f) + boulderCount.maximum;
        //Debug.Log("boulderMin: " + boulderMin + " boulderMax: " + boulderMax);
        LayoutObjectAtRandom(boulderTiles, boulderMin, boulderMax);

        // LayoutObjectAtRandom(exitTiles, 1, 1);

        // top goal
        Instantiate(rightGoalTile, new Vector3(columns / 2, rows - 1, 0f), Quaternion.identity);
        Instantiate(leftGoalTile, new Vector3((columns / 2) - 1, rows - 1, 0f), Quaternion.identity);

        // bottom goal
        Instantiate(rightGoalTile, new Vector3(columns / 2, 0, 0f), Quaternion.identity);
        Instantiate(leftGoalTile, new Vector3((columns / 2) - 1, 0, 0f), Quaternion.identity);
    }
}
