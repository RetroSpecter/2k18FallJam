using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controlSettingsHandler : MonoBehaviour {

    public static bool mouseControl;
    public GameObject targetButton;

    private Text target;

	// Use this for initialization
	void Start () {
        if(targetButton != null)
            target = targetButton.GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        
        
	}

    public bool getMouseControls()
    {
        return mouseControl;
    }

    public void toggle()
    {
        mouseControl = !mouseControl;
        string output = "KEYBOARD";
        if (mouseControl)
            output = "MOUSE";

        target.text = "Control Mode: " + output;
    }
}
