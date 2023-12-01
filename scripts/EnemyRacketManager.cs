using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

[System.Serializable]
struct RacketTranform
{
    public Vector3 position;
    public Vector3 LocalRotation;
}

public class EnemyRacketManager : MonoBehaviour
{
    [SerializeField]
    private Transform LookAtObject;
    [SerializeField]
    private Transform ParentObject;
    [SerializeField]
    private float SpineRotateMultiplier;
    [SerializeField, Range(0, 1)]
    private float Value;
    [SerializeField]
    private float RacketRange;
    [SerializeField]
    private CalculateBallReturn ReturnB;

    [SerializeField]
    private float RacketSpeed;

    [SerializeField]
    private Transform ball;

    [SerializeField]
    Transform TransfromStart;
    [SerializeField]
    Transform TransfromMid;
    [SerializeField]
    Transform TransfromEnd;

    [SerializeField]
    private Transform Racket;
    [SerializeField]
    private Transform Pivot;
    float wantedValue;

    [SerializeField]
    private Vector3 PivotStartRotation;
    [SerializeField]
    private Vector3 PivotMidRotation;
    [SerializeField]
    private Vector3 PivotEndRotation;
    private float cooldown;
    private BallControler bc;

    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private AudioClip RacketHit;
    [SerializeField]
    private AudioClip racketSwing;
    [SerializeField]
    private AudioClip[] Grunts;

    private void Awake()
    {
        bc = ball.GetComponent<BallControler>();
    }

    void Update()
    {
        LookAtObject.position = ParentObject.position + (RotateVectorAroundY(ParentObject.forward, (0.5f - Value) * 2 * -SpineRotateMultiplier) * 5);
        MoveRacket();
        lerpV();
        cooldown -= Time.deltaTime;
        if (Vector2.Distance(v3to2(ball.position), v3to2(ParentObject.position)) < RacketRange && cooldown <= 0 && bc.bounceCount > 0)
        {
            ReturnB.Shoot();
            Swing();
            cooldown = 0.5f;
            PlaySounds();
        }
    }

    void Swing()
    {
        if (wantedValue == 0)
        {
            wantedValue = 1;
        }
        else
        {
            wantedValue = 0;
        }
    }

    void MoveRacket()
    {
        if (Value <= 0.5f)
        {
            Racket.position = Vector3.Slerp(TransfromStart.position, TransfromMid.position, Value * 2);
            Racket.rotation = Quaternion.Lerp(TransfromStart.rotation, TransfromMid.rotation, Value * 2);
            Pivot.localRotation = Quaternion.Lerp(Quaternion.Euler(PivotStartRotation), Quaternion.Euler(PivotMidRotation), Value * 2);
        }
        else
        {
            Racket.position = Vector3.Slerp(TransfromMid.position, TransfromEnd.position, (Value - 0.5f) * 2);
            Racket.rotation = Quaternion.Lerp(TransfromMid.rotation, TransfromEnd.rotation, (Value - 0.5f) * 2);
            Pivot.localRotation = Quaternion.Lerp(Quaternion.Euler(PivotMidRotation), Quaternion.Euler(PivotEndRotation), (Value - 0.5f) * 2);
        }

    }
    private void PlaySounds()
    {
        source.pitch = Random.Range(0.85f, 1.15f);
        source.PlayOneShot(RacketHit);
        source.pitch = Random.Range(0.85f, 1.15f);
        source.PlayOneShot(racketSwing);
        if (Random.value > 0.5f)
        {
            source.pitch = Random.Range(0.75f, 1);
            source.PlayOneShot(Grunts[Random.Range(0, Grunts.Length)], 0.5f);
        }
    }

    void lerpV()
    {
        if (wantedValue == 0)
        {
            Value -= Time.deltaTime * RacketSpeed;
        }
        else
        {
            Value += Time.deltaTime * RacketSpeed;
        }
        Value = Mathf.Clamp01(Value);
    }

    Vector3 RotateVectorAroundY(Vector3 originalVector, float angleInDegrees)
    {
        float angleInRadians = angleInDegrees * Mathf.Deg2Rad;

        float cosTheta = Mathf.Cos(angleInRadians);
        float sinTheta = Mathf.Sin(angleInRadians);

        float x = originalVector.x * cosTheta - originalVector.z * sinTheta;
        float z = originalVector.x * sinTheta + originalVector.z * cosTheta;

        return new Vector3(x, originalVector.y, z);
    }

    private Vector2 v3to2(Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

}
