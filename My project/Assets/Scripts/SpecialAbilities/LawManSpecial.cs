using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class LawManSpecial : CharacterParentClass
{
    [Header("Baton Settings")]
    [SerializeField] private GameObject batonObject;
    [SerializeField] private float batonSwingAngle = 90f;
    [SerializeField] private float batonSwingTime = 0.25f;
    [SerializeField] private float batonCooldown = 1.5f;

    private float lastBatonTime = -999f;
    private bool isSwinging = false;

    // Instead of having a special, LawMan swings his baton
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

     /*   // Decide facing direction from input
        Vector2 dir = inputVector.sqrMagnitude > 0.01f
            ? inputVector.normalized
            : Vector2.right;

        // Set baton facing direction
        float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.identity;
        batonObject.transform.localRotation = Quaternion.Euler(0f, 0f, baseAngle - batonSwingAngle * 0.5f);
        // Set rotation
        float t = 0f;
        Quaternion startRot = batonObject.transform.localRotation;
        Quaternion endRot = Quaternion.Euler(0f, 0f, baseAngle + batonSwingAngle * 0.5f);
        // Swing baton
        while (t < 1f)
        {
            t += Time.deltaTime / batonSwingTime;
            batonObject.transform.localRotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }*/
        // Reset everything
        batonObject.SetActive(false);
        isSwinging = false;
        hitBox.gameObject.SetActive(false);
    }
}

