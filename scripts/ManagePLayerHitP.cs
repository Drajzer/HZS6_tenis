using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class ManagePLayerHitP : MonoBehaviour
{
    [SerializeField]
    private BallControler Bc;
    [SerializeField]
    private Transform ParentObject;
    [SerializeField]
    private float ToleranceAngle;
    [SerializeField]
    private float MaxVerticalSpeed;
    [SerializeField]
    private Vector2 NormalHitSpeed;
    [SerializeField]
    private Vector2 DashHitSpeed;
    [SerializeField]
    private float DashToleranceAngle;
    [SerializeField]
    private Transform[] normalBounds;
    [SerializeField]
    private Transform[] StrongHitBunds;
    [SerializeField]
    private AudioSource racket;
    [SerializeField]
    private AudioClip satisfying;
    private Vector3 p;
    private Transform ball;
    private SphereCollider sc;

    private void Awake()
    {
        ball = Bc.gameObject.transform;
        sc = ball.GetComponent<SphereCollider>();
    }

    public void HitBall(Vector3 hitAngle, bool dashHit)
    {
        Vector3 LandPosition;
        LandPosition.y = normalBounds[0].position.y;
        if (!dashHit)
        {
            float adition = -Vector3.SignedAngle(ParentObject.forward, hitAngle, Vector3.up) / (ToleranceAngle / 2);
            LandPosition.x = Random.Range(normalBounds[0].position.x, normalBounds[2].position.x);
            float distanceZ = Mathf.Abs(normalBounds[0].position.z - normalBounds[1].position.z) / 2;
            LandPosition.z = (normalBounds[0].position.z + normalBounds[1].position.z) / 2;
            LandPosition.z += distanceZ * adition;
            Bc.HitByPlayer(DetermineBallVelocity(ball.position, LandPosition, dashHit));
            p = LandPosition;
        }
        else
        {
            float adition = -Vector3.SignedAngle(ParentObject.forward, hitAngle, Vector3.up) / (DashToleranceAngle / 2);
            LandPosition.x = Random.Range(StrongHitBunds[0].position.x, StrongHitBunds[2].position.x);
            float distanceZ = Mathf.Abs(normalBounds[0].position.z - normalBounds[1].position.z) / 2;
            LandPosition.z = (normalBounds[0].position.z + normalBounds[1].position.z) / 2;
            LandPosition.z += distanceZ * adition;
            Bc.HitByPlayer(DetermineBallVelocity(ball.position, LandPosition, dashHit));
            p = LandPosition;
            racket.PlayOneShot(satisfying, 0.85f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(p, 0.2f);
    }

    private void Update()
    {

    }

    Vector3 DetermineBallVelocity(Vector3 startPosition, Vector3 LandPosition, bool dashing)
    {
        Vector3 velocity = Vector3.zero;

        if (!dashing)
        {
            float distance = Vector2.Distance(v3to2(startPosition), v3to2(LandPosition));
            float horizontalSpeed = Random.Range(NormalHitSpeed.x, NormalHitSpeed.y);
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
        }
        else
        {
            float distance = Vector2.Distance(v3to2(startPosition), v3to2(LandPosition));
            float horizontalSpeed = Random.Range(DashHitSpeed.x, DashHitSpeed.y);
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
        }

        return velocity;
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
