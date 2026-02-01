using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
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
    [SerializeField] protected GameObject hitBox; // Child GameObject with Collider(Trigger)
    [SerializeField] private GameObject hurtbox; // Child GameObject with Collider(Trigger)
    [SerializeField] protected float currentSpecial = 1f;
    [SerializeField] protected float maxSpecial = 100f;

    public string playerID;
    public string controllerID;

    [SerializeField] protected SpecialBarManager specialBar; //this needs a slider ui

    protected Rigidbody2D rb;
    protected Vector2 moveInput;
    protected Vector2 lookInput;
    protected Vector3 lookDir;
    protected bool isAttacking = false;
    protected bool specialReady = false;

    protected  void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Set drag to simulate friction
        rb.linearDamping = friction;
        rb.gravityScale = 0; // Assuming top-down; set to 1 for platformer
        hitBox.gameObject.SetActive(false);
        playerID = gameObject.name;

        currentSpecial = 1f;
        maxSpecial = 200f;

    }

    protected virtual void Update()
    {
    
        GainSpecial(currentSpecial);
        Death();
     
    }

    protected virtual void Death()
    {
        //death
        /*    if (rb.position.y < -3 || rb.position.y > 3) //these values can be changed for the real board
            {
                this.gameObject.SetActive(false);
            }
            if (rb.position.x < -3 || rb.position.x > 3) //these values can be changed for the real board
            {
                this.gameObject.SetActive(false);
            }*/
        if (Mathf.Abs(rb.position.y) > 3 || Mathf.Abs(rb.position.x) > 3)
        {
            this.gameObject.SetActive(false);
        }
    }


    // --- Input System Events ---

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();

        lookDir = new Vector3(lookInput.x, lookInput.y, 0);

        //Apply different look code depending on controllerID
        if (controllerID == "Mouse")
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(lookDir);
            lookDir = (new Vector3(mousePos.x, mousePos.y, 0) - new Vector3(transform.position.x, transform.position.y, 0)).normalized;
        }
        else
        {
            lookDir.Normalize();
        }

        /*if (lookInput != Vector2.zero)
        {
            Quaternion rot = Quaternion.FromToRotation(transform.up, lookDir);
            //float angle = Mathf.Atan2(lookInput.y, lookInput.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation, rot * transform.rotation, 1 / (weight + 1) * Time.deltaTime * 25);
        }*/
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed && !isAttacking && Time.time >= lastAttackTime + attackCooldown)
        {
            StartCoroutine(Attack());
        }
    }

    public void OnSpecial(InputValue value)
    {
        if (value.isPressed && currentSpecial >= maxSpecial && !specialReady)
        {
            StartCoroutine(Special());
        }
    }



    protected void FixedUpdate()
    {
        Move();
        Friction();
        
    }
    //New Input System
   //Movement (Keyboard and Controller)
 /*   protected virtual void HandleInput()
    {
       // inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));// this made me change the settings to allow BOTH old and new input systems.
    }*/

    protected virtual void Move()
    {
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, moveInput * Speed, friction * Time.deltaTime * 10);

        //Apply gradual rotation based on weight factor
        if (lookDir != Vector3.zero)
        {
            Quaternion rot = Quaternion.FromToRotation(transform.up, lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot * transform.rotation, 1 / weight * Time.deltaTime * 25);
        }
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
       
      
    }
    public float GetStrength()
    {
        return hitStrength;
    }

    //Weight
    public virtual void TakeKnockback(Vector2 direction, float force)
    {
       // Knockback
        // Reduce force based on weight
        float finalForce = force / weight;
        rb.AddForce(direction * finalForce, ForceMode2D.Impulse);
    }

    

    //special
    protected virtual void PerformSpecial()
    {
        Debug.Log("RAHHHHHH");
        //PUT character special code here.
      

    }
    // gain meter
    public void GainSpecial(float amount)
    {
        currentSpecial = Mathf.Clamp(currentSpecial + (amount * Time.deltaTime), 0f, maxSpecial);
        specialBar.SetSpecialBar(currentSpecial);
    }
    protected virtual IEnumerator Special()
    {
        // Check if meter is full
        if (currentSpecial >= maxSpecial && !specialReady)
        {
            specialReady = true;
            hitBox.gameObject.SetActive(true);
            PerformSpecial();
            // Consume the meter
            currentSpecial = 1f;
            yield return new WaitForSeconds(1f);
            specialReady = false;
            hitBox.gameObject.SetActive(false);
        }
    }

   
    public void changeStat(int id, int value, bool addative) //writes to player stats, when called with addative set to true, will add value instead of overwriting
    {
        switch (id) //not the nicest code block but should do the job
        {
            case 0:
                if (addative) Speed += value;
                else Speed = value;
                break;
            case 1:
                if (addative) weight += value;
                else weight = value;
                break;
            case 2:
                if (addative) friction += value;
                else friction = value;
                break;
            case 3:
                if (addative) hitStrength += value;
                else hitStrength = value;
                break;
            case 4:
                if (addative) attackCooldown += value;
                else attackCooldown = value;
                break;
        }
    }
  


}
