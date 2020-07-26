using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver = false;
    public GameObject gameOverPanel;
    public static bool isGameStarted;
    public GameObject startingText;
    public ParticleSystem motorIzq;
    public ParticleSystem motorDer;
    public GameObject tablero;

    void Start()
    {
        gameOver = false;
        tablero.SetActive(true);
        Time.timeScale = 1;
        isGameStarted = false;
        FindObjectOfType<AudioManager>().PlaySound("Tap to start");
        motorDer.Stop();
        motorIzq.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            Time.timeScale = 0;
            tablero.SetActive(false);
            gameOverPanel.SetActive(true);
            motorDer.Stop();
            motorIzq.Stop();
        }
        if (SwipeManager.tap)
        {
            if (!isGameStarted)
            {
                Destroy(startingText);
                FindObjectOfType<AudioManager>().StopSound("Tap to start");
                FindObjectOfType<AudioManager>().PlaySound("Stage 1");
                motorDer.Play();
                motorIzq.Play();
            }
            isGameStarted = true;
        }
    }
}
