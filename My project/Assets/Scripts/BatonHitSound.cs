using UnityEngine;

public class BatonHitSound : MonoBehaviour
{
    public LawManSpecial currentPlayer;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HurtBox") && collision.transform.parent != null && collision.transform.parent.name != currentPlayer.playerName)
        {
            // Notify parent LawManSpecial
            //var lawman = GetComponentInParent<LawManSpecial>();
            currentPlayer.OnBatonHit();

            CharacterParentClass attackedPlayer = collision.transform.parent.GetComponent<CharacterParentClass>();
            Debug.Log("Attacked player");
            Vector2 direction = (collision.transform.parent.position - transform.parent.position).normalized;
            attackedPlayer.TakeKnockback(direction, currentPlayer.GetBatonHitStrength());
        }
    }
}