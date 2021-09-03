using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 5.0f;

    // Start is called before the first frame update
    void Start() {
        Vector2 pos = transform.position;

        pos.x = 0.0f;
        pos.y = -4.5f;

        transform.position = pos;
    }

    // Update is called once per frame
    void Update() {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 pos = transform.position;
        pos.x += h * speed * .001f;
        pos.y += v * speed * .001f;

        transform.position = pos;
    }
}
