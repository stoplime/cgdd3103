using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FriendlyAIstate
{
	Follow,
	Attack
}

public class Friendly : MonoBehaviour {

	public FriendlyAIstate current_state = FriendlyAIstate.Follow;
	
	public GameObject[] enemiesObject;

	public float maxHealth = 100f;

	public float damage = 10f;

	public float health;

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

	public float healingTime = 10;

	public float healingAmount = 20f;

	public float personalSpace = 2f;

	public float followRadius = 4f;

	public float detectRadius = 5f;

	public float regenTime = 10;

	public float timer;

	
	private Transform target;

	private Transform playerTransform;

	private UnityEngine.AI.NavMeshAgent agent;

	private List<Transform> enemies;

	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.tag == "Enemy")
		{
			col.gameObject.SendMessage("TakeDamage", damage);
			Destroy(gameObject);
		}
	}

	private void Behaviors()
	{
		switch (current_state)
		{
		case FriendlyAIstate.Attack:
			// attacking the Enemy
			if (target == null)
			{
				current_state = FriendlyAIstate.Follow;
				break;
			}
			// if too far from the player, needs to go back to the player
			if (Vector3.Distance(transform.position, playerTransform.position) > followRadius*2f)
			{
				current_state = FriendlyAIstate.Follow;
				target = playerTransform;
				break;
			}
			// Attack the enemy

			
			// if there is no Enemy in sight
			if (Vector3.Distance(transform.position, target.position) > detectRadius)
			{
				target = null;
			}
			break;
		case FriendlyAIstate.Follow:
			// Follow the player
			float playerDist = Vector3.Distance(transform.position, playerTransform.position);

			if (playerDist > followRadius)
			{
				target = playerTransform;
			}
			// not too close
			else if (playerDist < personalSpace)
			{
				target = null;
			}

			// check for enemies in the detect radius
			for (int i = 0; i < enemies.Count; i++)
			{
				if (Vector3.Distance(transform.position, enemies[i].position) < detectRadius)
				{
					target = enemies[i];
					current_state = FriendlyAIstate.Attack;
					break;
				}
			}
			break;
			
		default:
			break;
		}
	}

	// Use this for initialization
	void Start () {
		health = maxHealth;
		agent = GetComponentInParent<UnityEngine.AI.NavMeshAgent>();
		timer = regenTime;
		enemies = new List<Transform>();
		enemiesObject = GameObject.FindGameObjectsWithTag("Enemy");
		for (int i = 0; i < enemiesObject.Length; i++)
		{
			enemies.Add(enemiesObject[i].transform);
		}
	}
	
	// Update is called once per frame
	void Update () {
		playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		Behaviors();
		if (target != null)
		{
			agent.SetDestination(target.position);
		}
		else
		{
			agent.ResetPath();
		}

		timer -= Time.deltaTime;
		if (health < maxHealth)
		{
			if (timer <= 0)
			{
				Health = health + healingAmount;
				timer = regenTime;	
			}
		}
		if (health == 0)
		{
			Destroy(gameObject);
			return;
		}
		transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
	}
}
