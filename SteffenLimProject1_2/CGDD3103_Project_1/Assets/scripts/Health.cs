using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

	GameObject parent;
	Enemy script;
	private float currentBarLength;
	private Vector3 scaleOrg;

	// Use this for initialization
	void Start () {
		scaleOrg = transform.localScale;
		parent = transform.parent.gameObject;
		script = parent.GetComponent<Enemy>();
	}
	
	// Update is called once per frame
	void Update () {
		currentBarLength = script.Health/script.maxHealth;
		transform.LookAt(Camera.main.transform);
		transform.Rotate(0, 180, 0);

		transform.localScale = Vector3.Lerp(scaleOrg, new Vector3(currentBarLength * scaleOrg.x, scaleOrg.y, scaleOrg.z), Time.fixedTime);
	}
}
