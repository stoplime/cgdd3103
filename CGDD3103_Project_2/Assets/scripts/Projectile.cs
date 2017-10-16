using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	[Tooltip("The damage each projectile will do.")]
	public float damage = 10f;

	[Tooltip("The time it takes for the projectile to die.")]
	public float decayTime;

	private float timer;
	
	public LayerMask layerMask = -1; //make sure we aren't in this layer 
	public float skinWidth = 0.1f; //probably doesn't need to be changed 

	// private float minimumExtent; 
	// private float partialExtent; 
	// private float sqrMinimumExtent; 
	// private Vector3 previousPosition; 
	// private Rigidbody myRigidbody;
	// private Collider myCollider;

	void OnCollisionEnter(Collision col)
	{
		CheckCollision(col);
	}

	private void CheckCollision(Collision col)
	{
		if(col.gameObject.tag == "Enemy")
		{
			col.gameObject.SendMessage("TakeDamage", damage);
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		timer = decayTime;
		// myRigidbody = GetComponent<Rigidbody>();
	   	// myCollider = GetComponent<Collider>();
		// previousPosition = myRigidbody.position; 
		// minimumExtent = Mathf.Min(Mathf.Min(myCollider.bounds.extents.x, myCollider.bounds.extents.y), myCollider.bounds.extents.z);
		// partialExtent = minimumExtent * (1.0f - skinWidth);
		// sqrMinimumExtent = minimumExtent * minimumExtent;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0)
		{
			Destroy(gameObject);
		}
	}

	// void FixedUpdate() 
	// { 
	//    //have we moved more than our minimum extent? 
	//    Vector3 movementThisStep = myRigidbody.position - previousPosition; 
	//    float movementSqrMagnitude = movementThisStep.sqrMagnitude;
 
	//    if (movementSqrMagnitude > sqrMinimumExtent) 
	// 	{ 
	//       float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
	//       RaycastHit hitInfo; 
 
	//       //check for obstructions we might have missed 
	//       if (Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, layerMask.value))
    //           {
    //              if (!hitInfo.collider)
    //                  return;
 
    //              if (hitInfo.collider.isTrigger) 
    //                  hitInfo.collider.SendMessage("OnTriggerEnter", myCollider);
 
    //              if (!hitInfo.collider.isTrigger)
    //                  myRigidbody.position = hitInfo.point - (movementThisStep / movementMagnitude) * partialExtent; 
 
    //           }
	//    } 
 
	//    previousPosition = myRigidbody.position; 
	// }
}
