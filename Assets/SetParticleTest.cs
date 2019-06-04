using UnityEngine;
using System.Collections;

public class SetParticleTest : MonoBehaviour
{
    public GameObject TargetPrimitive;

    [Range(0.0f, 1.0f)]
    public float ParticleSpeed;

    private Vector3[] m_targetVertices;
    private ParticleSystem.Particle[] m_targetParticles;

    void Start()
    {
        m_targetVertices = TargetPrimitive.GetComponent<MeshFilter>().sharedMesh.vertices;
        m_targetParticles = new ParticleSystem.Particle[m_targetVertices.Length];
    }

    void Update()
    {
        m_targetVertices = TargetPrimitive.GetComponent<MeshFilter>().sharedMesh.vertices;

        for (int i = 0; i < m_targetVertices.Length; i++)
        {
            m_targetVertices[i] = TargetPrimitive.transform.TransformPoint(m_targetVertices[i]);

            m_targetParticles[i].position = m_targetParticles[i].position * (1f - ParticleSpeed) + m_targetVertices[i] * ParticleSpeed;
            m_targetParticles[i].startColor = new Color(1f - m_targetVertices[i].x % 1f, 0.2f + m_targetVertices[i].y % 0.8f, 0.5f + m_targetVertices[i].z % 0.5f);
            m_targetParticles[i].startSize = 0.05f;

            m_targetParticles[i].remainingLifetime = 10f;
            m_targetParticles[i].startLifetime = 10f;
        }

        GetComponent<ParticleSystem>().SetParticles(m_targetParticles, m_targetParticles.Length);
    }
}