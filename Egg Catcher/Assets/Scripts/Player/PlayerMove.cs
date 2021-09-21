using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public readonly static float speed = 10f;

    public static Vector2 pos;

    public bool isExploded = false;

    void Update() {
        float h = Input.GetAxisRaw("Horizontal");

        pos = transform.position;
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
