using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMove : MonoBehaviour {
    public float speed;

    private bool moveRight;
    private bool moveLeft;
    private float randStart;

    private readonly float speedModifier = 1.5f;
    private float lastIncreased;
    private readonly float timeDelay = 10.0f;

    private readonly float xStoppingPoint = 9.0f;

    void Start() {
        Vector2 chickenPos = transform.position;

        speed = 6.0f;
        chickenPos.x = 0.0f;
        chickenPos.y = 4.5f;

        transform.position = chickenPos;

        // randomly selects whether the chicken initially moves left or right
        randStart = Random.Range(0.0f, 1.0f);

        if (randStart <= 0.50f) {
            moveRight = false;
            moveLeft = true;
        } else {
            moveRight = true;
            moveLeft = false;
        }
    }

    // moves the chicken across the top of the screen
    void Update() {
        Vector2 chickenPos = transform.position;

        // every 10 seconds, the chicken begins moving faster 
        if (Time.time > timeDelay + lastIncreased) {
            lastIncreased = Time.time;
            speed += speedModifier;
        }
        // if chicken is moving right
        if (moveRight) {
            // while chicken hasnt reached end of screen (right side)
            if (chickenPos.x < xStoppingPoint) {
                // keep moving right
                chickenPos.x += Time.deltaTime * speed;
            // if the chicken reaches the end of the screen (right side)
            } else {
                // send it back in the opposite direction (Left)
                moveRight = false;
                moveLeft = true;
            }
        }

        // if chicken is moving left
        if (moveLeft) {
            // if chicken hasnt reached end of screen (left side)
            if (chickenPos.x > -xStoppingPoint) {
                // keep moving left
                chickenPos.x -= Time.deltaTime * speed;
            // if the chicken reaches the end of the screen (left side)
            } else {
                // send it back in the opposite direction (right)
                moveRight = true;
                moveLeft = false;
            }
        }
        // update position
        transform.position = chickenPos;
    }
}
