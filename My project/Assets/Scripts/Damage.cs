using UnityEngine;

public class Damage : MonoBehaviour
{
    //this goes on the HitBox
    public CharacterParentClass currentPlayer;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        print("HitBox collided with " + collision.gameObject.name);
        if(collision.CompareTag("HurtBox") && collision.transform.parent != null && collision.transform.parent.name != currentPlayer.playerName)
        {
            //Weight
            CharacterParentClass attackedPlayer = collision.transform.parent.GetComponent<CharacterParentClass>();
            Debug.Log("Attacked player");
            Vector2 direction = (collision.transform.parent.position - transform.parent.position).normalized;
            attackedPlayer.TakeKnockback(direction,currentPlayer.GetStrength());
            
           
        }
    }
}
