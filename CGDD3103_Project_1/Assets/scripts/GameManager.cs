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
	};

	public static Dictionary<string, KeyCode> DefaultKeyConfig2
	{
		get
		{
			return defaultKeyConfig1;
		}
	}

	private static Dictionary<string, KeyCode> defaultKeyConfig2 = new Dictionary<string, KeyCode>()
	{
		{"forward", KeyCode.UpArrow},
		{"backward", KeyCode.DownArrow},
		{"left", KeyCode.LeftArrow},
		{"right", KeyCode.RightArrow},
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
