using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public GameObject controller;

    public float TimeLeft;
    public float TimeLeftWhite;

    public Text TimerTxt;

    public Text TimerTxtwhite;

    
    void Update()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        Game sc = controller.GetComponent<Game>();
        updateTimerBlack(TimeLeft);
        updateTimerWhite(TimeLeftWhite);
        if(sc.GetTurns() > 0){
            if (sc.GetCurrentPlayer() == "black")
            {
                if (TimeLeft > 0)
                {
                    TimeLeft -= Time.deltaTime;
                    updateTimerBlack(TimeLeft);
                }
                else
                {
                    controller.GetComponent<Game>().Winner("white");
                }
            }
            else
            {
                if (TimeLeftWhite > 0)
                {
                    TimeLeftWhite -= Time.deltaTime;
                    updateTimerWhite(TimeLeftWhite);
                }
                else
                {
                    controller.GetComponent<Game>().Winner("black");
                }
            }
        }
    }

    private void updateTimerBlack(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerTxt.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    private void updateTimerWhite(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerTxtwhite.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
