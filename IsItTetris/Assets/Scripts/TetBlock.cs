using UnityEngine;
using UnityEditor;

public class TetBlock : ScriptableObject
{
    int blockType=0;
    public float[,] cubePos;

    public void CreateBlock(float firstCubePosX, float firstCubePosY)
    {
        blockType = Random.Range(0, 7);
        //if (blockType > 6) blockType = 0;
        switch (blockType)
        {
            case (int)BlockType.L:
                cubePos = new float[5, 2] {
                    { firstCubePosX, firstCubePosY },
                    { firstCubePosX-1, firstCubePosY },
                    { firstCubePosX-2, firstCubePosY },
                    { firstCubePosX-2, firstCubePosY+1 },
                    { firstCubePosX-1, firstCubePosY }
                };
                break;
            case (int)BlockType.J:
                cubePos = new float[5, 2] {
                    { firstCubePosX, firstCubePosY },
                    { firstCubePosX-1, firstCubePosY },
                    { firstCubePosX-2, firstCubePosY },
                    { firstCubePosX-2, firstCubePosY-1 },
                    { firstCubePosX-1, firstCubePosY }
                };
                break;
            case (int)BlockType.Z:
                cubePos = new float[5, 2] {
                    { firstCubePosX, firstCubePosY },
                    { firstCubePosX, firstCubePosY-1 },
                    { firstCubePosX-1, firstCubePosY },
                    { firstCubePosX-1, firstCubePosY+1 },
                    { firstCubePosX-0.5f, firstCubePosY }
                };
                break;
            case (int)BlockType.S:
                cubePos = new float[5, 2] {
                    { firstCubePosX, firstCubePosY },
                    { firstCubePosX, firstCubePosY+1 },
                    { firstCubePosX-1, firstCubePosY },
                    { firstCubePosX-1, firstCubePosY-1 },
                    { firstCubePosX-0.5f, firstCubePosY }
                };
                break;
            case (int)BlockType.T:
                cubePos = new float[5, 2] {
                    { firstCubePosX, firstCubePosY },
                    { firstCubePosX-1, firstCubePosY },
                    { firstCubePosX-1, firstCubePosY-1 },
                    { firstCubePosX-1, firstCubePosY+1 },
                    { firstCubePosX-1, firstCubePosY }
                };
                break;
            case (int)BlockType.Sq:
                cubePos = new float[5, 2] {
                    { firstCubePosX, firstCubePosY },
                    { firstCubePosX, firstCubePosY+1 },
                    { firstCubePosX-1, firstCubePosY },
                    { firstCubePosX-1, firstCubePosY+1 },
                    { firstCubePosX-0.5f, firstCubePosY+0.5f }
                };
                break;
            case (int)BlockType.Col:
                cubePos = new float[5, 2] {
                    { firstCubePosX, firstCubePosY },
                    { firstCubePosX-1, firstCubePosY },
                    { firstCubePosX-2, firstCubePosY },
                    { firstCubePosX-3, firstCubePosY },
                    { firstCubePosX-1.5f, firstCubePosY }
                };
                break;
            default:
                break;
        }
        blockType++;
    }
    public void setBlockPosition()
    {

    }
}

public enum BlockType
{
    L, J, Z, S, T, Sq, Col
};