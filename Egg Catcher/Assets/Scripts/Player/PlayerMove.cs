using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public readonly static float speed = 10f;

    private float normalSpeed;

    public bool isExploded = false;

    void Start() { }

    // Update is called once per frame
    void Update() {
        float h = Input.GetAxisRaw("Horizontal");

        Vector2 pos = transform.position;
        Vector2 cameraPos = Camera.main.WorldToViewportPoint(transform.position);

        if (cameraPos.x <= 0.0) {
            pos.x = -pos.x - 0.1f;
        } else if (cameraPos.x >= 1.0) {
            pos.x = -pos.x + 0.1f;
        }

        pos.x += h * speed * Time.deltaTime;
        transform.position = pos;
    }
}
