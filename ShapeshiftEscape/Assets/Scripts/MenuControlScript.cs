using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControlScript : MonoBehaviour
{
    GameManager GameManagerScript;
    GameObject StartMenu;
    GameObject PauseMenu;
    GameObject GameWonMenu;
    GameObject GameOverMenu;
    // Start is called before the first frame update
    void Start()
    {
        GameManagerScript = FindObjectOfType<Camera>().GetComponent<GameManager>();
        StartMenu = transform.Find("StartMenu").gameObject;
        StartMenu.SetActive(true);
        PauseMenu = transform.Find("PauseMenu").gameObject;
        PauseMenu.SetActive(false);
        GameWonMenu = transform.Find("GameWonMenu").gameObject;
        GameWonMenu.SetActive(false);
        GameOverMenu = transform.Find("GameOverMenu").gameObject;
        GameOverMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ValidateGameMenu(StartMenu, GameManagerScript.GameStart);
        ValidateGameMenu(PauseMenu, GameManagerScript.GamePaused);
        ValidateGameMenu(GameWonMenu, GameManagerScript.GameWon);
        ValidateGameMenu(GameOverMenu, GameManagerScript.GameOver);
        
    }

    void ValidateGameMenu(GameObject menu, bool needShow)
    {
        if (needShow)
        {
            if (!menu.activeInHierarchy)
                menu.SetActive(true);
        }
        else
            menu.SetActive(false);
    }
}
