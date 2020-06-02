using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject cubePrefab, gameoverMsg, gamePausedMsg, gameScore;
    private const int fieldWidth = 10;
    private const int fieldHeight = 20;
    private GameField gameField;
    private int score = 0;
    private bool gamePaused = false;

    // Start is called before the first frame update
    void Start()
    {
        gameField = new GameField(fieldWidth, fieldHeight);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (!gamePaused)
            {
                gamePaused = true;
                gamePausedMsg.SetActive(true);
            }
            else
            {
                gamePaused = false;
                gamePausedMsg.SetActive(false);
            }
        if (!gamePaused)
        {
            DrawGameField(gameField.Update(ref score));
            TextMesh scoreText = gameScore.GetComponent<TextMesh>();
            scoreText.text = score.ToString();
        }
    }

    void DrawGameField(int[,] gameField)
    {
        if (gameField[0, 0] == -1)
            gameoverMsg.SetActive(true);
        else if (gameField[0, 0] == -2)
            return;
        else
        {
            foreach (GameObject oldCube in GameObject.FindGameObjectsWithTag("cube"))
            {
                Destroy(oldCube);
            }
            for (int i = fieldHeight - 1; i >= 0; i--)
            {
                for (int j = fieldWidth - 1; j >= 0; j--)
                {
                    if (gameField[i, j] == 1)
                    {
                        Instantiate(cubePrefab, new Vector3(j - fieldWidth / 2, i), new Quaternion());
                    }
                }
            }
        }
    }

}
