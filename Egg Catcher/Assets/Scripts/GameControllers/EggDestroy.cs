using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EggDestroy : MonoBehaviour {
    public GameObject brokenEggPrefab;
    private GameObject brokenEgg;
    public GameObject bombColPrefab;

    private Vector2 scorePos, eggPos;
    public GameObject scorePrefab;
    public GameObject score2Prefab;
    public Transform canvas;

    GameObject newScore;
    GameObject newScore2;
    private Color32 posScore = new Color32(12, 191, 0, 255);
    private Color32 goldScore = new Color32(212, 175, 55, 255);
    private Color32 negScore = new Color32(200, 0, 0, 255);

    private GameObject scoreText;
    public static int score = 0;

    private readonly float delay = 3f;

    void Start() {
        scoreText = GameObject.FindGameObjectWithTag("Score");
        canvas = GameObject.Find("Canvas").GetComponent<Transform>();
    }

    private void Update() {
        scorePos = PlayerMove.pos;
        scorePos.x += 0.75f;
        scorePos.y += 1.0f;
    }

    public void SetScore(int potentialScore, GameObject scoreText) {
        if (potentialScore == 25 || potentialScore == 100) {
            score += potentialScore;
        } else if (potentialScore == 50) {
            score -= potentialScore;
            if (score < 0) {
                score = 0;
            }
        } else if (potentialScore == 0) {
            score = 0;
        }
        SetFloatingScore(potentialScore);
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
    }

    public void SetFloatingScore(int potentialScore) {
        Vector3 scoreTextFloatPos = new Vector3(scoreText.transform.position.x + 3.25f, scoreText.transform.position.y + .4f, scoreText.transform.position.z);
        if (potentialScore == 25) {
            newScore = Instantiate(scorePrefab, scorePos, Quaternion.identity);
            newScore2 = Instantiate(score2Prefab, scoreTextFloatPos, Quaternion.identity);

            newScore.transform.SetParent(canvas);
            newScore.transform.GetChild(0).GetComponent<TextMesh>();

            newScore.transform.localScale = Vector3.one;
            newScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + potentialScore.ToString();
            newScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().faceColor = posScore;

            newScore2.transform.SetParent(canvas);
            newScore2.transform.GetChild(0).GetComponent<TextMesh>();

            newScore2.transform.localScale = Vector3.one;
            newScore2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + potentialScore.ToString();

        } else if (potentialScore == 100) {
            newScore = Instantiate(scorePrefab, scorePos, Quaternion.identity);
            newScore2 = Instantiate(score2Prefab, scoreTextFloatPos, Quaternion.identity);

            newScore.transform.SetParent(canvas);
            newScore.transform.GetChild(0).GetComponent<TextMesh>();

            newScore.transform.localScale = Vector3.one;
            newScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + potentialScore.ToString();
            newScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().faceColor = goldScore;

            newScore2.transform.SetParent(canvas);
            newScore2.transform.GetChild(0).GetComponent<TextMesh>();

            newScore2.transform.localScale = Vector3.one;
            newScore2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + potentialScore.ToString();

        } else if (potentialScore == 50) {
            newScore = Instantiate(scorePrefab, eggPos, Quaternion.identity);
            newScore2 = Instantiate(score2Prefab, scoreTextFloatPos, Quaternion.identity);

            newScore.transform.SetParent(canvas);
            newScore.transform.GetChild(0).GetComponent<TextMesh>();

            newScore.transform.localScale = Vector3.one;
            newScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-" + potentialScore.ToString();
            newScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().faceColor = negScore;

            newScore2.transform.SetParent(canvas);
            newScore2.transform.GetChild(0).GetComponent<TextMesh>();

            newScore2.transform.localScale = Vector3.one;
            newScore2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-" + potentialScore.ToString();
            newScore2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().faceColor = negScore;

        } else {
            newScore = Instantiate(scorePrefab, scorePos, Quaternion.identity);
            newScore2 = Instantiate(score2Prefab, scoreTextFloatPos, Quaternion.identity);

            newScore.transform.SetParent(canvas);
            newScore.transform.GetChild(0).GetComponent<TextMesh>();

            newScore2.transform.SetParent(canvas);
            newScore2.transform.GetChild(0).GetComponent<TextMesh>();

            newScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = null;
            newScore2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = null;
        }
        Destroy(newScore, 1.0f);
        Destroy(newScore2, 1.0f);
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
            eggPos = this.transform.position;
            this.GetComponent<SpriteRenderer>().enabled = false;

            // spawn broken egg where egg was destroyed
            brokenEgg = Instantiate(brokenEggPrefab) as GameObject;
            brokenEgg.transform.position = eggPos; // eggPos

            eggPos.x += 0.5f;
            eggPos.y += 0.5f;

            // destroy egg after "delay" seconds
            StartCoroutine( PopEgg() );

            // decrease score by 50 if egg hits ground
            SetScore(50, scoreText);
        }
    }

    IEnumerator PopEgg() {
        yield return new WaitForSeconds(delay);
        Renderer renderer = brokenEgg.GetComponent<Renderer>();
        Color newColor = renderer.material.color;

        while (newColor.a > 0) {
            newColor.a -= delay * Time.deltaTime;
            renderer.material.color = newColor;
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
