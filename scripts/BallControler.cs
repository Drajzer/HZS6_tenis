using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Rendering;

public class BallControler : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private RacketManager RManager;
    [SerializeField]
    private float TimeToHitTheBall;
    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private AudioClip[] BounceClips;
    [SerializeField]
    private Vector2 ballWhistleVelocity;
    [SerializeField]
    private Vector2 PithcChange;
    [SerializeField]
    private AudioSource Asource;

    private float timeLeft;

    public bool ShouldHit;
    public bool reset = true;
    int bounceCount = 0;
    float cooldown = 0;

    public void HitByPlayer(Vector3 Velocity)
    {
        rb.velocity = Velocity;
        bounceCount = 0;
    }

    private void Update()
    {
        Vector2 v = new Vector2(rb.velocity.x, rb.velocity.z);
        Asource.volume = Mathf.Clamp01((v.magnitude - ballWhistleVelocity.x) / (ballWhistleVelocity.y - ballWhistleVelocity.x)) * 0.4f;

        cooldown -= Time.deltaTime;
        if (timeLeft >= 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else
        {
            ShouldHit = false;
        }
        if (reset)
        {
            bounceCount = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (reset)
        {
            reset = false;
            ShouldHit = true;
            timeLeft = TimeToHitTheBall;
        }
        if (cooldown <= 0)
        {
            playSfx();
        }
    }

    void playSfx()
    {
        cooldown = 0.05f;
        source.pitch = Random.Range(0.9f, 1.1f);
        source.PlayOneShot(BounceClips[bounceCount]);
        bounceCount++;
        bounceCount = Mathf.Clamp(bounceCount, 0, BounceClips.Length - 1);
    }
}
