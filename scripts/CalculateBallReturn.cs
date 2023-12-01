using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Member;

public class CalculateBallReturn : MonoBehaviour
{
    [SerializeField]
    private Transform[] Bounds = new Transform[4];
    [SerializeField]
    private Transform[] Bounds1 = new Transform[4];
    [SerializeField]
    private Transform LauchPosition;
    [SerializeField]
    private BallControler Bc;
    [SerializeField]
    private Transform ball;
    [SerializeField]
    private Rigidbody Rb;
    [SerializeField]
    private SphereCollider coll;
    [SerializeField]
    private float MaxVerticalSpeed;
    public Vector2 LaunchSpeed;

    private SphereCollider sc;

    private void Start()
    {
        sc = ball.GetComponent<SphereCollider>();
    }

    Vector3 landPos;
    public void Shoot(float SpeedOveride = 1, bool Center = false)
    {
        if (!Center)
        {
            landPos = new Vector3(Random.Range(Mathf.Min(Bounds[0].position.x, Bounds[1].position.x, Bounds[2].position.x, Bounds[3].position.x),
                Mathf.Max(Bounds[0].position.x, Bounds[1].position.x, Bounds[2].position.x, Bounds[3].position.x)),

                Bounds[0].position.y,

                Random.Range(Mathf.Min(Bounds[0].position.z, Bounds[1].position.z, Bounds[2].position.z, Bounds[3].position.z),
                Mathf.Max(Bounds[0].position.z, Bounds[1].position.z, Bounds[2].position.z, Bounds[3].position.z)));
        }
        else
        {
            landPos = new Vector3(Random.Range(Mathf.Min(Bounds1[0].position.x, Bounds1[1].position.x, Bounds1[2].position.x, Bounds1[3].position.x),
                Mathf.Max(Bounds1[0].position.x, Bounds1[1].position.x, Bounds1[2].position.x, Bounds1[3].position.x)),

                Bounds1[0].position.y,

                Random.Range(Mathf.Min(Bounds1[0].position.z, Bounds1[1].position.z, Bounds1[2].position.z, Bounds1[3].position.z),
                Mathf.Max(Bounds1[0].position.z, Bounds1[1].position.z, Bounds1[2].position.z, Bounds1[3].position.z)));
        }
        Bc.ShouldHit = false;
        Bc.reset = true;
        ball.position = LauchPosition.position;
        Rb.velocity = DetermineBallVelocity(LauchPosition.position, landPos, SpeedOveride);
        Bc.hitByPlayer = false;
    }

    Vector3 DetermineBallVelocity(Vector3 startPosition, Vector3 LandPosition, float Overide)
    {
        Vector3 velocity;
        float distance = Vector2.Distance(v3to2(startPosition), v3to2(LandPosition));
        float horizontalSpeed = Random.Range(LaunchSpeed.x, LaunchSpeed.y) * Overide;
        float time = distance / horizontalSpeed;
        float h = ball.position.y - sc.radius;
        float verticalS = -CalculateInitialVelocity(h, time);
        if (verticalS > MaxVerticalSpeed)
        {
            verticalS = MaxVerticalSpeed;
            time = CalculateFreeFallTime(h, -verticalS);
            horizontalSpeed = distance / time;
        }
        Vector3 Dir = LandPosition - startPosition;
        Dir.Normalize();
        velocity = new Vector3(Dir.x, 0, Dir.z).normalized * horizontalSpeed + Vector3.up * verticalS;
        return velocity;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(landPos, 0.2f);
    }

    public float CalculateFreeFallTime(float height, float initialVelocity)
    {
        float time = (-initialVelocity + Mathf.Sqrt(initialVelocity * initialVelocity + 2 * -Physics.gravity.y * height)) / -Physics.gravity.y;

        return time;
    }

    public float CalculateInitialVelocity(float height, float time)
    {
        float initialV = (2 * height - -Physics.gravity.y * time * time) / (2 * time);

        return initialV;
    }

    private Vector2 v3to2(Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }
}
