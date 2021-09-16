using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Boost : MonoBehaviour {
    private readonly float speedMult = 1.5f;

    // a short boost of speed
    public float usedBoost = 0.0f;  // how much time boost used
    private readonly float boostCD = 10.0f; // how long it takes to be able to use again
    private readonly float maxBoostTime = 0.5f; // how long the ability can be used
    public bool isOnBoostCD = false;    // if ability can be used

    public float refillTimer = 0.0f;    // determine how much time has passed since boost was used last
    private readonly float timeToStartRefill = 5.0f; // if that value exceeds 5 seconds, start "refilling" boost

    Vector2 pos;

    private float boostSpeed;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() {
        float h = Input.GetAxisRaw("Horizontal");

        pos = transform.position;

        boostSpeed = h * PlayerMove.speed * Time.deltaTime * speedMult;

        if (Input.GetKey(KeyCode.LeftShift)) {
            Boost();
            refillTimer = 0.0f;
        }

        if (!Input.GetKey(KeyCode.LeftShift) && usedBoost > 0.0f && !isOnBoostCD) {
            if (refillTimer < timeToStartRefill) {
                refillTimer += Time.deltaTime;
            } else {
                while (usedBoost > 0.0f) {
                    ///// TODO SLOWLY REDUCE USED BOOST UNTIL 0 /////////
                    usedBoost -= Time.deltaTime;
                }
                refillTimer = 0.0f;
            }
        }
    }

    private void Boost() {
        if (!isOnBoostCD) {
            if (usedBoost < maxBoostTime) {
                usedBoost += Time.deltaTime;

                pos.x += boostSpeed;
                transform.position = pos;
            } else {
                isOnBoostCD = true;

                StartCoroutine(BoostCD());
            }
        }
    }

    private IEnumerator BoostCD() {
        yield return new WaitForSeconds(boostCD);
        isOnBoostCD = false;
        usedBoost = 0.0f;
    }
}
