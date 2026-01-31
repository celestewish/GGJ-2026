using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lumberjack3Handler : MonoBehaviour {
    public float playerTargetDistance;
    public float enemyLookDistance;
    public float attackDistance;
    public float enemyMovementSpeed;
    public float damping;
    public Transform playerTarget;
    Rigidbody theRigidbody;
    // Use this for initialization
    void Start () {
        theRigidbody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        playerTargetDistance = Vector3.Distance(playerTarget.position, transform.position);
        if (playerTargetDistance < attackDistance) {
            print("Attaaaacckkkk!");
            theRigidbody.AddForce(transform.forward * enemyMovementSpeed);
            GetComponent<Animator>().SetBool("Attack", true);
        }
        else if (playerTargetDistance < enemyLookDistance){
            print("Be alert!");
            Quaternion rotation = Quaternion.LookRotation(playerTarget.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        }
        else {
            print("At ease, soldier.");
            GetComponent<Animator>().SetBool("Attack", false);
        }
    }
}
