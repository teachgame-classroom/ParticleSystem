using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCurveTest : MonoBehaviour
{
    public int weaponId = 0;

    public Gradient gradient;
    private float colorValue;

    public AnimationCurve rotateCurve;
    public AnimationCurve recoilCurve;
    public AnimationCurve laserCurve;

    public AudioClip gatlingSound;
    public AudioClip smgSound;

    private Transform gatlingTrans;
    private Transform gatlingMuzzle;

    private Transform smgTrans;
    private Transform smgMuzzle;
    private Transform laserFlare;

    private Vector3 smgOriginPos;

    private float t;

    private float gatlingInterval = 0.05f;
    private float smgInterval = 0.1f;

    private float lastFireTime;

    // Start is called before the first frame update
    void Start()
    {
        gatlingTrans = transform.Find("Gatling");
        gatlingMuzzle = gatlingTrans.Find("muzzle");

        smgTrans = transform.Find("SMG");
        smgMuzzle = smgTrans.Find("muzzle");

        laserFlare = smgTrans.Find("flare");

        smgOriginPos = smgTrans.position;
    }

    // Update is called once per frame
    void Update()
    {
        colorValue += Time.deltaTime / 5;
        colorValue = Mathf.Repeat(colorValue, 1);

        Color color = gradient.Evaluate(colorValue);

        transform.Find("Cube").GetComponent<MeshRenderer>().material.SetColor("_Color", color);

        if(weaponId == 0)
        {
            if (Input.GetMouseButton(0))
            {
                t += Time.deltaTime / 5;

                if (t > 0.75f)
                {
                    ShootGatling();
                }
            }
            else
            {
                t -= Time.deltaTime / 5;
            }

            t = Mathf.Clamp01(t);
            float angularVelocity = rotateCurve.Evaluate(t) * 360 * 2;

            gatlingTrans.Rotate(0, 0, angularVelocity * Time.deltaTime, Space.Self);
        }

        if(weaponId == 1)
        {
            if(Input.GetMouseButton(0))
            {
                ShootSMG();
            }
        }

        if(weaponId == 2)
        {
            ShootLaser();
        }
    }

    void ShootGatling()
    {
        if(Time.time - lastFireTime > gatlingInterval)
        {
            AudioSource.PlayClipAtPoint(gatlingSound, transform.position);
            gatlingMuzzle.gameObject.SetActive(true);
            lastFireTime = Time.time;
            Invoke("HideMuzzle", gatlingInterval / 2);
        }
    }

    void ShootSMG()
    {
        if (Time.time - lastFireTime > smgInterval)
        {
            AudioSource.PlayClipAtPoint(smgSound, transform.position);
            smgMuzzle.gameObject.SetActive(true);
            lastFireTime = Time.time;

            StartCoroutine(RecoilCoroutine());

            Invoke("HideMuzzle", smgInterval / 2);
        }
    }

    void ShootLaser()
    {
        if(Input.GetMouseButton(0))
        {
            t += Time.deltaTime * 5f;
        }
        else
        {
            t -= Time.deltaTime * 5f;
        }

        if(t > 0.1f)
        {
            laserFlare.gameObject.SetActive(true);
            GetComponent<AudioSource>().enabled = true;
        }
        else
        {
            laserFlare.gameObject.SetActive(false);
            GetComponent<AudioSource>().enabled = false;
        }

        t = Mathf.Clamp01(t);

        float f = laserCurve.Evaluate(t);
        smgTrans.position = smgOriginPos - transform.forward * f * 0.5f;

    }

    IEnumerator RecoilCoroutine()
    {
        float f = 0;

        while(f < 1)
        {
            f += Time.deltaTime * 10f;
            f = Mathf.Clamp01(f);
            float recoil = recoilCurve.Evaluate(f);
            smgTrans.position = smgOriginPos - transform.forward * recoil * 0.3f;
            yield return null;
        }
    }

    void HideMuzzle()
    {
        gatlingMuzzle.gameObject.SetActive(false);
        smgMuzzle.gameObject.SetActive(false);
    }
}
