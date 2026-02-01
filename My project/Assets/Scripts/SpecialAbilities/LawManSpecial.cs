using System.Collections;
using UnityEngine;

public class LawManSpecial : CharacterParentClass
{
    [Header("Baton Settings")]
    [SerializeField] private float batonHitStrength = 15;
    [SerializeField] private GameObject batonObject;
    [SerializeField] private float batonSwingAngle = 90f;
    [SerializeField] private float batonSwingTime = 0.25f;
    [SerializeField] private float batonCooldown = 1.5f;

    private float lastBatonTime = -999f;
    private bool isSwinging = false;

    [Header("Audio")]
    public AudioClip lawManSound;
    public AudioClip lawManHitSound;
    public AudioClip punch;
    public AudioClip succPunch;

    protected override void Awake()
    {
        base.Awake();
        swingPunch = punch;
        successPunch = succPunch;
    }

    // Instead of normal attack, LawMan swings his baton
    protected override void PerformSpecial()
    {
        if (isSwinging) return;
        if (Time.time < lastBatonTime + batonCooldown) return;

        lastBatonTime = Time.time;
        StartCoroutine(BatonSwingCoroutine());
    }

    public float GetBatonHitStrength()
    {
        return batonHitStrength;
    }

    private IEnumerator BatonSwingCoroutine()
    {
        if (batonObject == null)
        {
            Debug.LogWarning("Baton object not assigned on " + name);
            yield break;
        }

        isSwinging = true;
        batonObject.SetActive(true);
        hitBox.gameObject.SetActive(true);
        if (audioSource != null && lawManSound != null)
        {
            audioSource.PlayOneShot(lawManSound); // non?overlapping SFX [web:152][web:160]
        }

        // Decide facing direction:
        // Prefer lookInput (right stick / mouse), fall back to moveInput, then default right
        Vector2 dir = Vector2.right;
        if (lookInput.sqrMagnitude > 0.01f)
            dir = lookInput.normalized;
        else if (moveInput.sqrMagnitude > 0.01f)
            dir = moveInput.normalized;

        float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Do NOT zero out player rotation, OnLook in parent handles that.
        // Just rotate the baton locally around the player.
        Quaternion startRot = Quaternion.Euler(0f, 0f, baseAngle - batonSwingAngle * 0.5f);
        Quaternion endRot = Quaternion.Euler(0f, 0f, baseAngle + batonSwingAngle * 0.5f);

        batonObject.transform.localRotation = startRot;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / batonSwingTime;
            batonObject.transform.localRotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        batonObject.SetActive(false);
        hitBox.gameObject.SetActive(false);
        isSwinging = false;
    }
    public void OnBatonHit()
    {
        Debug.Log("Baton hit");

        if (audioSource != null && lawManHitSound != null)
        {
            StartCoroutine(PlayHitDelayed(lawManHitSound, 0.2f));
        }
    }
    private IEnumerator PlayHitDelayed(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (audioSource != null)
        {
            Debug.Log("Clip played");
            audioSource.PlayOneShot(clip);
        }
    }
}
