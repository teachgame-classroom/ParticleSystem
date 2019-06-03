using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleControl : MonoBehaviour
{
    private ParticleSystem ps;
    private float startSpeed = 25;


    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(ps.isPlaying)
            {
                ps.Stop();
            }
            else if(ps.isStopped)
            {
                ps.Play();
            }
        }

        if(Input.GetKey(KeyCode.W))
        {
            startSpeed += Time.deltaTime * 10;

            ParticleSystem.MainModule main = ps.main;
            main.startSpeed = startSpeed;

            ParticleSystem.EmissionModule em = ps.emission;
            em.rateOverTime = startSpeed * startSpeed;
        }


        if (Input.GetKey(KeyCode.S))
        {
            startSpeed -= Time.deltaTime * 10;

            ParticleSystem.MainModule main = ps.main;
            main.startSpeed = startSpeed;

            ParticleSystem.EmissionModule em = ps.emission;
            em.rateOverTime = startSpeed * startSpeed;
        }
    }

    void OnParticleSystemStopped()
    {
        Debug.Log("粒子系统停止");
    }
}
