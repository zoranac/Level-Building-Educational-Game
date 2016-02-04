using UnityEngine;
using System.Collections;

public class TileSetUp : MonoBehaviour {
	public GameObject Tile;
    public GameObject ConnectorTile;
	public GameObject EditViewPlane;
    float x = -24.5f;
	float y =25f;
	float startX = -24.5f;
	float startZ = 25f;
	// Use this for initialization
	void Start () {
	    while(y > -25){
			while (x < 25.5f){
				GameObject o = (GameObject)Instantiate(Tile,new Vector3(x,y,0), new Quaternion(0,0,0,0));
				o.transform.parent = gameObject.transform;
			    x += .5f;
		    }
			x = startX;
			y-=.5f;
	    }

        x = -24.5f;
        y = 25f;
		while(y > -25){
			while (x < 25.5f){
				GameObject o = (GameObject)Instantiate(ConnectorTile,new Vector3(x,y,0), new Quaternion(0,0,0,0));
				o.transform.parent = EditViewPlane.transform;
				x += .5f;
			}
			x = startX;
			y-=.5f;
		}
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
