using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// one of the players two abilities, allows for a boost of speed temporarily
public class Ability_Boost : MonoBehaviour {
    // the speed boost is 125% of their normal move speed
    private readonly float speedMult = 1.25f;

    // was using headers for testing, the values were public
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
        // start EOFReset coroutine, which checks at the end of each frame to see if the player has moved
        // if the player isnt moving but is holding shift, i dont want to take away boost
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
        // update meters and timers every frame
        UpdateBoost();
        UpdateBoostTimer();
        UpdateBoostRefill();
        // as well as the transparency of the cooldown timer borders
        UpdateCDAlpha();
        float h = Input.GetAxisRaw("Horizontal");

        pos = transform.position;

        // set how fast the player should be moving while boosting
        boostSpeed = h * PlayerMove.speed * Time.deltaTime * speedMult;

        // if the player is pressing left shift, boost
        if (Input.GetKey(KeyCode.LeftShift)) {
            Boost();
            // reset refill timer 
            refillTimer = 0.0f;
        }

        // if the player is trying to boost but the ability is on cooldown
        if (Input.GetKey(KeyCode.LeftShift) && isOnBoostCD) {
            // plays a little animation that makes the text reading "boost" above the boost bar shake a little,
            // which feels pretty nice
            boostText.GetComponent<Animator>().Play("boostText");
        }

        // if the player is NOT trying to boost, AND they have used some of their boost, AND boost is NOT on cooldown
        if (!Input.GetKey(KeyCode.LeftShift) && usedBoost > 0.0f && !isOnBoostCD) {
            // if the refill timer is not full
            if (refillTimer < timeToStartRefill) {
                // increase it
                refillTimer += Time.deltaTime;
            // if it is full
            } else {
                // reduce usedBoost to 0 over 'timeToRefill' seconds
                // effectively "refill" the boost meter
                usedBoost -= (maxBoostTime / timeToRefill) * Time.deltaTime;
                // sort of clamps the boost bar from going beneath 0, doesn't matter that much though
                if (usedBoost < 0.0f) {
                    usedBoost = 0.0f;
                }
            }
        }
    }
    
    // updates how filled the boost bar is visually
    private void UpdateBoost() {
        boostProgressUI.fillAmount = (maxBoostTime - usedBoost) / maxBoostTime;
    }

    // the refill mechanic allows the player to "recharge" the boost meter 
    // IF they are not actively using the ability. this prevents situations
    // where the player has used, for example, 90% of their bosot meter,
    // and has to decide between conserving that last 10% and having it go
    // on cooldown, which doesn't feel great. i think cooldown should only
    // occur when the player burns through it all very quickly

    // updates how filled the refill bar is visually
    private void UpdateBoostRefill() {
        boostRefillProgressUI.fillAmount = refillTimer / timeToStartRefill;
    }

    // updates what percent of the cooldown timer circle is filled
    private void UpdateBoostRadial() {
        // if you are on cooldown
        if (cdProgress != 0.0f) {
            // update % filled
            boostCDRadial.fillAmount = cdProgress / boostCD;
        // if no longer on CD
        } else {
            // fully fill the cooldown radial
            boostCDRadial.fillAmount = 1.0f;
        }
    }

    // update the cooldown text over the radial
    private void UpdateBoostTimer() {
        // if you are on cd
        if (isOnBoostCD) {
            // if you are still progressing through CD
            if (cdProgress < boostCD) {
                // increase progression
                cdProgress += Time.deltaTime;
                // if progression exceeds CD
                if (cdProgress >= boostCD) {
                    // you are no long on CD
                    cdProgress = boostCD;
                    isOnBoostCD = false;
                }
                // updates text (F1 is regex for 1 decimal point float)
                boostCDText.text = (boostCD - cdProgress).ToString("F1");
                // update the CD radial
                UpdateBoostRadial();
            }
        // if you are not on CD, hide the text over the radial (even though its technically 60seconds)
        } else {
            boostCDText.text = null;
        }
    }

    // updates the transparency of the border surrounding the radial
    private void UpdateCDAlpha() {
        // gradient using alpha = 1 when usedBoost = 0f, alpha = 0 when usedBoost = maxBoostTime
        boostOffCDCG.alpha = 1 - (usedBoost / maxBoostTime);
        boostOnCDCG.alpha = usedBoost;
    }

    // the actual boost ability function
    private void Boost() {
        // if you are not on CD and you are not refilling (refilling meaning you just came off CD and the bar is refilling)
        if (!isOnBoostCD && !isRefilling) {
            // if you havent used all boost
            if (usedBoost <= maxBoostTime) {
                // if you are moving
                if (transform.hasChanged) {
                    // then update how much boost youve used
                    usedBoost += Time.deltaTime;
                    // and speed boost
                    pos.x += boostSpeed;
                }
                // update position
                transform.position = pos;
            // if you use all boost
            } else {
                // start boost CD
                isOnBoostCD = true;
                StartCoroutine(BoostCD());
            }
        }
    }

    // boost cooldown coroutine
    private IEnumerator BoostCD() {
        // waits for the boost cooldown to end
        yield return new WaitForSeconds(boostCD);
        // reset cd progress to 0
        cdProgress = 0.0f;
        // while boost bar is refilling
        while (usedBoost > 0.0f) {
            // you are refilling
            isRefilling = true;
            // reset the refill timer to 0
            refillTimer = 0.0f;
            // decrease the amount of boost youve used slowly (so refill slowly)
            usedBoost -= (maxBoostTime / (timeToRefill / 2)) * Time.deltaTime;
            // when refilled
            if (usedBoost <= 0.0f) {
                // take off boost CD
                usedBoost = 0.0f;
                isOnBoostCD = false;
            }
            // update boost bar
            UpdateBoost();
            yield return null;
        }
        // no longer refilling once bar is filled
        isRefilling = false;
    }
    
    //coroutine for checking if player is moving
    private IEnumerator EOFReset() {
        while (true) {
            // at end of current frame
            yield return new WaitForEndOfFrame();
            // see if player is moving
            if (Input.GetAxisRaw("Horizontal") == 0) {
                // if not, set haschanged to false
                transform.hasChanged = false;
            }
        }
    }
}
