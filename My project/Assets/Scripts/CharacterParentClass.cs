using System.Collections;
using UnityEngine;

public class CharacterParentClass : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] protected float Speed = 1f;
 //   [SerializeField] protected float acceleration = 50f;

    [Header("Physics Settings")]
    [SerializeField] protected float weight = 1f; // Higher = Heavier = less knockback 
    [SerializeField] protected float friction = 5f; // Higher =Heavier= stops faster

    [Header("Combat Settings")]
    [SerializeField] protected float hitStrength = 10f;
    [SerializeField] protected float attackCooldown = 1f;
    [SerializeField] protected float lastAttackTime = -999f;
    [SerializeField] private GameObject hitBox; // Child GameObject with Collider(Trigger)
    [SerializeField] private GameObject hurtbox; // Child GameObject with Collider(Trigger)



    protected Rigidbody2D rb;
    protected Vector2 inputVector;
    protected bool isAttacking = false;

    protected  void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Set drag to simulate friction
        rb.linearDamping = friction;
        rb.gravityScale = 0; // Assuming top-down; set to 1 for platformer
        hitBox.gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        HandleInput();
        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            StartCoroutine(Attack());
        }
        //death
        if (rb.position.y < -3 || rb.position.y > 3) //these values can be changed for the real board
        {
            this.gameObject.SetActive(false);
        }
        if (rb.position.x < -3 || rb.position.x > 3) //these values can be changed for the real board
        {
            this.gameObject.SetActive(false);
        }
    }

    protected  void FixedUpdate()
    {
        Move();
        Friction();
    }

    //Movement (Keyboard and Controller)
    protected  void HandleInput()
    {
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));// this made me change the settings to allow BOTH old and new input systems.
    }

    protected  void Move()
    {
        Vector2 force = inputVector.normalized * Speed;// * acceleration;
        rb.AddForce(force);
    }

    //Friction
    protected virtual void Friction()
    {
        //  can override this method for custom complex friction physics.
        rb.linearDamping = friction;
    }

    //Cooldown
    protected virtual IEnumerator Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown) yield break;

        isAttacking = true;
        PerformAttack();
        lastAttackTime = Time.time;

        yield return new WaitForSeconds(0.2f); // Attack animation time
        isAttacking = false;
        hitBox.gameObject.SetActive(false);
    }
    //Attack
    protected virtual void PerformAttack()
    {
        Debug.Log(gameObject.name + " attacked with strength: " + hitStrength);
        // 1. Activate Hitbox Object via animation event or script
        hitBox.gameObject.SetActive(true);
        hitStrength = 10;//or any value
      
    }

    //Weight
    public virtual void TakeKnockback(Vector2 direction, float force)
    {
       // Knockback
        // Reduce force based on weight
        float finalForce = force / weight;
        rb.AddForce(direction * finalForce, ForceMode2D.Impulse);
    }
  


}
