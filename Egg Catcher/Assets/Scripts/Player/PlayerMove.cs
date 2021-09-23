using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// controls the basic player movement / qualities
public class PlayerMove : MonoBehaviour {
    public readonly static float speed = 10f;
    public static Vector2 pos;

    // initially the player is not exploded / game is ongoing
    public bool isExploded = false;

    void Update() {
        float h = Input.GetAxisRaw("Horizontal");

        pos = transform.position;
        Vector2 cameraPos = Camera.main.WorldToViewportPoint(transform.position);

        // if player moves offscreen, wrap them around to the other side of the screen
        // (i used worldtoviewportpoint and camera coordinates since they make more sense to me)
        if (cameraPos.x <= 0.0) {
            pos.x = -pos.x - 0.1f;
        } else if (cameraPos.x >= 1.0) {
            pos.x = -pos.x + 0.1f;
        }

        // player moving at normal speed
        pos.x += h * speed * Time.deltaTime;
        transform.position = pos;
    }
}
