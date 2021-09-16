using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EggDestroy : MonoBehaviour {
    public GameObject brokenEggPrefab;
    private GameObject brokenEgg;

    private GameObject scoreText;
    public static int score;

    private readonly float delay = 3f;

    // Start is called before the first frame update
    void Start() {
        scoreText = GameObject.FindGameObjectWithTag("Score");
        SetScore(0, scoreText);
    }

    public void SetScore(int potentialScore, GameObject scoreBoard) {
        if (potentialScore == 25 || potentialScore == 100) {
            score += potentialScore;
        } else if (potentialScore == 50) {
            score -= potentialScore;
            if (score < 0) {
                score = 0;
            }
        }
        scoreBoard.GetComponent<Text>().text = "Score: " + score.ToString();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Basket") {
            // increase score by 25 if normal egg
            if (this.gameObject.tag == "Egg") {
                SetScore(25, scoreText);
            }
            // increase score by 100 if golden egg
            else if (this.gameObject.tag == "GoldenEgg") {
                SetScore(100, scoreText);
            }
            Destroy(this.gameObject);
        } else if (col.gameObject.tag == "Terrain") {
            // destroy egg and get position where it was destroyed
            Vector2 eggPos = this.transform.position;
            this.GetComponent<SpriteRenderer>().enabled = false;

            // spawn broken egg where egg was destroyed
            brokenEgg = Instantiate(brokenEggPrefab) as GameObject;
            brokenEgg.transform.position = eggPos;

            // destroy egg after "delay" seconds
            StartCoroutine( PopEgg() );

            // decrease score by 50 if egg hits ground
            SetScore(50, scoreText);
        }
    }

    IEnumerator PopEgg() {
        yield return new WaitForSecondsRealtime(delay);
        Destroy(this.gameObject);
        Destroy(brokenEgg.gameObject);
    }
}
