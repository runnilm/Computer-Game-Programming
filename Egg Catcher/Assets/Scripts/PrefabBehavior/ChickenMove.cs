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

        randStart = Random.Range(0.0f, 1.0f);

        if (randStart <= 0.50f) {
            moveRight = false;
            moveLeft = true;
        } else {
            moveRight = true;
            moveLeft = false;
        }
    }

    void Update() {
        Vector2 chickenPos = transform.position;

        if (Time.time > timeDelay + lastIncreased) {
            lastIncreased = Time.time;
            speed += speedModifier;
        }
        if (moveRight) {
            if (chickenPos.x < xStoppingPoint) {
                chickenPos.x += Time.deltaTime * speed;
            } else {
                moveRight = false;
                moveLeft = true;
            }
        }

        if (moveLeft) {
            if (chickenPos.x > -xStoppingPoint) {
                chickenPos.x -= Time.deltaTime * speed;
            } else {
                moveRight = true;
                moveLeft = false;
            }
        }
        transform.position = chickenPos;
    }
}
