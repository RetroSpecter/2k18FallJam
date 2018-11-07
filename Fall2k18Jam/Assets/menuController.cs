using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void startGame()
    {
        Debug.Log("loading main scene");
        GameManager.instance.StartGame();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
