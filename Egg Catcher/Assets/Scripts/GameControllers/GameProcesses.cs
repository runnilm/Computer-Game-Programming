using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameProcesses : MonoBehaviour {
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

        Application.targetFrameRate = framerate;
    }

    void Update() {
        if (timer >= 0.0f) {
            timer -= Time.deltaTime;
            UpdateTimer(timer);

            // game state = game over / hit by bomb
            if (playerScript.isExploded == true) {
                youLoseText.enabled = true;
                restartText.enabled = true;
                Time.timeScale = 0.0f;
            }
        } else {
            // game state = game over / time ran out
            gameOverText.enabled = true;
            restartText.enabled = true;
            Time.timeScale = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1.0f;
        }
    }

    public void UpdateTimer(float newTime) {
        timerText.text = "Time left: " + newTime.ToString("F1");
    }
}
