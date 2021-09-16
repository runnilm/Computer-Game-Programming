using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Permeability : MonoBehaviour {
    //permeability - can't collect eggs or be hit by bombs
    public float usedPerm = 0.0f;  // how much time permeability used
    public float permCD = 15.0f;   // how long it takes to be able to use again
    public float maxPermTime = 1.0f;   // how long the ability can be used
    public bool isOnPermCD = false; // if ability can be used

    private Color32 transparency;
    private Color32 noTransparency;

    // Start is called before the first frame update
    void Start() {
        noTransparency = this.GetComponent<SpriteRenderer>().color;
        transparency = new Color32(noTransparency.r, noTransparency.g, noTransparency.b, 123);
        isOnPermCD = false;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.Space)) {
            Perm();
        } else {
            this.GetComponent<EdgeCollider2D>().enabled = true;
            this.GetComponent<SpriteRenderer>().color = noTransparency;
        }
    }

    private void Perm() {
        if (!isOnPermCD) {
            if (usedPerm < maxPermTime) {
                usedPerm += Time.deltaTime;

                this.GetComponent<EdgeCollider2D>().enabled = false;
                this.GetComponent<SpriteRenderer>().color = transparency;
            } else {
                this.GetComponent<EdgeCollider2D>().enabled = true;
                this.GetComponent<SpriteRenderer>().color = noTransparency;
                isOnPermCD = true;

                StartCoroutine(PermCD());
            }
        }
    }

    private IEnumerator PermCD() {
        yield return new WaitForSeconds(permCD);
        isOnPermCD = false;
        usedPerm = 0.0f;
    }
}
