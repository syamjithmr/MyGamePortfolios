using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class GameManager : MonoBehaviour
{
    public GameObject circleCone;
    public GameObject squareCone;
    public GameObject triangleCone;

    public GameObject circleBlock;
    public GameObject squareBlock;
    public GameObject triangleBlock;

    GameObject currentCone;
    int currentConeIndex;
    Vector3 currentConeVelocity;
    int maxConeVelocity;

    List<GameObject> blocks = new List<GameObject>();
    public string destroyedBlockTagName { get; set; }

    public bool GameStart { get; set; }
    public bool GamePaused { get; set; }
    public bool GameWon { get; set; }
    public bool GameOver { get; set; }
    public bool Needblock { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        blocks.Add(Instantiate(generateRandomBlock(), new Vector3(0.0f, 10.0f, 0.0f), new Quaternion()));
        blocks.Add(Instantiate(generateRandomBlock(), new Vector3(0.0f, 0.0f, 0.0f), new Quaternion()));
        blocks.Add(Instantiate(generateRandomBlock(), new Vector3(0.0f, -10.0f, 0.0f), new Quaternion()));

        currentConeVelocity = new Vector3(0.0f, -5.0f);
        maxConeVelocity = -10;
         currentConeIndex = Random.Range(0, 3);
        generateNextCone(circleCone.transform.position, circleCone.transform.rotation);
        transform.position = new Vector3(transform.position.x, currentCone.transform.position.y + 5.0f, transform.position.z);
        GameStart = true;
        GamePaused = false;
        GameWon = false;
        GameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameOver && !GamePaused && !GameStart && !GameWon)
        {
            transform.position = new Vector3(transform.position.x, currentCone.transform.position.y + 5.0f, transform.position.z);
            if (Needblock)
            {
                blocks.Add(Instantiate(generateRandomBlock(), new Vector3(0.0f, blocks[2].transform.position.y - 10.0f, 0.0f), new Quaternion()));
                if (blocks[1].CompareTag(destroyedBlockTagName))
                {
                    if (currentConeVelocity.y > maxConeVelocity)
                    {
                        currentConeVelocity.y -= 1;
                        currentCone.GetComponent<Rigidbody>().velocity = currentConeVelocity;
                        currentCone.GetComponent<ParticleSystem>().Pause();
                        ParticleSystem.MainModule mainModule = currentCone.GetComponent<ParticleSystem>().main;
                        mainModule.startSpeed = mainModule.startSpeed.constant + 0.1f;
                        ParticleSystem.EmissionModule emissionModule = currentCone.GetComponent<ParticleSystem>().emission;
                        emissionModule.rateOverTime = emissionModule.rateOverTime.constant + 10;
                        currentCone.GetComponent<ParticleSystem>().Play();
                    }
                }
                blocks.RemoveAt(0);
                Needblock = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Vector3 currConePos = new Vector3(currentCone.transform.position.x, currentCone.transform.position.y, currentCone.transform.position.z);
                Quaternion currConeRot = new Quaternion(currentCone.transform.rotation.x, currentCone.transform.rotation.y, currentCone.transform.rotation.z, currentCone.transform.rotation.w);
                Destroy(currentCone);
                generateNextCone(currConePos, currConeRot);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
                pauseGame();
        }
        else if (GameWon)
        {
            currentCone.GetComponent<Rigidbody>().velocity = currentConeVelocity;
        }
        else
        {
            currentCone.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f);
            ParticleSystem.EmissionModule emission = currentCone.GetComponent<ParticleSystem>().emission;
            emission.enabled = false;
        }
    }

    public void startGame()
    {
        GameStart = false;
        currentCone.GetComponent<Rigidbody>().velocity = currentConeVelocity;
        ParticleSystem.EmissionModule emission = currentCone.GetComponent<ParticleSystem>().emission;
        emission.enabled = true;
    }

    public void pauseGame()
    {
        GamePaused = true;
        currentCone.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f);
    }

    public void resumeGame()
    {
        GamePaused = false;
        currentCone.GetComponent<Rigidbody>().velocity = currentConeVelocity;
    }

    public void reloadGame()
    {
        Destroy(blocks[0]);
        Destroy(blocks[1]);
        Destroy(blocks[2]);
        blocks.Clear();

        Destroy(currentCone);

        blocks.Add(Instantiate(generateRandomBlock(), new Vector3(0.0f, -10.0f, 0.0f), new Quaternion()));
        blocks.Add(Instantiate(generateRandomBlock(), new Vector3(0.0f, -20.0f, 0.0f), new Quaternion()));
        blocks.Add(Instantiate(generateRandomBlock(), new Vector3(0.0f, -30.0f, 0.0f), new Quaternion()));

        GameStart = true;
        GamePaused = false;
        GameWon = false;
        GameOver = false;

        currentConeVelocity = new Vector3(0.0f, -5.0f);
        currentConeIndex = Random.Range(0, 3);
        generateNextCone(circleCone.transform.position, circleCone.transform.rotation);
    }

    GameObject generateRandomBlock()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                return circleBlock;
            case 1:
                return squareBlock;
            default:
                return triangleBlock;
        }
    }
    GameObject generateCone(int index)
    {
        switch (index)
        {
            case 0:
                return circleCone;
            case 1:
                return squareCone;
            default:
                return triangleCone;
        }
    }

    void generateNextCone(Vector3 pos, Quaternion rot)
    {
        if (++currentConeIndex == 3)
            currentConeIndex = 0;
        //ConeScript prevConeScript = currentCone.GetComponent<ConeScript>();
        currentCone = Instantiate(generateCone(currentConeIndex), pos, rot);
        currentCone.GetComponent<Rigidbody>().velocity = currentConeVelocity;

        ConstraintSource constraintSrc = new ConstraintSource
        {
            sourceTransform = currentCone.transform,
            weight = 1.0f
        };
        GetComponent<LookAtConstraint>().SetSource(0, constraintSrc);
    }

}
