using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// EVERYTHING IN THIS CLASS IS ESSENTIALLY THE SAME FUNCTIONALLY AS ABILITY_BOOST.CS, I AM ONLY COMMENTING BIG DIFFERENCES
public class Ability_Permeability : MonoBehaviour {

    [Header("Perm Main Parameters")]
    //permeability - can't collect eggs or be hit by bombs
    private float usedPerm = 0.0f;  // how much time permeability used
    private float permCD = 15.0f;   // how long it takes to be able to use again
    private float maxPermTime = 0.25f;   // how long the ability can be used .25f
    private bool isOnPermCD = false; // if ability can be used
    public bool isPerm = false;
    private float permCDProgress = 0.0f;
    public GameObject bombColPrefab;
    private Color32 transparency;
    private Color32 noTransparency;

    [Header("Refill Main Parameters")]
    private float refillTimer = 0.0f;    // determine how much time has passed since boost was used last
    private readonly float timeToStartRefill = 10.0f; // if that value exceeds 5 seconds, start "refilling" boost
    private readonly float timeToRefill = 5.0f;
    private bool isRefilling = false;

    [Header("Perm UI Elements")]
    private Image permProgressUI;
    private Image permCDRadial;
    private TextMeshProUGUI permCDText;
    private GameObject permText;

    [Header("Refill UI Elements")]
    private Image permRefillProgressUI;

    [Header("PermCD Border UI Elements")]
    private CanvasGroup permOnCDCG;
    private CanvasGroup permOffCDCG;

    void Start() {
        permProgressUI = GameObject.Find("PermBar").GetComponent<Image>();
        permCDRadial = GameObject.Find("PermTimer").GetComponent<Image>();
        permCDText = GameObject.Find("PermCD").GetComponent<TextMeshProUGUI>();
        permText = GameObject.Find("PermText");

        permRefillProgressUI = GameObject.Find("PermRefillBar").GetComponent<Image>();

        permOnCDCG = GameObject.Find("POnCD_CanvasGroup").GetComponent<CanvasGroup>();
        permOffCDCG = GameObject.Find("POffCD_CanvasGroup").GetComponent<CanvasGroup>();

        noTransparency = this.GetComponent<SpriteRenderer>().color;
        transparency = new Color32(noTransparency.r, noTransparency.g, noTransparency.b, 123);
        isOnPermCD = false;
    }

    void Update() {
        UpdatePerm();
        UpdatePermTimer();
        UpdatePermRefill();
        UpdateCDAlpha();

        if (Input.GetKey(KeyCode.Space)) {
            Perm();
            refillTimer = 0.0f;
        } else {
            // if player isn't using permeability, their sprite has no transparency and the basket collider is active (they can be hit)
            this.GetComponent<EdgeCollider2D>().enabled = true;
            this.GetComponent<SpriteRenderer>().color = noTransparency;
        }

        if (Input.GetKey(KeyCode.Space) && isOnPermCD) {
            permText.GetComponent<Animator>().Play("permText");
        }

        if (!Input.GetKey(KeyCode.Space) && usedPerm > 0.0f && !isOnPermCD) {
            if (refillTimer < timeToStartRefill) {
                refillTimer += Time.deltaTime;
            } else {
                //reduce usedBoost to 0 over 'timeToRefill' seconds
                usedPerm -= (maxPermTime / timeToRefill) * Time.deltaTime;
                if (usedPerm <= 0.0f) {
                    usedPerm = 0.0f;
                }
            }
        }
    }

    private void UpdatePerm() {
        permProgressUI.fillAmount = (maxPermTime - usedPerm) / maxPermTime;
    }

    private void UpdatePermRefill() {
        permRefillProgressUI.fillAmount = refillTimer / timeToStartRefill;
    }

    private void UpdatePermRadial() {
        if (permCDProgress != 0.0f) {
            permCDRadial.fillAmount = permCDProgress / permCD;
        }
        else {
            permCDRadial.fillAmount = 1.0f;
        }
    }

    private void UpdatePermTimer() {
        if (isOnPermCD) {
            if (permCDProgress < permCD) {
                permCDProgress += Time.deltaTime;
                if (permCDProgress > permCD) {
                    permCDProgress = permCD;
                    isOnPermCD = false;
                }
                permCDText.text = (permCD - permCDProgress).ToString("F1");
                UpdatePermRadial();
            }
        } else {
            permCDText.text = null;
        }
    }

    private void UpdateCDAlpha() {
        // alpha = 1 when usedPerm = 0f, alpha = 0 when usedPerm = maxPermTime
        permOffCDCG.alpha = 1- (usedPerm / maxPermTime);
        permOnCDCG.alpha = usedPerm;
    }

    private void Perm() {
        if (!isOnPermCD && !isRefilling) {
            if (usedPerm <= maxPermTime) {
                isPerm = true;
                usedPerm += Time.deltaTime;

                // while using permeability, the player cannot get hit by bombs (but CAN grab eggs still)
                Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), bombColPrefab.GetComponent<Collider2D>());

                // lower the player sprite's transparency by half while using ability
                this.GetComponent<SpriteRenderer>().color = transparency;
            } else {
                isPerm = false;
                this.GetComponent<EdgeCollider2D>().enabled = true;
                this.GetComponent<SpriteRenderer>().color = noTransparency;
               
                isOnPermCD = true;
                StartCoroutine(PermCD());
            }
        }
    }

    private IEnumerator PermCD() {
        yield return new WaitForSeconds(permCD);
        permCDProgress = 0.0f;
        while (usedPerm > 0.0f) {
            isRefilling = true;
            refillTimer = 0.0f;
            usedPerm -= (maxPermTime / (timeToRefill / 2)) * Time.deltaTime;
            if (usedPerm <= 0.0f) {
                usedPerm = 0.0f;
                isOnPermCD = false;
            }
            UpdatePerm();
            yield return null;
        }
        isRefilling = false;
    }
}
