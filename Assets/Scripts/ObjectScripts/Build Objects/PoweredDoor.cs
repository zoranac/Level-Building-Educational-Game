using UnityEngine;
using System.Collections;

public class PoweredDoor : PoweredObject {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<BoxCollider2D>().enabled = !Powered;
	}

}
