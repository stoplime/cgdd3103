using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	[Tooltip("The damage each projectile will do.")]
	public float damage = 10f;

	[Tooltip("The time it takes for the projectile to die.")]
	public float decayTime;

	private float timer;

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
		timer = decayTime;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0)
		{
			Destroy(gameObject);
		}
	}
}
