using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {
	const float PI = 3.1415926f;

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

	public float timer;

	public int Profile { get; set; }

    public Rigidbody projectile;

    public Rigidbody projectile2;

	public float RareProjectileProb;

	private bool grounded = false;

	private Rigidbody rb;

	public float jumpForce = 5f;

	private InventoryControl inventory;

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

	public void HotbarSelect()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			inventory.SelectedHotbar = 0;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			inventory.SelectedHotbar = 1;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			inventory.SelectedHotbar = 2;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			inventory.SelectedHotbar = 3;
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

	void OnCollisionStay(Collision col)
	{
		if (col.gameObject.tag == "Ground")
		{
			grounded = true;
		}
	}

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		Help.isPause = false;
		timer = regenTime;
		Profile = 0;
		keyProfile = new Dictionary<string, KeyCode>[2];
		keyProfile[0] = GameManager.DefaultKeyConfig1;
		keyProfile[1] = GameManager.DefaultKeyConfig2;
		rb = GetComponent<Rigidbody>();
		inventory = GetComponentInParent<InventoryControl>();
	}
	// Update is called once per frame
	void Update () {
		rb.angularVelocity = Vector3.zero;

		if (Input.GetKeyDown(keyProfile[Profile]["inventory"]))
		{
			inventory.OpenInventoryToggle = !inventory.OpenInventoryToggle;
		}

		if (!Help.isPause)
		{
			Cursor.lockState = CursorLockMode.Locked;

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

			if (Input.GetKeyDown(keyProfile[Profile]["jump"]))
			{
				if (grounded)
				{
					rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
					grounded = false;
				}
			}

			// firing action
			if (Input.GetKeyDown(keyProfile[Profile]["fire"]))
			{
				Rigidbody clone = null;
				
				if (inventory.getHotbarItemID(inventory.SelectedHotbar) == 0)
				{
					clone = Instantiate(projectile, transform.position + Camera.main.transform.forward * 1.5f, Camera.main.transform.rotation) as Rigidbody;
					inventory.removeOneHotbarItem(inventory.SelectedHotbar);
				}
				else if (inventory.getHotbarItemID(inventory.SelectedHotbar) == 1)
				{
					clone = Instantiate(projectile2, transform.position + Camera.main.transform.forward * 1.5f, Camera.main.transform.rotation) as Rigidbody;
					inventory.removeOneHotbarItem(inventory.SelectedHotbar);
				}
				if (clone != null)
				{
					clone.transform.rotation = Camera.main.transform.rotation;
					clone.transform.Rotate(90, 0, 0);
					// clone.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * projectileSpeed);
					clone.velocity = Camera.main.transform.forward * projectileSpeed;
				}
			}

			HotbarSelect();

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
		}
		else{
			Cursor.lockState = CursorLockMode.None;
			transform.Translate(Vector3.zero);
		}
		transform.eulerAngles = new Vector3(0f, yaw, 0.0f);
	}
}
