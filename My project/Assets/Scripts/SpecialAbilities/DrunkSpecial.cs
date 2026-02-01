using System.Collections;
using UnityEngine;

public class DrunkSpecial : CharacterParentClass
{
    [Header("Inversion Settings")]
    [SerializeField] private float inversionChancePerSecond = 0.8f; // 0�1
    [SerializeField] private float inversionDuration = 5f;

    [Header("Audio")]
    public AudioClip drunkSpecial;
    public AudioClip punch;
    public AudioClip succPunch;

    private bool controlsInverted = false;

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
        hitStrength = 20f;
    }

    protected override void Update()
    {
        base.Update();

        // Randomly decide to invert controls while playing
        if (!controlsInverted)
        {
            float p = inversionChancePerSecond * Time.deltaTime;
            if (Random.value < p)
            {
                Debug.Log("Inverted");
                StartCoroutine(InvertControlsTemporarily());
            }
        }
    }

    // Overrides move to invert controls
    protected override void Move()
    {
        Vector2 finalInput = moveInput;

        if (controlsInverted)
        {
            finalInput *= -1f;
        }

        rb.linearVelocity = finalInput * Speed;
    }

    // Inverts controls for a set number of seconds
    private IEnumerator InvertControlsTemporarily()
    {
        // Play special sound if assigned
        if (audioSource != null && attackClip != null)
        {
            audioSource.PlayOneShot(attackClip); // non‑overlapping SFX [web:152][web:160]
        }
        controlsInverted = true;
        yield return new WaitForSeconds(inversionDuration);
        controlsInverted = false;
    }

}
