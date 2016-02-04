using UnityEngine;
using System.Collections;

public class SwitchScript : InteractableObject {
	public bool On;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    override public void Interact()
	{
		On = !On;
	}
}
