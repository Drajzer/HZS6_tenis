using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RacketManager : MonoBehaviour
{
    [SerializeField]
    private Transform LookAtObject;
    [SerializeField]
    private ManageSoundsForPlayer sounds;
    [SerializeField]
    private Transform ParentObject;
    [SerializeField]
    private Transform HitCenter;
    [SerializeField]
    private Transform Camera;
    [SerializeField]
    private float SpineRotateMultiplier;
    [SerializeField, Range(0, 1)]
    private float Value;
    [SerializeField]
    public float HeightTolerance;
    [SerializeField]
    private PlayerMovementControler movementControler;
    [SerializeField]
    private ManagePLayerHitP HitP;

    private float RacketRange;
    [SerializeField]
    private float RacketSpeed;

    [SerializeField]
    private Transform ball;
    [SerializeField]
    private Transform RacketTip;

    [SerializeField]
    private Color colorNormal;
    [SerializeField]
    private Color Wasted;
    [SerializeField]
    private Image Stamina;

    [SerializeField]
    Transform TransfromStart;
    [SerializeField]
    Transform TransfromMid;
    [SerializeField]
    Transform TransfromEnd;
    [SerializeField]
    RectTransform ballUi;

    [SerializeField]
    RectTransform Line1;
    [SerializeField]
    RectTransform Line2;
    [SerializeField]
    RectTransform Line3;
    [SerializeField]
    RectTransform Line4;

    [SerializeField]
    private Color NormalColor;
    [SerializeField]
    private Color PerfectColor;
    [SerializeField]
    private float StaminaRecoverSpeed;
    [SerializeField]
    private float staminaWastePerClick;
    [SerializeField]
    private Slider slider;

    bool staminaWasted = false;
    private float StaminaLeft;

    [SerializeField]
    private Transform Racket;
    [SerializeField]
    private Transform Pivot;
    private float ang = 0;
    float wantedValue;

    [SerializeField]
    private Vector3 PivotStartRotation;
    [SerializeField]
    private Vector3 PivotMidRotation;
    [SerializeField]
    private Vector3 PivotEndRotation;

    private BallControler BC;
    private float cantHit = 0;
    [HideInInspector]
    public bool canHit;
    public bool PlayersTurn;

    private Image element1;
    private Image element2;
    private Image element3;
    private Image element4;
    private Image element5;

    private void Start()
    {
        element1 = ballUi.GetComponent<Image>();
        element2 = Line1.GetComponent<Image>();
        element3 = Line2.GetComponent<Image>();
        element4 = Line3.GetComponent<Image>();
        element5 = Line4.GetComponent<Image>();
        BC = ball.GetComponent<BallControler>();
    }

    void Update()
    {
        slider.value = StaminaLeft;
        LookAtObject.position = ParentObject.position + (RotateVectorAroundY(ParentObject.forward, (0.5f - Value) * 2 * -SpineRotateMultiplier) * 5);
        MoveRacket();
        DetectColision();
        ManageBallUI();
        ManageStamina();
        canHit = BC.ShouldHit;
        lerpV();
        if (cantHit >= -1)
        {
            cantHit -= Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(0) && !staminaWasted)
        {
            StaminaLeft -= staminaWastePerClick;
            if (wantedValue == 0)
            {
                wantedValue = 1;
                sounds.SwingRacket();
            }
            else
            {
                wantedValue = 0;
                sounds.SwingRacket();
            }
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

    private void DetectColision()
    {
        RacketRange = Vector2.Distance(v3to2(Pivot.position), v3to2(RacketTip.position));
        Vector2 attackDir = v3to2(movementControler.NormalizedMV).normalized;
        if (attackDir.magnitude == 0)
        {
            attackDir = new Vector2(ParentObject.forward.x, Mathf.Abs(ParentObject.forward.z)).normalized;
        }

        if (Vector2.Distance(v3to2(Pivot.position), v3to2(ball.position)) < RacketRange && cantHit <= 0 && Value > 0 && Value < 1 && !BC.hitByPlayer)
        {
            if (canHit)
            {
                Colision(new Vector3(attackDir.x, 0, attackDir.y).normalized);
                sounds.RacketHit();
            }
            
        }
    }

    private void Colision(Vector3 AttackDirection)
    {
        cantHit = 0.2f;
        AttackDirection = Vector3.Lerp(AttackDirection, new Vector3(ParentObject.forward.x, 0, ParentObject.forward.z), 0.4f);
        HitP.HitBall(AttackDirection, movementControler.Dashing);
    }

    private void ManageBallUI()
    {
        Vector3 dir = Camera.position - ball.position;
        ballUi.SetPositionAndRotation(ball.position, Quaternion.LookRotation(dir.normalized) * Quaternion.AngleAxis(ang, Vector3.forward));
        if (ang > 360)
        {
            ang -= 360;
        }

        if (canHit)
        {
            ang += Time.deltaTime * 250;

            element1.color = PerfectColor;
            element2.color = PerfectColor;
            element3.color = PerfectColor;
            element4.color = PerfectColor;
            element5.color = PerfectColor;
        }
        else
        {
            ang += Time.deltaTime * 100;

            element1.color = NormalColor;
            element2.color = NormalColor;
            element3.color = NormalColor;
            element4.color = NormalColor;
            element5.color = NormalColor;
        }
    }

    private Vector2 v3to2(Vector3 v)
    {
        return new Vector2 (v.x, v.z);
    }

    void ManageStamina()
    {
        StaminaLeft += Time.deltaTime * StaminaRecoverSpeed;
        StaminaLeft = Mathf.Clamp01(StaminaLeft);
        if (StaminaLeft <= 0)
        {
            staminaWasted = true;
        }
        if (StaminaLeft >= 1)
        {
            staminaWasted = false;
        }
        if (staminaWasted)
        {
            Stamina.color = Color.Lerp(Wasted, colorNormal, StaminaLeft);
        }
        else
        {
            Stamina.color = colorNormal;
        }

    }
}
