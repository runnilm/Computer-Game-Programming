using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class is necessary to reset the overall score, which is a static variable, to 0.
public class ResetScore : MonoBehaviour{
    void Start(){
        EggDestroy.score = 0;
    }
}
