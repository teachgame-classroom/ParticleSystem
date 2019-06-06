using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleControl : MonoBehaviour
{
    public HoleParticleControl holeParticleControl;

    private ParticleSystem ps;
    private float startSpeed = 25;

    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    private List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>(500);
    private List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>(500);

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ParticleSystem.ForceOverLifetimeModule force = ps.forceOverLifetime;

        force.xMultiplier = 5;
        force.zMultiplier = 5;
        force.yMultiplier = 50;

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

    // 发生粒子碰撞时会被自动调用的方法
    void OnParticleCollision(GameObject other)
    {
        // 获取粒子碰撞信息
        ParticlePhysicsExtensions.GetCollisionEvents(ps, other, collisionEvents);

        // 将粒子碰撞信息发送给显示弹孔的粒子系统
        holeParticleControl.ReceiveCollisionEvent(collisionEvents);

        //Debug.Log("粒子撞上了" + other.name);
    }

    void OnParticleTrigger()
    {
        int enterCount = ParticlePhysicsExtensions.GetTriggerParticles(ps, ParticleSystemTriggerEventType.Enter, enter);
        int exitCount = ParticlePhysicsExtensions.GetTriggerParticles(ps, ParticleSystemTriggerEventType.Exit, exit);

        Debug.Log("Trigger,Enter:" + enterCount + ",Exit:" + exitCount);
        Debug.Log("EnterList:" + enter.Count + ",ExitList:" + exit.Count);

        for (int i = 0; i < enter.Count; i++)
        {
            ParticleSystem.Particle p = enter[i];
            p.startColor = Color.green;
            enter[i] = p;
        }

        for (int i = 0; i < exit.Count; i++)
        {
            ParticleSystem.Particle p = exit[i];
            p.startColor = Color.red;
            exit[i] = p;
        }

        ParticlePhysicsExtensions.SetTriggerParticles(ps, ParticleSystemTriggerEventType.Enter, enter);
        ParticlePhysicsExtensions.SetTriggerParticles(ps, ParticleSystemTriggerEventType.Exit, exit);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        foreach(ParticleCollisionEvent e in collisionEvents)
        {
            //Gizmos.DrawSphere(e.intersection, 1f);
        }
    }
}
