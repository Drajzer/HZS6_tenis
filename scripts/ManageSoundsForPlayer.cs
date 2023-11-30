using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageSoundsForPlayer : MonoBehaviour
{
    [SerializeField]
    private PlayerMovementControler controler;
    [SerializeField]
    private Vector2 PitchVariation;
    [SerializeField]
    private AudioSource Source;
    [SerializeField]
    private AudioClip[] StepClips;
    [SerializeField]
    private float MultiplierPerSecond;
    [SerializeField]
    private AudioClip dahsSFX;
    [SerializeField]
    private AudioClip HitRacket;
    [SerializeField]
    private AudioSource RackeSFX;
    [SerializeField]
    private AudioClip RacketSwing;
    [SerializeField]
    private AudioClip[] Grunts;

    private float time;

    // Update is called once per frame
    void Update()
    {
        if (controler.RealSpeed > 0.1f && time >= 1 / (controler.RealSpeed * MultiplierPerSecond))
        {
            time = 0;
            PlayWalk();
        }
        else if (controler.RealSpeed > 0.1f)
        {
            time += Time.deltaTime;
        }
    }

    private void PlayWalk()
    {
        Source.pitch = Random.Range(PitchVariation.x, PitchVariation.y);
        Source.PlayOneShot(StepClips[Random.Range(0, StepClips.Length)], 0.7f);
    }

    public void DashSound()
    {
        Source.pitch = Random.Range(PitchVariation.x, PitchVariation.y);
        Source.PlayOneShot(dahsSFX);
    }

    public void RacketHit()
    {
        Source.pitch = Random.Range(PitchVariation.x, PitchVariation.y);
        RackeSFX.PlayOneShot(HitRacket);
        if (Random.Range(1, 4) > 2)
        {
            Source.pitch = Random.Range(PitchVariation.x, PitchVariation.y);
            Source.PlayOneShot(Grunts[Random.Range(0, Grunts.Length)], 0.35f);
        }
    }

    public void SwingRacket()
    {
        Source.pitch = Random.Range(PitchVariation.x, PitchVariation.y);
        RackeSFX.PlayOneShot(RacketSwing, 0.4f);
    }
}
