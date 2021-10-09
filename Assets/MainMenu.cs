using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject gameController;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayGame()
    {
        Time.timeScale = 1;
        mainMenu.SetActive(false);
        gameController.SetActive(true);
    }
}
