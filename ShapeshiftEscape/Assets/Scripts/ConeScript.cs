using UnityEngine;

public class ConeScript : MonoBehaviour
{
    ParticleSystem speedLines;
    bool isSpeeding;
    // Start is called before the first frame update
    void Start()
    {
        speedLines = GetComponent<ParticleSystem>();
        isSpeeding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpeeding)
        {
            if (speedLines.isStopped)
            {
                isSpeeding = false;
            }
        }
    }

    public void initiateSpeeding()
    {
        isSpeeding = true;
    }
}
