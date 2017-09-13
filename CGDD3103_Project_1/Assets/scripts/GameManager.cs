using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject mainCharacter;

	public List<GameObject> enemies;

	private List<GameObject> projectiles;

	public static Dictionary<string, KeyCode> DefaultKeyConfig1
	{
		get
		{
			return defaultKeyConfig1;
		}
	}

	private static Dictionary<string, KeyCode> defaultKeyConfig1 = new Dictionary<string, KeyCode>()
	{
		{"forward", KeyCode.W},
		{"backward", KeyCode.S},
		{"left", KeyCode.A},
		{"right", KeyCode.D},
		{"fire", KeyCode.Mouse0}
	};

	public static Dictionary<string, KeyCode> DefaultKeyConfig2
	{
		get
		{
			return defaultKeyConfig2;
		}
	}

	private static Dictionary<string, KeyCode> defaultKeyConfig2 = new Dictionary<string, KeyCode>()
	{
		{"forward", KeyCode.T},
		{"backward", KeyCode.G},
		{"left", KeyCode.F},
		{"right", KeyCode.H},
		{"fire", KeyCode.Space}
	};

	void Awake() {
		mainCharacter = GameObject.Find("MainCharacter");
	}

	// Use this for initialization
	void Start () {
		
	}

	void Update () {

	}
}
