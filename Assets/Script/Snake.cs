using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour
{
    int[,] grid = new int[15,10];
    int snakeScore = 3;
    int snakeX = 0;
    int snakeY = 4;

    Transform snakeTransform;
    float lastMove;
    float timeInBetweenMoves = 0.5f;
    Vector3 direction;
    bool haslost;

    public GameObject atheiosPrefab;
    public Text scoreText;

    private void Start()
    {
        snakeTransform = transform;
        direction = Vector3.right;
        grid[snakeX, snakeY] = snakeScore;

        scoreText.text = "Score : " + snakeScore.ToString();

        grid[8, 4] = -1;
        GameObject go = Instantiate(atheiosPrefab) as GameObject;
        go.transform.position = new Vector3(8, 4, 0);
        go.name = "Atheios";
    }

	private void Update()
    {
        if (haslost)
        {
            return;
        }

        if (Time.time - lastMove > timeInBetweenMoves)
        {
            //Every move iterate through our whole array and remove ever tile that's not -1 or 0
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] > 0)
                    { 
                        grid[i, j]--;
                        if (grid[i, j] == 0)
                        {
                            //we have to destroy something
                            GameObject toDestroy = GameObject.Find(i.ToString() + j.ToString());
                            if (toDestroy != null)
                            {
                            Destroy(toDestroy);
                            }
                        }
                    }
                }
            }
            
            lastMove = Time.time;

            //Add up direction to snakeX & snakeY
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.position = new Vector3(snakeX, snakeY, 0);
            go.name = snakeX.ToString() + snakeY.ToString();

            if (direction.x == 1)
            {
                snakeX++;
            }
            if (direction.x == -1)
            {
                snakeX--;
            }
            if (direction.y == 1)
            {
                snakeY++;
            }
            if (direction.y == -1)
            {
                snakeY--;
            }
           
            // if snake goes out of bounds
            if (snakeX >= grid.GetLength(0) || snakeX < 0 || snakeY >= grid.GetLength(1) || snakeY < 0)
            {
                haslost = true;
            }
            else
            {
                //We eat Atheios
                if (grid[snakeX, snakeY] == -1)
                {
                    Debug.Log("Atheios!");
                    GameObject toDestroy = GameObject.Find("Atheios");
                    Destroy(toDestroy);
                    snakeScore++;
                    scoreText.text = "Score : " + snakeScore.ToString();

                    for (int i = 0; i < grid.GetLength(0); i++)
                    {
                        for (int j = 0; j < grid.GetLength(1); j++)
                        {
                            if (grid[i, j] > 0)
                                grid[i, j]++;
                        }
                    }
                    //create new atheios
                    bool atheiosCreated = false;
                    while(!atheiosCreated)
                    { 
                        int x = UnityEngine.Random.Range(0, grid.GetLength(0));
                        int y = UnityEngine.Random.Range(0, grid.GetLength(1));
                        if (grid[x, y] == 0)
                        {
                            grid[x, y] = -1;
                            GameObject atheios = Instantiate(atheiosPrefab) as GameObject;
                            atheios.transform.position = new Vector3(x, y, 0);
                            atheios.name = "Atheios";
                            atheiosCreated = true;
                        }
                    }
                }
                else if (grid[snakeX, snakeY] != 0)
                {
                    haslost = true;
                    Debug.Log("We lost");
                    return;
                }

                //move
                snakeTransform.position += direction;
                grid[snakeX, snakeY] = snakeScore;
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            direction = Vector3.up;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            direction = Vector3.left;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            direction = Vector3.down;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            direction = Vector3.right;
        }
    }
    public void OnBackButtonClick()
    {
        SceneManager.LoadScene("Menu");
    }
}
