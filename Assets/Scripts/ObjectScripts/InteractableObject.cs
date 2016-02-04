using UnityEngine;
using System.Collections;

public class InteractableObject : PlaceableObject {
    public bool AttachedToGenerator;
    public bool On;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Interact()
	{
		On = !On;
	}
}
