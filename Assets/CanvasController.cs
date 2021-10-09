using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Mechanics;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{

    public PlayerController player;
    public Text coins;
    public Text score;
    public Text timeText;

    public int time = 300;

    // Start is called before the first frame update
    void Start()
    {
        StartTime();
    }

    // Update is called once per frame
    void Update()
    {
        score.text = (player.coinCount * 200).ToString();
        if (player.coinCount < 10)
        {
            coins.text = "x0" + player.coinCount.ToString();
        }
        else
        {
            coins.text = "x" + player.coinCount.ToString();
        }
    }

    void StartTime()
    {
        StartCoroutine(Time());
    }

    IEnumerator Time()
    {
        while (time > 0)
        {
            timeText.text = time.ToString();
            yield return new WaitForSeconds(1);
            time--;
        }
        SceneManager.LoadScene("Overworld 1-1");
    }
}
