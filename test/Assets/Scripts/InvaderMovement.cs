using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderMovement : MonoBehaviour {
    public float speed = 5.0f;
    public int speedMult = 2;
    public float xStoppingPoint = 6.5f;
    public float yStoppingPoint = 1.75f;
    public bool moveRight;
    public bool moveLeft;
    public bool dropEvent;

    // Start is called before the first frame update
    void Start() {
        Vector2 pos = transform.position;

        pos.x = Random.Range(-5.0f, 5.0f);
        pos.y = Random.Range(0.0f, 5.0f);

        transform.position = pos;
    }

    // Update is called once per frame
    void FixedUpdate() {
        Vector2 pos = transform.position;

        if (moveRight) {
            if (pos.x < xStoppingPoint) {
                pos.x += Time.deltaTime * speed * speedMult;
            } else {
                moveRight = false;
                moveLeft = true;
                dropEvent = true;
            }
        }

        if (moveLeft) {
            if (pos.x > -xStoppingPoint) {
                pos.x -= Time.deltaTime * speed * speedMult;
            } else {
                moveRight = true;
                moveLeft = false;
                dropEvent = true;
            }
        }

        if (dropEvent) {
            if (pos.y > -yStoppingPoint) {
                pos.y -= 0.5f;
                dropEvent = false;
            } else {
                Time.timeScale = 0;
                dropEvent = false;
            }
        }

        transform.position = pos;
    }
}
