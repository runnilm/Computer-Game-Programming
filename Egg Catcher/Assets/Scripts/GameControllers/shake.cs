using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// simple class to globally allow the camera to shake when shake 
// class instantiation .CamShake() is called,
// not really necessarily at all but I thought I might be using
// the animation more often than just the one script
public class shake : MonoBehaviour {
    public Animator shakeAnim;
    public void CamShake() {
        shakeAnim.SetTrigger("shakeCam");
    }
}
