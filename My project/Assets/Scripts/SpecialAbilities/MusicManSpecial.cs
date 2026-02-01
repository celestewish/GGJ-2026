using UnityEngine;

public class MusicManSpecial : CharacterParentClass
{
    [Header("Special Settings")]
    public float specialRadius = 3f;
    public float knockbackForce = 5f;
    public LayerMask enemyLayer;

    [Header("Audio")]
    public AudioClip specialSfx;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    public AudioClip punch;
    public AudioClip succPunch;

    [Header("Shockwave Prefab")]
    public GameObject shockwavePrefab;

    protected override void Awake()
    {
        base.Awake();
        swingPunch = punch;
        successPunch = succPunch;
    }


    // Inherited method
    protected override void PerformSpecial()
    {
        Debug.Log("Ran");
        // Play loud sound
        if (audioSource != null && specialSfx != null)
        {
            audioSource.PlayOneShot(specialSfx, sfxVolume);
        }

        // Find all enemies in radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, specialRadius, enemyLayer);

        // Apply knockback to each enemy
        foreach (Collider2D hit in hits)
        {
            Rigidbody2D enemyRb = hit.attachedRigidbody;
            if (enemyRb == null) continue;

            Vector2 direction = (enemyRb.position - (Vector2)transform.position).normalized;
            enemyRb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        }

        if (shockwavePrefab != null)
        {
            Instantiate(shockwavePrefab, transform.position, Quaternion.identity);
        }
    }
}
