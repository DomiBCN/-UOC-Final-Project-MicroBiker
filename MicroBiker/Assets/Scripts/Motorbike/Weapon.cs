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
    
    float shootTimer = 2;
    bool shooting;

    private void Awake()
    {
        halo = GetComponent<Light>();
    }

    private void FixedUpdate()
    {
        if (shootTimer < 1)
        {
            shootTimer += Time.fixedDeltaTime;
        }
        if (TouchInputManager.shoot && shootTimer >= timeBetweenShoots && !shooting)
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
