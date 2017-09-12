using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiClass : MonoBehaviour {

	void OnGUI () {
        
        GUI.Box(new Rect(10,10,100,90), "Health");
    }
}
