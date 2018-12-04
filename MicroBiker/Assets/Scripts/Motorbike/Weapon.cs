using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Light))]
public class Weapon : MonoBehaviour
{

    public GameObject laserPrefab;
    public float timeBetweenShoots = 1f;
    public float haloMaxRange = 0.54f;
    public float haloExpansionIncrements = 0.02f;
    public float haloIncrementWait = 0.05f;

    Light halo;
    WaitForSeconds haloWait;

    //Screen touch zones reference points
    float screenSplit;
    float screenSectionShootStart;
    float screenSectionShootEnd;

    float shootTimer = 2;
    bool shoot;
    bool shooting;

    private void Awake()
    {
        screenSplit = Screen.width / 4;
        screenSectionShootStart = screenSplit;
        screenSectionShootEnd = screenSplit * 3;
        halo = GetComponent<Light>();
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch[] touches = Input.touches;
            if (touches.Length > 0)
            {
                foreach (var myTouch in touches)
                {
                    //Check if shoot zone pressed
                    if (myTouch.position.x > screenSectionShootStart && myTouch.position.x < screenSectionShootEnd && myTouch.phase != TouchPhase.Ended && myTouch.phase != TouchPhase.Canceled)
                    {
                        shoot = true;
                    }
                    else if (myTouch.position.x > screenSectionShootStart && myTouch.position.x < screenSectionShootEnd && (myTouch.phase == TouchPhase.Ended || myTouch.phase == TouchPhase.Canceled))
                    {
                        shoot = false;
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (shootTimer < 1)
        {
            shootTimer += Time.fixedDeltaTime;
        }
        if (shoot && shootTimer >= timeBetweenShoots && !shooting)
        {
            //shoot
            shooting = true;
            StartCoroutine(ShootLaser());
            shootTimer = 0;
        }
    }

    IEnumerator ShootLaser()
    {
        while (halo.range < haloMaxRange)
        {
            halo.range += haloExpansionIncrements;
            yield return new WaitForSeconds(haloIncrementWait);
        }
        Instantiate(laserPrefab, transform.position, transform.rotation, transform);
        yield return new WaitForSeconds(haloIncrementWait);
        while (halo.range > 0)
        {
            halo.range -= haloExpansionIncrements;
            yield return new WaitForSeconds(haloIncrementWait);
        }
        halo.range = 0;
        shooting = false;
    }
}
