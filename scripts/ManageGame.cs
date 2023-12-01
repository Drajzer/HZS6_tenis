using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Net.Sockets;

public class ManageGame : MonoBehaviour
{
    [SerializeField]
    private Transform ball;
    [SerializeField]
    private CalculateBallReturn ballReturn;
    [SerializeField]
    private ManagePLayerHitP PlayerHit;
    [SerializeField]
    private Transform PlacePositionBall;
    [SerializeField]
    private float TriggerDistance;
    [SerializeField]
    private TextMeshProUGUI DjokovicScore;
    [SerializeField]
    private TextMeshProUGUI FeddereScore;
    [SerializeField]
    private string fedName;
    [SerializeField]
    private int pointsToWin;
    [SerializeField]
    private Image Black;
    [SerializeField]
    private Color normal;
    [SerializeField]
    private Color End;
    [SerializeField]
    private float fadeToBlackSpeed;
    [SerializeField]
    private PlayerMovementControler cont;
    [SerializeField]
    private RacketManager racket;
    [SerializeField]
    private GameObject LossScreen;
    [SerializeField]
    private GameObject WinScreen;

    private Color WantedColor;
    private BallControler bc;
    private Rigidbody rb;
    bool canPause;

    private int DJscore;
    private int FDscore;

    bool ended = false;

    bool callAgain = true;
    private void Awake()
    {
        AudioListener.volume = 1;
        WantedColor = normal;
        bc = ball.GetComponent<BallControler>();
        rb = ball.GetComponent<Rigidbody>();
        StartCoroutine(LaunchBall());
        rb.isKinematic = true;
        ball.position = PlacePositionBall.position;
        canPause = true;
    }

    void Update()
    {
        LerpColor();
        DjokovicScore.text = "Djokovic - " + DJscore;
        FeddereScore.text = FDscore + " - " + fedName;

        if (DJscore >= pointsToWin && !ended)
        {
            ended = true;
            StartCoroutine(Win());
        }
        if (FDscore >= pointsToWin && !ended)
        {
            ended = true;
            StartCoroutine(Lose());
            cont.enabled = false;
            racket.enabled = false;
        }

        float distance = Vector3.Distance(ball.position, PlacePositionBall.position);
        if (bc.bounceCount == 2 && !ended || ball.position.y < -10 && !ended || distance > TriggerDistance && !ended)
        {
            if (callAgain)
            {
                if (bc.hitByPlayer)
                {
                    ScorePlayer();
                }
                else
                {
                    ScoreEnemy();
                }
                callAgain = false;
            }

        }
    }

    private IEnumerator Win()
    {
        canPause = false;
        WantedColor = End;
        AudioListener.volume = 0;
        yield return new WaitForSeconds(2);
        WinScreen.SetActive(true);
    }

    private IEnumerator Lose()
    {
        canPause = false;
        AudioListener.volume = 0;
        WantedColor = End;
        yield return new WaitForSeconds(2);
        LossScreen.SetActive(true);
    }

    void LerpColor()
    {
        Black.color = Vector4.Lerp(Black.color, WantedColor, fadeToBlackSpeed * Time.deltaTime);
    }
    IEnumerator LaunchBall()
    {
        yield return new WaitForSeconds(0.5f);
        ball.position = PlacePositionBall.position;
        rb.isKinematic = true;
        yield return new WaitForSeconds(1);
        rb.isKinematic = false;
        if (Random.value < 0.5f)
        {
            PlayerHit.HitBall(Vector3.forward, false, false, 0.5f);
        }
        else
        {
            ballReturn.Shoot(0.5f, true);
        }
        bc.bounceCount = 0;
        callAgain = true;
    }

    void ScorePlayer()
    {
        StartCoroutine(LaunchBall());
        DJscore++;
    }
    
    void ScoreEnemy()
    {
        StartCoroutine(LaunchBall());
        FDscore++;
    }
}
