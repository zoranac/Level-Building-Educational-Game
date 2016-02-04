using UnityEngine;
using System.Collections;

public class PoweredObject : MonoBehaviour {
    public bool Powered = false;
	
	// Update is called once per frame
	void FixedUpdate () {
		TestIfPowered();
	}
	void TestIfPowered(){
		foreach (Collider2D col in Physics2D.OverlapPointAll(transform.position)){
			if (col.tag == "DotTile"){
				Powered = col.GetComponent<DotTileScript>().Powered;
			}
		}
	}
}
