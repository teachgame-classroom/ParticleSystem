using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleParticleControl : MonoBehaviour
{
    private ParticleSystem ps;
    ParticleSystem.Particle[] particles;

    private int idx = 0;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[500];
    }

    // Update is called once per frame
    void Update()
    {
        // 在Update将粒子数组写入粒子系统，则粒子状态会保持与数组一致，如果数组中的粒子的生命周期保持不变，粒子就会一直存在
        ps.SetParticles(particles);
    }

    // 接收其他粒子系统的碰撞信息，在碰撞位置上显示弹孔粒子
    public void ReceiveCollisionEvent(List<ParticleCollisionEvent> particleCollisionEvents)
    {
        int particleCount = particleCollisionEvents.Count;

        Debug.Log(particleCount);

        // 每个碰撞位置都出现一个弹孔粒子
        foreach(ParticleCollisionEvent e in particleCollisionEvents)
        {
            if (idx >= particles.Length) idx = 0;

            particles[idx].position = e.intersection;   // 粒子出现位置
            particles[idx].startSize = 1f;          // 粒子尺寸
            particles[idx].startLifetime = 1f;      // startLifetime需要大于0，否则出现后立即消失就看不到了
            particles[idx].remainingLifetime = 1f;  // remainingLifetime需要大于0，否则出现后立即消失就看不到了
            particles[idx].color = Color.black;

            // 粒子面片的角度，和碰撞的物体表面法线方向相同
            Vector3 rotation3D = Quaternion.LookRotation(e.normal).eulerAngles;
            particles[idx].rotation3D = rotation3D;

            idx++;
        }

        //ps.SetParticles(particles);
    }

    void OnDrawGizmos()
    {
        //ps.GetParticles(particles);

        //for(int i = 0; i < particles.Length; i++)
        //{
        //    particles[i].color = Color.red;
        //}

        //ps.SetParticles(particles);
    }
}
