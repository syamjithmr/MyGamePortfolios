using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public GameObject blockExplosion;

    Vector3 TunnelEndPos;

    // Start is called before the first frame update
    void Start()
    {
        TunnelEndPos = GameObject.Find("TunnelEnd").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(tag))
        {
            FindObjectOfType<Camera>().GetComponent<GameManager>().GameOver = true;
            GameObject gameoverExplosion = Instantiate(blockExplosion, transform.position, new Quaternion());
            gameoverExplosion.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            if (transform.position.y > TunnelEndPos.y + 50)
            {
                FindObjectOfType<Camera>().GetComponent<GameManager>().Needblock = true;
                FindObjectOfType<Camera>().GetComponent<GameManager>().destroyedBlockTagName = tag;
            }
            else
                FindObjectOfType<Camera>().GetComponent<GameManager>().Needblock = false;

        }
        Destroy(this.gameObject);
    }
}
