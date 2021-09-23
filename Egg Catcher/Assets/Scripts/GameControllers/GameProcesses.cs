using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameProcesses : MonoBehaviour {
    // game intended to be played at 144fps
    public readonly int framerate = 144;

    private TextMeshProUGUI timerText;
    private float timer = 60.0f;

    private TextMeshProUGUI gameOverText;
    private TextMeshProUGUI youLoseText;
    private TextMeshProUGUI restartText;

    private PlayerMove playerScript;

    void Start() {
        timerText = GameObject.FindGameObjectWithTag("Timer").GetComponent<TextMeshProUGUI>();

        gameOverText = GameObject.Find("GameOver").GetComponent<TextMeshProUGUI>();
        youLoseText = GameObject.Find("YouLose").GetComponent<TextMeshProUGUI>();
        restartText = GameObject.Find("Restart").GetComponent<TextMeshProUGUI>();

        playerScript = GameObject.Find("basket").GetComponent<PlayerMove>();

        // setting target framerate of 144fps
        Application.targetFrameRate = framerate;
    }

    void Update() {
        // while 60 seconds havent gone by since the game start
        if (timer >= 0.0f) {
            // reduce the time and update the timer text
            timer -= Time.deltaTime;
            UpdateTimer(timer);

            // game state = game over / hit by bomb
            if (playerScript.isExploded == true) {
                // show youlose text and restart text
                youLoseText.enabled = true;
                restartText.enabled = true;
                // stop the time/game
                Time.timeScale = 0.0f;
            }
        } else {
            // game state = game over / time ran out
            gameOverText.enabled = true;
            restartText.enabled = true;
            Time.timeScale = 0.0f;
        }

        // if R is clicked at any time in the game
        if (Input.GetKeyDown(KeyCode.R)) {
            // reload the main game scene to play again
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1.0f;
        }
    }

    // updates the timer text UI element
    public void UpdateTimer(float newTime) {
        timerText.text = "Time left: " + newTime.ToString("F1");
    }
}
