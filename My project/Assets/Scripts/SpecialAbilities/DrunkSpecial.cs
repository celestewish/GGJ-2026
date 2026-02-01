using System.Collections;
using UnityEngine;

public class DrunkSpecial : CharacterParentClass
{
    [SerializeField] private float specialDuration = 10f;
    [SerializeField] private float drunkHitStrength = 20f;
    [Header("Inversion Settings")]
    [SerializeField] private float inversionChancePerSecond = 0.8f; // 0�1
    [SerializeField] private float inversionDuration = 2f;

    [Header("Audio")]
    public AudioClip drunkSpecial;
    public AudioClip punch;
    public AudioClip succPunch;

    private bool controlsInverted = false;

    private float defaultHitStrength;

    protected override void Awake()
    {
        base.Awake();
        attackClip = drunkSpecial;
        swingPunch = punch;
        successPunch = succPunch;
    }

    //RAAAAAAAAAAAAAAAAAAAAAAAAAAAA
    private void Start()
    {
        defaultHitStrength = hitStrength;
    }

    protected override void Update()
    {
        base.Update();

        /*// Randomly decide to invert controls while playing
        if (!controlsInverted)
        {
            float p = inversionChancePerSecond * Time.deltaTime;
            if (Random.value < p)
            {
                Debug.Log("Inverted");
                StartCoroutine(InvertControlsTemporarily());
            }
        }*/
    }

    // Overrides move to invert controls
    protected override void Move()
    {
        Vector2 finalInput = moveInput;

        if (controlsInverted)
        {
            finalInput *= -1f;
        }

        //rb.linearVelocity = finalInput * Speed;
        rb.AddForce(finalInput * Speed * Time.deltaTime * 5, ForceMode2D.Impulse);

        //Apply gradual rotation based on weight factor
        if (lookDir != Vector3.zero)
        {
            Quaternion rot = Quaternion.FromToRotation(transform.up, lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot * transform.rotation, 1 / weight * Time.deltaTime * 25);
        }
    }

    protected override void PerformSpecial()
    {
        doingSpecial = true;
        StartCoroutine(SpecialDuration());
    }

    private IEnumerator SpecialDuration()
    {
        hitStrength = drunkHitStrength;

        // Play special sound if assigned
        if (audioSource != null && attackClip != null)
        {
            audioSource.PlayOneShot(attackClip); // non‑overlapping SFX [web:152][web:160]
        }

        float time = 0;
        while (time < specialDuration)
        {
            if (!controlsInverted)
            {
                float p = inversionChancePerSecond * Time.deltaTime;
                if (Random.value < p)
                {
                    Debug.Log("Inverted");
                    StartCoroutine(InvertControlsTemporarily());
                }
            }

            yield return null;
            time += Time.deltaTime;
        }

        doingSpecial = false;

        hitStrength = defaultHitStrength;
    }

    // Inverts controls for a set number of seconds
    private IEnumerator InvertControlsTemporarily()
    {

        controlsInverted = true;
        yield return new WaitForSeconds(inversionDuration);
        controlsInverted = false;
    }

}
