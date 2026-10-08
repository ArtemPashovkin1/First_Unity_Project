using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private Transform[] faces = new Transform[6];

    private Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    public Rigidbody Body => body;

    public bool IsSleeping()
    {
        return body.linearVelocity.sqrMagnitude < 0.001f
            && body.angularVelocity.sqrMagnitude < 0.001f;
    }

    public int TopFace()
    {
        int best = 0;
        float bestDot = -2f;
        for (int i = 0; i < faces.Length; i++)
        {
            if (faces[i] == null) continue;
            float dot = Vector3.Dot(transform.TransformDirection(faces[i].localPosition.normalized), Vector3.up);
            if (dot > bestDot)
            {
                bestDot = dot;
                best = i;
            }
        }
        return best + 1;
    }
}