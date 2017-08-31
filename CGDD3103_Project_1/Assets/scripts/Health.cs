using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

	public float maximumHealth = 100f;
	private float currentHealth;
	private float currentBarLength;
	private Vector3 scaleOrg;

	// Use this for initialization
	void Start () {
		scaleOrg = transform.localScale;
		currentHealth = maximumHealth;
	}
	
	// Update is called once per frame
	void Update () {
		currentBarLength = currentHealth / maximumHealth;
		transform.LookAt(Camera.main.transform);
		transform.Rotate(0, 180, 0);
		// transform.LookAt (new Vector3(-Camera.main.transform.position.x, -Camera.main.transform.position.y, -Camera.main.transform.position.z));

		if (Input.GetAxis("Fire1")>0){
			currentHealth -= 1f;
		}

		transform.localScale = Vector3.Lerp(scaleOrg, new Vector3(currentBarLength * scaleOrg.x, scaleOrg.y, scaleOrg.z), Time.fixedTime);
	}
}
