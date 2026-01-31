using System.Collections;
using UnityEngine;

public class LawManSpecial : CharacterParentClass
{
    [Header("Baton Settings")]
    [SerializeField] private GameObject batonObject;
    [SerializeField] private float batonSwingAngle = 90f;
    [SerializeField] private float batonSwingTime = 0.25f;
    [SerializeField] private float batonCooldown = 1.5f;

    private float lastBatonTime = -999f;
    private bool isSwinging = false;

    // Instead of normal attack, LawMan swings his baton
    protected override void PerformAttack()
    {
        if (isSwinging) return;
        if (Time.time < lastBatonTime + batonCooldown) return;

        lastBatonTime = Time.time;
        StartCoroutine(BatonSwingCoroutine());
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
}
