using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAppearFromCenterManager : MonoBehaviour {

    float completion = 0;

    float timeStill = 1.5f;
    float timeStillOut = 2.8f;
    float timer = 0f;

    int bulletsToFire = 3;
    int nShot = 0;
    float[] timesToShoot;

    SmoothProgressor progressor;
    SmoothProgressor progressorOut;
    Material material;

    void Start() {
        progressor = new SmoothProgressor();
        progressorOut = new SmoothProgressor();

        timesToShoot = new float[bulletsToFire];
        for (int i = 0; i < bulletsToFire; i++) {
            timesToShoot[i] = (timeStill / (bulletsToFire + 1)) * (i + 1);
        }

        this.StartFadeIn();

        material = gameObject.GetComponent<SpriteRenderer>().material;
    }

    void StartFadeIn() {
        nShot = 0;
        progressor.StartTransition(0, 1.1f, 30);
        progressor.InvertSlope();
    }

    void StartFadeOut() {
        progressorOut.StartTransition(0, -1.1f, 30);
    }

    bool IsFadedOut() {
        return completion <= 0.1f;
    }

    void FixedUpdate() {
        if (progressor.active) {
            completion += progressor.Progress();

            material.SetFloat("_Completion", completion);
        } else if (!progressorOut.active) {
            timer += Time.fixedDeltaTime;

            //waiting to appear
            if (this.IsFadedOut()) {
                if (timer >= timeStillOut) {
                    timer = 0;
                    StartFadeIn();
                }
                //waiting to disappear / shooting
            } else {

                for (int i = nShot; i < timesToShoot.Length; i++) {
                    if (timer >= timesToShoot[i]) {

                        BulletCreator.FromTo(
                            gameObject.transform.position,
                            GameObject.Find("Player").transform.position,
                            1,
                            ShootingPatterns.One,
                            false,
                            5f,
                            true
                        );

                        nShot++;
                    }
                }

                if (timer >= timeStill) {
                    timer = 0;
                    StartFadeOut();
                }
            }
        } else {
            completion += progressorOut.Progress();

            material.SetFloat("_Completion", completion);

            //finished cycle
            if (!progressorOut.active) {
                transform.position = PositionGenerator.NearAndInWater(GameObject.Find("Player").transform.position);
            }
        }
    }
}
