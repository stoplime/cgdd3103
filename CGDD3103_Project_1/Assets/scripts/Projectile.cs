using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	[Tooltip("The damage each projectile will do.")]
	public float damage = 10f;

	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.tag == "Enemy")
		{
			col.gameObject.SendMessage("TakeDamage", damage);
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
