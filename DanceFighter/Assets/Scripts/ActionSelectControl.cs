using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectControl : MonoBehaviour
{
    float keyDownTime;
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("DanceMovesSelect").gameObject.SetActive(false);
        transform.Find("FightMovesSelect").gameObject.SetActive(false);
        transform.Find("StartScreen").gameObject.SetActive(true);
        keyDownTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!transform.Find("StartScreen").gameObject.activeInHierarchy)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                keyDownTime += Time.deltaTime;
                if (!transform.Find("DanceMovesSelect").gameObject.activeInHierarchy && keyDownTime > 0.2f)
                {
                    Time.timeScale = 0.0f;
                    Camera.main.GetComponent<EthanParentConstraint>().IsPaused = true;
                    Cursor.visible = true;
                    transform.Find("DanceMovesSelect").gameObject.SetActive(true);
                }
            }
            else if (Input.GetKey(KeyCode.E))
            {
                keyDownTime += Time.deltaTime;
                if (!transform.Find("FightMovesSelect").gameObject.activeInHierarchy && keyDownTime > 0.2f)
                {
                    Time.timeScale = 0.0f;
                    Camera.main.GetComponent<EthanParentConstraint>().IsPaused = true;
                    Cursor.visible = true;
                    transform.Find("FightMovesSelect").gameObject.SetActive(true);
                }
            }
            else
            {
                Time.timeScale = 1.0f;
                Camera.main.GetComponent<EthanParentConstraint>().IsPaused = false;
                Cursor.visible = false;
                transform.Find("DanceMovesSelect").gameObject.SetActive(false);
                transform.Find("FightMovesSelect").gameObject.SetActive(false);
                keyDownTime = 0.0f;
            }
        }
    }

    public void StartGameActions()
    {
        transform.Find("StartScreen").gameObject.SetActive(false);
        Camera.main.GetComponent<EthanParentConstraint>().IsPaused = false;
        Cursor.visible = false;
    }
}
