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
    // defining colors for use with floating scores
    // (posScore for normal eggs text color)
    private Color32 posScore = new Color32(12, 191, 0, 255);
    // (goldScore for golden eggs)
    private Color32 goldScore = new Color32(212, 175, 55, 255);
    // (negScore for letting an egg hit the ground)
    private Color32 negScore = new Color32(200, 0, 0, 255);

    private GameObject scoreText;
    // static score variable for consistent value
    public static int score = 0;

    // 3 second delay
    private readonly float delay = 3f;

    void Start() {
        // finding objects in scene
        scoreText = GameObject.FindGameObjectWithTag("Score");
        canvas = GameObject.Find("Canvas").GetComponent<Transform>();
    }

    private void Update() {
        // determining correct position of floating scores per frame
        scorePos = PlayerMove.pos;
        scorePos.x += 0.75f;
        scorePos.y += 1.0f;
    }

    // for setting the current score
    public void SetScore(int potentialScore, GameObject scoreText) {
        // if you should gain score
        if (potentialScore == 25 || potentialScore == 100) {
            // add it
            score += potentialScore;
        // if you should lose score (an egg hit the ground)
        } else if (potentialScore == 50) {
            // remove it
            score -= potentialScore;
            // if removing score means total score is negative
            if (score < 0) {
                // keep score at 0
                score = 0;
            }
        }
        // get a floating +/- score text on screen
        SetFloatingScore(potentialScore);
        // update overall score on screen
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
    }

    // shows how much score you get or lose on screen, floats up and disappears
    public void SetFloatingScore(int potentialScore) {
        // positions a floating score above the overall score
        Vector3 scoreTextFloatPos = new Vector3(scoreText.transform.position.x + 3.25f, scoreText.transform.position.y + .4f, scoreText.transform.position.z);
        // if player got an egg
        if (potentialScore == 25) {
            // instantiate two floating scores, one on player and one above overall score
            newScore = Instantiate(scorePrefab, scorePos, Quaternion.identity);
            newScore2 = Instantiate(score2Prefab, scoreTextFloatPos, Quaternion.identity);

            // newScore is the floating score above the player position
            // places the floating score on the canvas so that it appears properly
            newScore.transform.SetParent(canvas);
            // the floating score prefab is a parent + child,
            // i don't actually remember why i made it that way, but it was necessary as a workaround to an issue!
            newScore.transform.GetChild(0).GetComponent<TextMesh>();

            // scales the floating text down in case the canvas blew it up
            newScore.transform.localScale = Vector3.one;
            // set text shown to +25
            newScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + potentialScore.ToString();
            // set text color to posScore
            newScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().faceColor = posScore;

            // newScore 2 is the floating score above the overall score
            newScore2.transform.SetParent(canvas);
            newScore2.transform.GetChild(0).GetComponent<TextMesh>();

            newScore2.transform.localScale = Vector3.one;
            newScore2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + potentialScore.ToString();
            // no need to set the color of the floating text above the overall score,
            // it uses a special font i made to match better with the text of the "overall score" UI element

        // if golden egg was caught
        } else if (potentialScore == 100) {
            newScore = Instantiate(scorePrefab, scorePos, Quaternion.identity);
            newScore2 = Instantiate(score2Prefab, scoreTextFloatPos, Quaternion.identity);

            newScore.transform.SetParent(canvas);
            newScore.transform.GetChild(0).GetComponent<TextMesh>();

            newScore.transform.localScale = Vector3.one;
            // will be a +100 over the player
            newScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + potentialScore.ToString();
            // with golden color text
            newScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().faceColor = goldScore;

            newScore2.transform.SetParent(canvas);
            newScore2.transform.GetChild(0).GetComponent<TextMesh>();

            newScore2.transform.localScale = Vector3.one;
            newScore2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + potentialScore.ToString();

        // if player let an egg hit the ground
        } else if (potentialScore == 50) {
            newScore = Instantiate(scorePrefab, eggPos, Quaternion.identity);
            newScore2 = Instantiate(score2Prefab, scoreTextFloatPos, Quaternion.identity);

            newScore.transform.SetParent(canvas);
            newScore.transform.GetChild(0).GetComponent<TextMesh>();

            newScore.transform.localScale = Vector3.one;
            // will show a -50
            newScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-" + potentialScore.ToString();
            // using the negScore color
            newScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().faceColor = negScore;

            newScore2.transform.SetParent(canvas);
            newScore2.transform.GetChild(0).GetComponent<TextMesh>();

            newScore2.transform.localScale = Vector3.one;
            newScore2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-" + potentialScore.ToString();
            // uses negScore to more obviously indicate on the overall score UI element that the player lost points
            newScore2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().faceColor = negScore;

        // this is mostly for my testing
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
        // destroy the floating score texts after their animations have happened
        Destroy(newScore, 1.0f);
        Destroy(newScore2, 1.0f);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        // if the egg collides with the basket
        if (col.gameObject.tag == "Basket") {
            // increase score by 25 if normal egg
            if (this.gameObject.tag == "Egg") {
                SetScore(25, scoreText);
            }
            // increase score by 100 if golden egg
            else if (this.gameObject.tag == "GoldenEgg") {
                SetScore(100, scoreText);
            }
            // destroy the egg after awarding points
            Destroy(this.gameObject);
        // if an egg hits the ground
        } else if (col.gameObject.tag == "Terrain") {
            // destroy egg and get position where it was destroyed
            eggPos = this.transform.position;
            // hide the egg, it shouldn't be destroyed yet as there is work to do still
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
        // waits for "delay" seconds
        yield return new WaitForSeconds(delay);
        // gets the renderer of the brokenEgg prefab instantiation
        Renderer renderer = brokenEgg.GetComponent<Renderer>();
        // gets the initial color of the brokenEgg prefab renderer
        Color newColor = renderer.material.color;

        // while the alpha (transparency) of the material color is not 0
        while (newColor.a > 0) {
            // decrease the alpha over time (delay) to fade out the broken egg
            newColor.a -= delay * Time.deltaTime;
            // set the new alpha
            renderer.material.color = newColor;
            // wait until while loop is completed
            yield return null;
        }

        // destroy the egg finally
        Destroy(this.gameObject);
    }
}
