using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementControler : MonoBehaviour
{
    [SerializeField]
    private Animator m_Animator;
    [SerializeField]
    private float WalkSpeed;
    [SerializeField]
    private float RunSpeed;
    [SerializeField]
    private float Acceleration;
    [SerializeField]
    private float DirectionChangeSpeed;
    [SerializeField]
    private CharacterController Controller;
    [SerializeField]
    public float DashSpeed;
    [SerializeField]
    private float DashDistance;
    [SerializeField]
    private CreateAPath path;
    [SerializeField]
    private float DashTotalRecoveryTime;
    [SerializeField]
    private ManageSoundsForPlayer sounds;
    [SerializeField]
    private Slider DashBar;

    float CurrentRecoveryTime;

    float Speed;
    float WantedSpeed;
    bool Runing;
    [HideInInspector]
    public bool Dashing;

    [HideInInspector]
    public float RealSpeed;

    float currentDistance;
    Vector3 LastPosition;

    Vector3 movementVector;
    [HideInInspector]
    public Vector3 NormalizedMV;

    private void Start()
    {
        CurrentRecoveryTime = DashTotalRecoveryTime;
    }

    void Update()
    {
        DashBar.value = Mathf.Clamp01(CurrentRecoveryTime / DashTotalRecoveryTime);
        movementVector = Vector3.Lerp(movementVector, new Vector3(Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal")), Time.deltaTime * DirectionChangeSpeed);

        NormalizedMV = movementVector;
        if (movementVector.magnitude > 1)
        {
            NormalizedMV.Normalize();
        }
        if (movementVector.magnitude > 0)
        {
            WantedSpeed = WalkSpeed;
            Runing = true;
            if (Runing)
            {
                WantedSpeed = RunSpeed;
            }
        }

        Speed = Mathf.Lerp(Speed, WantedSpeed, Time.deltaTime * Acceleration);

        if (!Dashing)
        {
            Controller.Move(NormalizedMV * Speed * Time.deltaTime);
        }
        else
        {
            Controller.Move(NormalizedMV.normalized * DashSpeed * Time.deltaTime);
        }

        RealSpeed = Mathf.Lerp(RealSpeed,  Vector3.Distance(LastPosition, transform.position) / Time.deltaTime, Time.deltaTime * Acceleration);
        RealSpeed = Mathf.Clamp(RealSpeed, 0, RunSpeed);
        LastPosition = transform.position;

        ManageAnimator();
        Dash();
    }

    void ManageAnimator()
    {
        float ForwardValue = (movementVector.x * (Mathf.Min(RealSpeed, WalkSpeed) / WalkSpeed));
        ForwardValue += (movementVector.x * (Mathf.Max((RealSpeed - WalkSpeed), 0) / (RunSpeed - WalkSpeed)));

        float RightValue = (movementVector.z * (Mathf.Min(RealSpeed, WalkSpeed) / WalkSpeed));
        RightValue += (movementVector.z * (Mathf.Max(RealSpeed - WalkSpeed, 0) / (RunSpeed - WalkSpeed)));
        m_Animator.SetFloat("Forward", ForwardValue);
        m_Animator.SetFloat("Right", -RightValue);
    }

    void Dash()
    {
        CurrentRecoveryTime += Time.deltaTime;
        CurrentRecoveryTime = Mathf.Clamp(CurrentRecoveryTime, 0, DashTotalRecoveryTime);

        if (Input.GetKeyDown(KeyCode.Space) && CurrentRecoveryTime > DashTotalRecoveryTime / 3)
        {
            sounds.DashSound();
            CurrentRecoveryTime -= DashTotalRecoveryTime / 3;
            currentDistance = DashDistance;
            Dashing = true;
            path.StartCreating(DashDistance / DashSpeed, 120, 0.02f);
        }
        if (currentDistance > 0)
        {
            currentDistance -= Time.deltaTime * DashSpeed;
        }
        else
            Dashing = false;
    }

}
