using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

	[Tooltip("Max health value")]
	public float maxHealth = 100;

	[Tooltip("Health value")]
	public float health = 100;

	[Tooltip("Yaw speen for the player, camera is fixed onto the yaw of the player")]
	public float cameraSpeedH = 2.0f;

	[Tooltip("Speed for strafing left and right.")]
	public float speedH = 2.0f;

	[Tooltip("Speed for forward and backward motion.")]
    public float speedV = 2.0f;

	[Tooltip("Speed for projectiles.")]
    public float projectileSpeed = 20.0f;

	public float regenAmount = 2f;

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

	/// <summary>
	/// used to keep track of the current yaw value
	/// </summary>
    private float yaw = 0.0f;

	/// <summary>
	/// tracks the velocity of the character
	/// </summary>
	private Vector3 vel;

	private Dictionary<string, KeyCode>[] keyProfile;

	public float regenTime = 10;

	private float timer;

	public int Profile { get; set; }

	public bool MovementLock{ get; set; }
	
    public Rigidbody projectile;

	public float Yaw
	{
		get
		{
			return Help.angleClamp(yaw, true);
		}
		set
		{
			yaw = Help.angleClamp(value, true);
		}
	}

	/// <summary>
	/// sets the keycode of a given profile and key
	/// </summary>
	/// <param name="profile"></param>
	/// <param name="key"></param>
	/// <param name="code"></param>
	public void setKeyProfile(int profile, string key, KeyCode code)
	{
		if (profile < keyProfile.Length && profile >= 0)
		{
			if (keyProfile[profile].ContainsKey(key))
			{
				keyProfile[profile][key] = code;
			}
		}
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Enemy")
		{
			Health = health - 5;
			timer = regenTime;
		}
	}

	// Use this for initialization
	void Start () {
		timer = regenTime;
		Profile = 0;
		keyProfile = new Dictionary<string, KeyCode>[2];
		keyProfile[0] = GameManager.DefaultKeyConfig1;
		keyProfile[1] = GameManager.DefaultKeyConfig2;
	}
	// Update is called once per frame
	void Update () {
		if (!MovementLock)
		{
			if (Input.GetKey(keyProfile[Profile]["right"]))
			{
				vel.x = Time.deltaTime * speedH;
			}
			else if (Input.GetKey(keyProfile[Profile]["left"]))
			{
				vel.x = -Time.deltaTime * speedH;
			}
			else
			{
				vel.x = 0;
			}

			if (Input.GetKey(keyProfile[Profile]["forward"]))
			{
				vel.z = Time.deltaTime * speedV;
			}
			else if (Input.GetKey(keyProfile[Profile]["backward"]))
			{
				vel.z = -Time.deltaTime * speedV;
			}
			else
			{
				vel.z = 0;
			}

			// firing action
			if (Input.GetKeyDown(keyProfile[Profile]["fire"]))
			{
				Rigidbody clone;
				clone = Instantiate(projectile, transform.position + transform.rotation * Vector3.forward, Camera.main.transform.rotation) as Rigidbody;
				Vector3 trajection = Camera.main.transform.rotation * Vector3.forward;
				// clone.transform.LookAt(Camera.main.transform);
				clone.transform.Rotate(90, 0, 0);
				clone.velocity = transform.TransformDirection(Vector3.forward * projectileSpeed);
			}

			timer -= Time.deltaTime;
			if (health < maxHealth)
			{
				if (timer <= 0)
				{
					Health = health + regenAmount;
					timer = regenTime;	
				}
			}

			transform.Translate(vel);

			yaw += cameraSpeedH * Input.GetAxis("Mouse X");
			yaw = Help.angleClamp(yaw, true);

			transform.eulerAngles = new Vector3(0f, yaw, 0.0f);
		}
	}
}
