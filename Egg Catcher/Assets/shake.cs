using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shake : MonoBehaviour {
    public Animator shakeAnim;
    public void CamShake() {
        shakeAnim.SetTrigger("shakeCam");
    }
}
