using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CalculateBallReturn : MonoBehaviour
{
    [SerializeField]
    private Transform[] Bounds = new Transform[4];
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
    private float LaunchSpeed;
    [SerializeField]
    private float verticalSpeedDown;

    [SerializeField]
    private bool createP;
    [SerializeField]
    private bool Launch;
    Vector3 landPos;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            createP = false;
            landPos = new Vector3(Random.Range(Mathf.Min(Bounds[0].position.x, Bounds[1].position.x, Bounds[2].position.x, Bounds[3].position.x),
                Mathf.Max(Bounds[0].position.x, Bounds[1].position.x, Bounds[2].position.x, Bounds[3].position.x)),

                Bounds[0].position.y,

                Random.Range(Mathf.Min(Bounds[0].position.z, Bounds[1].position.z, Bounds[2].position.z, Bounds[3].position.z),
                Mathf.Max(Bounds[0].position.z, Bounds[1].position.z, Bounds[2].position.z, Bounds[3].position.z)));
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Bc.ShouldHit = false;
            Bc.reset = true;
            Launch = false;
            ball.position = LauchPosition.position;
            Vector3 dir = -(LauchPosition.position - landPos).normalized;
            float height = ball.position.y - coll.radius;
            float t = Vector2.Distance(v3to2(LauchPosition.position), v3to2(landPos)) / LaunchSpeed;
            Rb.velocity = new Vector3(dir.x, 0, dir.z).normalized * LaunchSpeed + Vector3.up * -CalculateInitialVelocity(height, t);
        }
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
