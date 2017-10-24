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

	public GameObject WaypointsObject;

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

	public float detectRange = 6f;

	public float followRange = 12f;

	public float reachTargetDist = 2f;

	public float bravery = 0.5f;

	public float boredom = 0.5f;

	// public float speed;

	// public float fireSpeed;

	private float timer;

	private List<Transform> waypoints;

	private UnityEngine.AI.NavMeshAgent agent;

	private Transform target;

	private Transform playerTransform;

	private float preHealth;

	private bool checkedBravery = false;

	private float idleTimer = 10f;

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
			target = playerTransform;

			// check if the player is no longer within range, then patrol
			if (Vector3.Distance(transform.position, playerTransform.position) > followRange)
			{
				current_state = AIstate.Patrol;
			}

			// if health is low, check if it should flee
			if (!checkedBravery && health < 0.25*maxHealth)
			{
				checkedBravery = true;
				if (Random.Range(0f, 1f) > bravery)
				{
					// flee
					findFleeTarge();
					current_state = AIstate.Flee;
				}
			}
			break;
		case AIstate.Patrol:
			// Walk around waypoints
			if (target == null)
			{
				target = waypoints[Random.Range(0, waypoints.Count)];
			}
			// reaches the target waypoint, choose new waypoint randomly
			if (Vector3.Distance(transform.position, target.position) < reachTargetDist)
			{
				if (Random.Range(0f,1f) > boredom)
				{
					current_state = AIstate.Idle;
				}
				else
				{
					target = waypoints[Random.Range(0, waypoints.Count)];
				}
			}
			// check if got damaged then go into attack mode
			if (preHealth - health > 0.01)
			{
				current_state = AIstate.Attack;
			}
			// check if the player is within range, then attack
			if (Vector3.Distance(transform.position, playerTransform.position) < detectRange)
			{
				current_state = AIstate.Attack;
			}
			break;
		case AIstate.Idle:
			// do Idle animation
			target = null;
			// check if got damaged then go into attack mode
			if (preHealth - health > 0.01)
			{
				current_state = AIstate.Attack;
			}
			// check if the player is within range, then attack
			if (Vector3.Distance(transform.position, playerTransform.position) < detectRange)
			{
				current_state = AIstate.Attack;
			}

			// every 10 seconds, check to see if it should patrol
			if (idleTimer <= 0)
			{
				idleTimer = 10f;
				if (Random.Range(0f, 1f) > boredom)
				{
					current_state = AIstate.Idle;
				}
				else
				{
					current_state = AIstate.Patrol;
				}
			}
			else
			{
				idleTimer -= Time.deltaTime;
			}
			
			// reset bravery check
			checkedBravery = false;
			break;
		case AIstate.Flee:
			// run away
			// if flee target is reached
			if (Vector3.Distance(transform.position, target.position) < reachTargetDist)
			{
				current_state = AIstate.Idle;
			}
			
			break;
			
		default:
			break;
		}
		preHealth = health;
	}

	private void findFleeTarge()
	{
		List<int> hiddenWaypoints = new List<int>();			
		for (int i = 0; i < waypoints.Count; i++)
		{
			Vector3 direction = (playerTransform.position - waypoints[i].position).normalized;
			Ray ray = new Ray(waypoints[i].position, direction);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				// There is no line of sight between this waypoint and the player 
				hiddenWaypoints.Add(i);
			}
		}
		// Choose a waypoint that does not have line of sight of the player to flee
		if (hiddenWaypoints.Count != 0)
		{
			target = waypoints[hiddenWaypoints[Random.Range(0, hiddenWaypoints.Count)]];
		}
	}

	// Use this for initialization
	void Start () {
		health = maxHealth;
		preHealth = health;
		agent = GetComponentInParent<UnityEngine.AI.NavMeshAgent>();
		waypoints = new List<Transform>();
		for (int i = 0; i < WaypointsObject.transform.childCount; i++)
		{
			waypoints.Add(WaypointsObject.transform.GetChild(i));
		}
	}
	
	// Update is called once per frame
	void Update () {
		playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		// playerTransform = GameObject.FindGameObjectWithTag("Respawn").GetComponent<Transform>();
		Behaviors();
		if (target != null)
		{
			agent.SetDestination(target.position);
		}
		timer -= Time.deltaTime;
		if (timer <= 0)
		{
			timer = healingTime;
			Health = health + healingAmount; 
		}

		if (health == 0)
		{
			Destroy(gameObject);
			return;
		}
		transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
	}


}
