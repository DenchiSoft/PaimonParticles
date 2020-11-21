using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Stripped down version of the "Plexus" effect by Mirza Beig (MIT License).
/// See https://www.youtube.com/watch?v=ruNPkuYT1Ck
/// </summary>
public class PaimonParticles : MonoBehaviour
{
    // Renderer to use for the lines between particles.
    public LineRenderer lineRendererTemplate;

    private ParticleSystem lineParticleSystem;
    private ParticleSystem.Particle[] particles;
    private ParticleSystem.MainModule particleSystemMainModule;
    private List<LineRenderer> lineRenderers = new List<LineRenderer>();

    // Particle properties.
    private Vector3[] particlePositions;
    private Color[] particleColors;
    private float[] particleSizes;

    void Start()
    {
        lineParticleSystem = GetComponent<ParticleSystem>();
        particleSystemMainModule = lineParticleSystem.main;

        int maxParticles = particleSystemMainModule.maxParticles;

        if (particles == null || particles.Length < maxParticles)
        {
            particles = new ParticleSystem.Particle[maxParticles];
            particlePositions = new Vector3[maxParticles];
            particleColors = new Color[maxParticles];
            particleSizes = new float[maxParticles];
        }
    }

    void OnDisable()
    {
        lineRenderers.ForEach(renderer => renderer.enabled = false);
    }

    void LateUpdate()
    {
        int lineRenderersCount = lineRenderers.Count;

        Color lineRendererStartColor = lineRendererTemplate.startColor;
        Color lineRendererEndColor = lineRendererTemplate.endColor;

        float lineRendererStartWidth = lineRendererTemplate.startWidth * lineRendererTemplate.widthMultiplier;
        float lineRendererEndWidth = lineRendererTemplate.endWidth * lineRendererTemplate.widthMultiplier;

        fillParticlePropertyCache();

        int lrIndex = 0;

        for (int i = 0; i < lineParticleSystem.particleCount; i++)
        {
            Color particleColor = particleColors[i];
            Color lineStartColor = particleColor;

            float lineStartWidth = Mathf.LerpUnclamped(lineRendererStartWidth, particleSizes[i], 0.15f);

            if (i + 1 < lineParticleSystem.particleCount)
            {
                LineRenderer lr;

                if (lrIndex == lineRenderersCount)
                {
                    lr = Instantiate(lineRendererTemplate, transform, false);
                    lineRenderers.Add(lr);
                    lineRenderersCount++;
                }

                lr = lineRenderers[lrIndex];

                lr.enabled = true;
                lr.SetPosition(0, particlePositions[i]);
                lr.SetPosition(1, particlePositions[i + 1]);
                lineStartColor.a = particleColor.a;
                lr.startColor = lineStartColor;

                particleColor = particleColors[i + 1];

                Color lineEndColour = particleColor;
                lineEndColour.a = particleColor.a;
            
                lr.endColor = lineEndColour;
                lr.startWidth = lineStartWidth;
                lr.endWidth = Mathf.LerpUnclamped(lineRendererEndWidth, particleSizes[i + 1], 0.15f);

                lrIndex++;
            }
        }

        for (int i = lrIndex; i < lineRenderersCount; i++)
        {
            if (lineRenderers[i].enabled)
            {
                lineRenderers[i].enabled = false;
            }
        }
    }

    private void fillParticlePropertyCache()
    {
        lineParticleSystem.GetParticles(particles);

        for (int i = 0; i < lineParticleSystem.particleCount; i++)
        {
            particlePositions[i] = particles[i].position;
            particleColors[i] = particles[i].GetCurrentColor(lineParticleSystem);
            particleSizes[i] = particles[i].GetCurrentSize(lineParticleSystem);
        }
    }
}
