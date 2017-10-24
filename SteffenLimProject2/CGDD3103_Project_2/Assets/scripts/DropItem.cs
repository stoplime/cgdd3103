using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour {

	public int id;

	public int amount = 1;

	public float animationSpeed = 100;

	private Rigidbody rb;

	/// <summary>
	/// OnCollisionEnter is called when this collider/rigidbody has begun
	/// touching another rigidbody/collider.
	/// </summary>
	/// <param name="other">The Collision data associated with this collision.</param>
	void OnCollisionEnter(Collision other)
	{
		if (other.collider.tag == "Player")
		{
			if(other.gameObject.GetComponent<InventoryControl>().addInvItem(id, amount))
			{
				Destroy(gameObject);
			}
			else
			{
				other.gameObject.GetComponent<InventoryControl>().InventoryFullFlag = true;
			}
		}
		if (!(other.collider.tag == "Ground" || other.collider.tag == "Obstacle"))
		{
			Physics.IgnoreCollision(other.collider, GetComponent<Collider>());
		}
	}

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		rb.angularVelocity = Vector3.zero;

		transform.Rotate(0, Time.deltaTime * animationSpeed, 0, Space.World);
	}
}
