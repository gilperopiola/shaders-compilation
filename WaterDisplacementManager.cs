using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDisplacementManager : MonoBehaviour {

    float displacement = 0f;
    float step = 0.0032f;

    int status = 0; //0 = rising; 1 = waiting; 2 = descending; 3 = waiting;
    float timer = 0;

    float minValue = -0.22f;
    float waitingTime1 = 0.08f;
    float waitingTime2 = 0.09f;

    Material waterDisplacementMat;

    void Start() {
        waterDisplacementMat = (Material)Resources.Load("Materials/Water Displacement", typeof(Material));
    }

    void FixedUpdate() {

        if (status == 0) {
            displacement -= step;

            if (displacement <= minValue) {
                status = 1;
                timer = 0;
            }
        }

        if (status == 1) {
            timer += step;

            if (timer >= waitingTime1) {
                status = 2;
                timer = 0;
            }
        }

        if (status == 2) {
            displacement += step;

            if (displacement >= 0) {
                status = 3;
                timer = 0;
            }
        }

        if (status == 3) {
            timer += step;

            if (timer >= waitingTime2) {
                status = 0;
                timer = 0;
            }
        }

        waterDisplacementMat.SetFloat("_DisplacementY", displacement);
    }
}
