using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Unity.VisualScripting.Member;

public class EnemyMovementControler : MonoBehaviour
{
    [SerializeField]
    private float WalkSpeed;
    [SerializeField]
    private float RunSpeed;
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private float DirectionChangeSpeed;
    [SerializeField]
    private ManagePLayerHitP PlayerHit;
    [SerializeField]
    private BallControler bc;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float distnaceAhead;
    [SerializeField]
    private Vector2 BallSpeed;
    public float MovementSpeed;
    public float ReactTime;
    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private AudioClip[] walking;
    [SerializeField]
    private float StepMultiplier;

    [SerializeField]
    private CalculateBallReturn ballReturn;

    private float sp;
    private float react;

    public int difficulty;

    public float LevelMultiplierSpeed;
    public float LevelReactionMultiplier;
    public float LevelBallSpeedMultiplier;

    float Speed;
    Vector3 movementVector;
    Vector3 actualMovement;
    Vector3 lastPos;
    float timeLeft;
    float time;
    [SerializeField]
    private ManageSavingAndData data;
    // Update is called once per frame

    private void Awake()
    {
        lastPos = transform.position;
    }

    private void SetDfficultyParameters()
    {
        switch (difficulty)
        {
            case 0:
                sp = MovementSpeed * 0.7f * LevelMultiplierSpeed;
                react = (ReactTime * 1.4f) / LevelReactionMultiplier;
                ballReturn.LaunchSpeed.x = (BallSpeed.x * 0.8f) * LevelBallSpeedMultiplier;
                ballReturn.LaunchSpeed.y = (BallSpeed.y * 0.8f) * LevelBallSpeedMultiplier;
                break;
            case 1:
                sp = MovementSpeed * LevelMultiplierSpeed;
                react = ReactTime / LevelReactionMultiplier;
                ballReturn.LaunchSpeed.x = BallSpeed.x * LevelBallSpeedMultiplier;
                ballReturn.LaunchSpeed.y = BallSpeed.y * LevelBallSpeedMultiplier;
                break;
            case 2:
                sp = MovementSpeed * 1.5f * LevelMultiplierSpeed;
                react = (ReactTime * 0.4f) / LevelReactionMultiplier;
                ballReturn.LaunchSpeed.x = (BallSpeed.x * 1.4f) * LevelBallSpeedMultiplier;
                ballReturn.LaunchSpeed.y = (BallSpeed.y * 1.4f) * LevelBallSpeedMultiplier;
                break;
        }
    }

    void Update()
    {
        difficulty = data.save.Difficulty;
        SetDfficultyParameters();
        agent.speed = sp;
        DetectMovement();
        ManageAnimator();
        ManageWalkingSounds();
        if (bc.hitByPlayer && timeLeft <= 0 && bc.bounceCount < 2)
        {
            agent.SetDestination(PlayerHit.p + SetY0(rb.velocity).normalized * distnaceAhead);
        }
        else if (!bc.hitByPlayer)
        {
            timeLeft = react;
        }
        timeLeft -= Time.deltaTime;
    }

    void DetectMovement()
    {
        actualMovement = (transform.position - lastPos);
        movementVector = Vector3.Lerp(movementVector, actualMovement, DirectionChangeSpeed);
        movementVector = RotateVectorAroundY(movementVector.normalized, 180);
        Speed = Vector3.Distance(lastPos, transform.position) / Time.deltaTime;
        lastPos = transform.position;
    }

    void ManageAnimator()
    {
        float ForwardValue = (movementVector.x * (Mathf.Min(Speed, WalkSpeed) / WalkSpeed));
        ForwardValue += (movementVector.x * (Mathf.Max((Speed - WalkSpeed), 0) / (RunSpeed - WalkSpeed)));

        float RightValue = (movementVector.z * (Mathf.Min(Speed, WalkSpeed) / WalkSpeed));
        RightValue += (movementVector.z * (Mathf.Max(Speed - WalkSpeed, 0) / (RunSpeed - WalkSpeed)));
        anim.SetFloat("Forward", ForwardValue);
        anim.SetFloat("Right", -RightValue);
    }

    void ManageWalkingSounds()
    {
        if (Speed > 0.1f && time >= 1 / (Speed * StepMultiplier))
        {
            time = 0;
            playWalk();
        }
        else if (Speed > 0.1f)
        {
            time += Time.deltaTime;
        }
    }
    void playWalk()
    {
        source.pitch = Random.Range(0.8f, 1.2f);
        source.PlayOneShot(walking[Random.Range(0, walking.Length)], 0.7f);
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

    Vector3 SetY0(Vector3 v)
    {
        return new Vector3(v.x, 0, v.z);
    }
}
