using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIstate
{
	Idle,
	Attack,
	Patrol,
	Flee
}

public class Enemy : MonoBehaviour {

	public AIstate current_state = AIstate.Idle;

	public Vector3 initPos;

	public float damage = 1;

	public float Health
	{
		get
		{
			return health;
		}
		set
		{
			if (value >= 0 && value <= maxHealth)
			{
				health = value;
			}
			else if(value > maxHealth)
			{
				health = maxHealth;
			}
			else
			{
				health = 0;
			}
		}
	}
	private float health;

	public float maxHealth = 100f;

	public float healingTime = 10;

	public float healingAmount = 20f;

	public float speed;

	public float fireSpeed;

	private float timer;

	public void TakeDamage(float dmg)
	{
		Health = health - dmg;
		timer = healingTime;
	}

	private void Behaviors()
	{
		switch (current_state)
		{
		case AIstate.Attack:
			// attacking the player
			break;
		case AIstate.Patrol:
			// Walk around waypoints
			break;
		case AIstate.Idle:
			// do Idle animation
			break;
		case AIstate.Flee:
			// run to the opposite side waypoint
			break;
			
		default:
			break;
		}
	}

	// Use this for initialization
	void Start () {
		health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0)
		{
			timer = healingTime;
			Health = health + healingAmount; 
		}

		if (health == 0)
		{
			Destroy(gameObject);
		}
		transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
	}


}
