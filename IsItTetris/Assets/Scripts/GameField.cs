using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class GameField
{
    private readonly int[,] gameField;
    private readonly int fieldWidth;
    private readonly int fieldHeight;
    private TetBlock currentBlock;
    private float timeElapsedSinceLastDrop;
    private float dropTime;
    private bool blockCollided;
    List<int> completedRows;
    bool completedRowsCleared;
    private bool noUpdate;
    private bool gameOver;

    public GameField(int pFieldWidth, int pFieldHeight)
    {
        fieldHeight = pFieldHeight;
        fieldWidth = pFieldWidth;
        gameField = new int[fieldHeight, fieldWidth];
        currentBlock = ScriptableObject.CreateInstance<TetBlock>();
        timeElapsedSinceLastDrop = 0.0f;
        dropTime = 0.5f;
        blockCollided = true;
        completedRows = new List<int>();
        completedRowsCleared = false;
        noUpdate = false;
        gameOver = false;
    }

    public int[,] Update(ref int score)
    {
        if (!gameOver)
        {
            if (blockCollided)
            {
                if (completedRows.Count != 0)
                {
                    if (completedRowsCleared)
                    {
                        timeElapsedSinceLastDrop += Time.deltaTime;
                        if (timeElapsedSinceLastDrop > dropTime)
                        {
                            noUpdate = false;
                            FillClearedRows();
                            timeElapsedSinceLastDrop = 0.0f;
                        }
                    }
                    else
                    {
                        foreach (int row in completedRows)
                        {
                            for (int i = 0; i < fieldWidth; i++)
                                gameField[row, i] = 0;
                            completedRowsCleared = true;
                            score += completedRows.Count * completedRows.Count * 10;
                            timeElapsedSinceLastDrop = 0.0f;
                        }
                    }
                    return gameField;
                }
                int currentBlockPosX;
                int currentBlockPosY;
                currentBlockPosX = fieldHeight - 1;
                currentBlockPosY = fieldWidth / 2;
                currentBlock.CreateBlock(currentBlockPosX, currentBlockPosY);
                if (!CheckGameFieldBlockPos(0, 0))
                {
                    gameOver = true;
                    return new int[1, 1] { { -1 } };
                }
                blockCollided = false;
                SetBlockPosition(0, 0);
            }

            Drop();

            if (Input.GetKey(KeyCode.DownArrow) && !blockCollided)
                Down();
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && !blockCollided)
                Left();
            else if (Input.GetKeyDown(KeyCode.RightArrow) && !blockCollided)
                Right();
            else if (Input.GetKeyDown(KeyCode.UpArrow) && !blockCollided)
                Up();
        }
        if (noUpdate)
        {
            return new int[1, 1] {{ -2 }};
        }
        return gameField;
    }

    private void Drop()
    {
        timeElapsedSinceLastDrop += Time.deltaTime;
        if (timeElapsedSinceLastDrop > dropTime && !blockCollided)
        {
            noUpdate = false;
            Down();
            timeElapsedSinceLastDrop = 0.0f;
        }
        else
        {
            noUpdate = true;
        }
    }

    private void Down()
    {
        if (CheckBlockPos('y', 0))
        {
            ClearBlockPosition();
            if (CheckGameFieldBlockPos(0, -1))
            {
                SetBlockPosition(0, -1);
            }
            else
            {
                SetBlockPosition(0, 0);
                blockCollided = true;
            }
        }
        else
            blockCollided = true;

        if (blockCollided)
        {
            for(int i=0; i<4; i++)
            {
                bool rowComplete = true;
                for(int j=0; j<fieldWidth; j++)
                {
                    if (gameField[(int)currentBlock.cubePos[i, 0], j] == 0)
                    {
                        rowComplete = false;
                        break;
                    }
                }
                if (rowComplete)
                    if(!completedRows.Contains((int)currentBlock.cubePos[i, 0]))
                        completedRows.Add((int)currentBlock.cubePos[i, 0]);
            }
        }
    }

    private void Up()
    {
        ClearBlockPosition();
        Rotate();
        if (CheckGameFieldBlockPos(0, 0))
        {
            SetBlockPosition(0, 0);
        }
    }

    private void Left()
    {
        if (CheckBlockPos('x', 0))
        {
            ClearBlockPosition();
            if (CheckGameFieldBlockPos(-1, 0))
            {
                SetBlockPosition(-1, 0);
            }
        }
    }

    private void Right()
    {
        if (CheckBlockPos('x', fieldWidth - 1))
        {
            ClearBlockPosition();
            if (CheckGameFieldBlockPos(1, 0))
            {
                SetBlockPosition(1, 0);
            }
        }
    }

    private void FillClearedRows()
    {
        completedRows.Sort();
        int rowsPassed = 1;
        for (int i = completedRows[0]+1; i < fieldHeight; i++)
        {
            for (int j = 0; j < fieldWidth; j++)
            {
                if (completedRows.Contains(i))
                {
                    rowsPassed++;
                    break;
                }
                else
                {
                    if (gameField[i, j] == 1)
                    {
                        gameField[i, j] = 0;
                        gameField[i-rowsPassed, j] = 1;
                    }
                }
            }
        }
        completedRows.Clear();
        completedRowsCleared = false;
    }

    private void ClearBlockPosition()
    {
        gameField[(int)currentBlock.cubePos[0, 0], (int)currentBlock.cubePos[0, 1]] = 0;
        gameField[(int)currentBlock.cubePos[1, 0], (int)currentBlock.cubePos[1, 1]] = 0;
        gameField[(int)currentBlock.cubePos[2, 0], (int)currentBlock.cubePos[2, 1]] = 0;
        gameField[(int)currentBlock.cubePos[3, 0], (int)currentBlock.cubePos[3, 1]] = 0;

    }

    private void SetBlockPosition(int x, int y)
    {
        currentBlock.cubePos[0, 0] += y; currentBlock.cubePos[0, 1] += x;
        currentBlock.cubePos[1, 0] += y; currentBlock.cubePos[1, 1] += x;
        currentBlock.cubePos[2, 0] += y; currentBlock.cubePos[2, 1] += x;
        currentBlock.cubePos[3, 0] += y; currentBlock.cubePos[3, 1] += x;
        currentBlock.cubePos[4, 0] += y; currentBlock.cubePos[4, 1] += x;

        gameField[(int)currentBlock.cubePos[0, 0], (int)currentBlock.cubePos[0, 1]] = 1;
        gameField[(int)currentBlock.cubePos[1, 0], (int)currentBlock.cubePos[1, 1]] = 1;
        gameField[(int)currentBlock.cubePos[2, 0], (int)currentBlock.cubePos[2, 1]] = 1;
        gameField[(int)currentBlock.cubePos[3, 0], (int)currentBlock.cubePos[3, 1]] = 1;
    }

    private void Rotate()
    {
        float[,] tempCubePos = new float[5, 2];
        float[,] cubePosBackUp = (float[,])currentBlock.cubePos.Clone();
        float xTranslate = currentBlock.cubePos[4, 0];
        float yTranslate = currentBlock.cubePos[4, 1];

        currentBlock.cubePos[0, 1] -= yTranslate; currentBlock.cubePos[0, 0] -= xTranslate;
        currentBlock.cubePos[1, 1] -= yTranslate; currentBlock.cubePos[1, 0] -= xTranslate;
        currentBlock.cubePos[2, 1] -= yTranslate; currentBlock.cubePos[2, 0] -= xTranslate;
        currentBlock.cubePos[3, 1] -= yTranslate; currentBlock.cubePos[3, 0] -= xTranslate;
        currentBlock.cubePos[4, 1] -= yTranslate; currentBlock.cubePos[4, 0] -= xTranslate;

        tempCubePos[0, 1] = currentBlock.cubePos[0, 1] * (int)Mathf.Cos(Mathf.PI / 2) - currentBlock.cubePos[0, 0] * (int)Mathf.Sin(Mathf.PI / 2);
        tempCubePos[1, 1] = currentBlock.cubePos[1, 1] * (int)Mathf.Cos(Mathf.PI / 2) - currentBlock.cubePos[1, 0] * (int)Mathf.Sin(Mathf.PI / 2);
        tempCubePos[2, 1] = currentBlock.cubePos[2, 1] * (int)Mathf.Cos(Mathf.PI / 2) - currentBlock.cubePos[2, 0] * (int)Mathf.Sin(Mathf.PI / 2);
        tempCubePos[3, 1] = currentBlock.cubePos[3, 1] * (int)Mathf.Cos(Mathf.PI / 2) - currentBlock.cubePos[3, 0] * (int)Mathf.Sin(Mathf.PI / 2);
        tempCubePos[4, 1] = currentBlock.cubePos[4, 1] * (int)Mathf.Cos(Mathf.PI / 2) - currentBlock.cubePos[4, 0] * (int)Mathf.Sin(Mathf.PI / 2);

        tempCubePos[0, 0] = currentBlock.cubePos[0, 1] * (int)Mathf.Sin(Mathf.PI / 2) + currentBlock.cubePos[0, 0] * (int)Mathf.Cos(Mathf.PI / 2);
        tempCubePos[1, 0] = currentBlock.cubePos[1, 1] * (int)Mathf.Sin(Mathf.PI / 2) + currentBlock.cubePos[1, 0] * (int)Mathf.Cos(Mathf.PI / 2);
        tempCubePos[2, 0] = currentBlock.cubePos[2, 1] * (int)Mathf.Sin(Mathf.PI / 2) + currentBlock.cubePos[2, 0] * (int)Mathf.Cos(Mathf.PI / 2);
        tempCubePos[3, 0] = currentBlock.cubePos[3, 1] * (int)Mathf.Sin(Mathf.PI / 2) + currentBlock.cubePos[3, 0] * (int)Mathf.Cos(Mathf.PI / 2);
        tempCubePos[4, 0] = currentBlock.cubePos[4, 1] * (int)Mathf.Sin(Mathf.PI / 2) + currentBlock.cubePos[4, 0] * (int)Mathf.Cos(Mathf.PI / 2);

        tempCubePos[0, 1] += yTranslate; tempCubePos[0, 0] += xTranslate;
        tempCubePos[1, 1] += yTranslate; tempCubePos[1, 0] += xTranslate;
        tempCubePos[2, 1] += yTranslate; tempCubePos[2, 0] += xTranslate;
        tempCubePos[3, 1] += yTranslate; tempCubePos[3, 0] += xTranslate;
        tempCubePos[4, 1] += yTranslate; tempCubePos[4, 0] += xTranslate;
        currentBlock.cubePos = (float[,])tempCubePos.Clone();
        if (!(CheckBlockPos('y', -1) && CheckBlockPos('x', -1) && CheckBlockPos('x', fieldWidth) && CheckGameFieldBlockPos(0, 0)))
            currentBlock.cubePos = cubePosBackUp;
    }

    private bool CheckBlockPos(char coordinate, int value)
    {
        if (coordinate.Equals('x'))
            return ((int)currentBlock.cubePos[0, 1] != value &&
                    (int)currentBlock.cubePos[1, 1] != value &&
                    (int)currentBlock.cubePos[2, 1] != value &&
                    (int)currentBlock.cubePos[3, 1] != value);
        else
            return ((int)currentBlock.cubePos[0, 0] != value &&
                    (int)currentBlock.cubePos[1, 0] != value &&
                    (int)currentBlock.cubePos[2, 0] != value &&
                    (int)currentBlock.cubePos[3, 0] != value);
    }

    private bool CheckGameFieldBlockPos(int x, int y)
    {
        return gameField[(int)currentBlock.cubePos[0, 0]+y, (int)currentBlock.cubePos[0, 1]+x] == 0 &&
               gameField[(int)currentBlock.cubePos[1, 0]+y, (int)currentBlock.cubePos[1, 1]+x] == 0 &&
               gameField[(int)currentBlock.cubePos[2, 0]+y, (int)currentBlock.cubePos[2, 1]+x] == 0 &&
               gameField[(int)currentBlock.cubePos[3, 0]+y, (int)currentBlock.cubePos[3, 1]+x] == 0;
    }
}
