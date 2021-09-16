using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameProcesses : MonoBehaviour {
    public readonly int framerate = 144;

    private GameObject timerText;
    private float timer = 60.0f;

    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI youLoseText;
    public TextMeshProUGUI restartText;

    private readonly Color32 blankText  = new Color32(0,    0,      0,      0); // black alpha 0
    private readonly Color32 fullText   = new Color32(255,  255,    255,    0); // white alpha 0

    private GameObject player;
    private PlayerMove playerScript;


    // Start is called before the first frame update
    void Start() {
        timerText = GameObject.FindGameObjectWithTag("Timer");

        gameOverText = GameObject.Find("GameOver").GetComponent<TextMeshProUGUI>();
        youLoseText = GameObject.Find("YouLose").GetComponent<TextMeshProUGUI>();
        restartText = GameObject.Find("Restart").GetComponent<TextMeshProUGUI>();

        player = GameObject.FindGameObjectWithTag("Basket");
        playerScript = player.GetComponent<PlayerMove>();

        Application.targetFrameRate = framerate;
    }

    // Update is called once per frame
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
        timerText.GetComponent<Text>().text = "Time left: " + newTime.ToString("F1");
    }
}
