using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ability_Boost : MonoBehaviour {
    private readonly float speedMult = 1.25f;

    [Header("Boost Main Parameters")]
    // a short boost of speed
    public float usedBoost = 0.0f;  // how much time boost used
    private readonly float boostCD = 10.0f; // how long it takes to be able to use again
    private readonly float maxBoostTime = 0.5f; // how long the ability can be used
    private bool isOnBoostCD = false;    // if ability can be used
    private float cdProgress = 0.0f;
    private float boostSpeed;
    private Vector2 pos;

    [Header("Refill Main Parameters")]
    public float refillTimer = 0.0f;    // determine how much time has passed since boost was used last
    private readonly float timeToStartRefill = 5.0f; // if that value exceeds 5 seconds, start "refilling" boost
    private readonly float timeToRefill = 2.5f;
    private bool isRefilling = false;

    [Header("Boost UI Elements")]
    private Image boostProgressUI;
    private Image boostCDRadial;
    private TextMeshProUGUI boostCDText;
    private GameObject boostText;

    [Header("Refill UI Elements")]
    private Image boostRefillProgressUI;

    [Header("BoostCD Border UI Elements")]
    private CanvasGroup boostOnCDCG;
    private CanvasGroup boostOffCDCG;

    void Start() {
        StartCoroutine(EOFReset());

        boostProgressUI = GameObject.Find("BoostBar").GetComponent<Image>();
        boostCDRadial = GameObject.Find("BoostTimer").GetComponent<Image>();
        boostCDText = GameObject.Find("BoostCD").GetComponent<TextMeshProUGUI>();
        boostText = GameObject.Find("BoostText");

        boostRefillProgressUI = GameObject.Find("BoostRefillBar").GetComponent<Image>();

        boostOnCDCG = GameObject.Find("BOnCD_CanvasGroup").GetComponent<CanvasGroup>();
        boostOffCDCG = GameObject.Find("BOffCD_CanvasGroup").GetComponent<CanvasGroup>();
    }

    void Update() {
        UpdateBoost();
        UpdateBoostTimer();
        UpdateBoostRefill();
        UpdateCDAlpha();
        float h = Input.GetAxisRaw("Horizontal");

        pos = transform.position;

        boostSpeed = h * PlayerMove.speed * Time.deltaTime * speedMult;

        if (Input.GetKey(KeyCode.LeftShift)) {
            Boost();
            refillTimer = 0.0f;
        }

        if (Input.GetKey(KeyCode.LeftShift) && isOnBoostCD) {
            boostText.GetComponent<Animator>().Play("boostText");
        }

        if (!Input.GetKey(KeyCode.LeftShift) && usedBoost > 0.0f && !isOnBoostCD) {
            if (refillTimer < timeToStartRefill) {
                refillTimer += Time.deltaTime;
            } else {
                //reduce usedBoost to 0 over 'timeToRefill' seconds
                usedBoost -= (maxBoostTime / timeToRefill) * Time.deltaTime;
                if (usedBoost < 0.0f) {
                    usedBoost = 0.0f;
                }
            }
        }
    }
    
    private void UpdateBoost() {
        boostProgressUI.fillAmount = (maxBoostTime - usedBoost) / maxBoostTime;
    }

    private void UpdateBoostRefill() {
        boostRefillProgressUI.fillAmount = refillTimer / timeToStartRefill;
    }

    private void UpdateBoostRadial() {
        if (cdProgress != 0.0f) {
            boostCDRadial.fillAmount = cdProgress / boostCD;
        } else {
            boostCDRadial.fillAmount = 1.0f;
        }
    }

    private void UpdateBoostTimer() {
        if (isOnBoostCD) {
            if (cdProgress < boostCD) {
                cdProgress += Time.deltaTime;
                if (cdProgress > boostCD) {
                    cdProgress = boostCD;
                    isOnBoostCD = false;
                }
                boostCDText.text = (boostCD - cdProgress).ToString("F1");
                UpdateBoostRadial();
            }
        } else {
            boostCDText.text = null;
        }
    }

    private void UpdateCDAlpha() {
        // gradient using alpha = 1 when usedBoost = 0f, alpha = 0 when usedBoost = maxBoostTime
        boostOffCDCG.alpha = 1 - (usedBoost / maxBoostTime);
        boostOnCDCG.alpha = usedBoost;
    }

    private void Boost() {
        if (!isOnBoostCD && !isRefilling) {
            if (usedBoost <= maxBoostTime) {
                if (transform.hasChanged) {
                    usedBoost += Time.deltaTime;
                    pos.x += boostSpeed;
                }
                transform.position = pos;
            } else {
                isOnBoostCD = true;
                StartCoroutine(BoostCD());
            }
        }
    }

    private IEnumerator BoostCD() {
        yield return new WaitForSeconds(boostCD);
        cdProgress = 0.0f;
        while (usedBoost > 0.0f) {
            isRefilling = true;
            refillTimer = 0.0f;
            usedBoost -= (maxBoostTime / (timeToRefill / 2)) * Time.deltaTime;
            if (usedBoost <= 0.0f) {
                usedBoost = 0.0f;
                isOnBoostCD = false;
            }
            UpdateBoost();
            yield return null;
        }
        isRefilling = false;
    }
    
    private IEnumerator EOFReset() {
        while (true) {
            yield return new WaitForEndOfFrame();
            if (Input.GetAxisRaw("Horizontal") == 0) {
                transform.hasChanged = false;
            }
        }
    }
}
