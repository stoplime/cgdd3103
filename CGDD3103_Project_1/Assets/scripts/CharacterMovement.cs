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

	/// <summary>
	/// used to keep track of the current yaw value
	/// </summary>
    private float yaw = 0.0f;

	/// <summary>
	/// tracks the velocity of the character
	/// </summary>
	private Vector3 vel;

	private Dictionary<string, KeyCode>[] keyProfile;

	public int Profile { get; set; }

	public bool MovementLock{ get; set; }

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

	public string getKeyProfileName(string key)
	{
		return keyProfile[Profile][key].ToString();
	}

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
	public void setKeyProfile(string key, KeyCode code)
	{
		if (keyProfile[Profile].ContainsKey(key))
		{
			keyProfile[Profile][key] = code;
		}
	}

	// Use this for initialization
	void Start () {
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

			transform.Translate(vel);

			yaw += cameraSpeedH * Input.GetAxis("Mouse X");
			yaw = Help.angleClamp(yaw, true);

			transform.eulerAngles = new Vector3(0f, yaw, 0.0f);
		}
	}
}
