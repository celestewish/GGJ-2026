using UnityEngine;

public class BatonHitSound : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Notify parent LawManSpecial
            var lawman = GetComponentInParent<LawManSpecial>();
            lawman?.OnBatonHit();
        }
    }
}