using UnityEngine;
using System.Collections;

public class PoweredDoor : PoweredObject {
    public Sprite[] sprites = new Sprite[2];
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<BoxCollider2D>().enabled = !Powered;
        if (Powered)
            GetComponent<SpriteRenderer>().sprite = sprites[1];
        else
            GetComponent<SpriteRenderer>().sprite = sprites[0];
	}

}
