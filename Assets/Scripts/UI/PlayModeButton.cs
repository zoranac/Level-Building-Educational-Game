using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayModeButton : MonoBehaviour {
	GameObject control;
	// Use this for initialization
	void Start () {
		control = GameObject.Find("Control");
	}
	
	// Update is called once per frame
	void Update () {
		if (control.GetComponent<ControlScript>().CurrentMode == ControlScript.Mode.Play){
			GetComponentInChildren<Text>().text = "Return";
		}else{
			GetComponentInChildren<Text>().text = "Play";
		}
	}
}
