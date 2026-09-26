using System.Collections.Generic;
using UnityEngine;

public class CubeOrbit : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField, Min(0)] private int cubeCount = 10;
    [SerializeField, Range(0f, 15f)] private float radius = 5f;
    [SerializeField, Range(0f, 360f)] private float speed = 40f;
    [SerializeField] private bool clockwise = true;
    [SerializeField] private bool evenDistribution = true;

    private const float FollowStep = 15f;

    private readonly List<Transform> cubes = new List<Transform>();
    private readonly List<float> baseAngles = new List<float>();
    private float orbitAngle;
    private bool appliedEven;

    private void Awake()
    {
        if (cubePrefab == null)
            return;

        appliedEven = evenDistribution;
        int count = Mathf.Max(0, cubeCount);
        for (int i = 0; i < count; i++)
        {
            float angle = ComputeAngle(i, count);
            baseAngles.Add(angle);
            cubes.Add(Instantiate(cubePrefab, OffsetToWorld(angle), transform.rotation, transform).transform);
        }
    }

    private void Update()
    {
        if (cubes.Count == 0) return;

        if (evenDistribution != appliedEven)
        {
            appliedEven = evenDistribution;
            for (int i = 0; i < baseAngles.Count; i++)
                baseAngles[i] = ComputeAngle(i, baseAngles.Count);
        }

        orbitAngle += (clockwise ? -1f : 1f) * speed * Time.deltaTime;

        for (int i = 0; i < cubes.Count; i++)
            cubes[i].position = OffsetToWorld(baseAngles[i] + orbitAngle);
    }

    private Vector3 OffsetToWorld(float angle)
    {
        return transform.TransformPoint(Quaternion.Euler(0f, angle, 0f) * (Vector3.forward * radius));
    }

    private float ComputeAngle(int index, int total)
    {
        if (evenDistribution && total > 0) return index * 360f / total;
        return index * FollowStep;
    }
}