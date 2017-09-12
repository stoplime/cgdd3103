using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

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

	// Use this for initialization
	void Start () {
		
	}
	// Update is called once per frame
	void Update () {
		float xpos = Input.GetAxis("Horizontal") * Time.deltaTime * speedH;
		float zpos = Input.GetAxis("Vertical") * Time.deltaTime * speedV;

		transform.Translate(xpos, 0, zpos);

		yaw += cameraSpeedH * Input.GetAxis("Mouse X");
		yaw = Help.angleClamp(yaw, true);

        transform.eulerAngles = new Vector3(0f, yaw, 0.0f);
	}
}
