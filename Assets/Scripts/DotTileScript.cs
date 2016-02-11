using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DotTileScript : MonoBehaviour {
	public GameObject TileAbove;
	public GameObject TileBelow;
	public GameObject TileLeft;
	public GameObject TileRight;
	public List<GameObject> Connections = new List<GameObject>();
	bool Tested = false;
	public bool Powered = false;
	public int Power = 0;
	public GameObject PowerSourceObj;
	public GameObject ObjectOnMe;
	// Use this for initialization
	void Start () {
		
	}
	void TestIfPowered(){

		int highestPower = 0;

		if (ObjectOnMe != null && 
		    ObjectOnMe.GetComponent<PowerOutput>() != null && 
		    ObjectOnMe.GetComponent<PowerOutput>().powerOutput > 0
		    )
		{
			highestPower = ObjectOnMe.GetComponent<PowerOutput>().powerOutput;
		}
		foreach(GameObject obj in Connections)
		{
			if (obj.GetComponent<PowerLineScript>().Power > highestPower)
			{
				highestPower = obj.GetComponent<PowerLineScript>().Power;
				PowerSourceObj = obj.GetComponent<PowerLineScript>().PowerSourceObj;
			}
		}
		Power = highestPower;

		if (ObjectOnMe != null && 
		    ObjectOnMe.GetComponent<TempPowerOutput>() != null)
		{
			if (PowerSourceObj == null)
				Power = 0;
			if (ObjectOnMe.GetComponent<TempPowerOutput>().tempPowerOutput == 0)
				Power = 0;
		}


//		else if (ObjectOnMe != null && 
//		         ObjectOnMe.GetComponent<TempPowerOutput>() != null && 
//		         ObjectOnMe.GetComponent<TempPowerOutput>().tempPowerOutput > highestPower)
//		{
//			Power = ObjectOnMe.GetComponent<TempPowerOutput>().tempPowerOutput;
//		}

		if (Power > 0)
		{
			Powered = true;
		}
		else
		{
			Powered = false;
		}
	}
	// Update is called once per frame
	void Update () {
        TestIfPowered();
		if (!Tested)
		{
			foreach (RaycastHit2D ray in Physics2D.RaycastAll(transform.position,Vector2.up,.5f)){
				if (ray.collider.gameObject != this.gameObject){
					if (ray.collider.tag == "DotTile")
					{
						TileAbove = ray.collider.gameObject;
						break;
					}
				}
			}
			foreach (RaycastHit2D ray in Physics2D.RaycastAll(transform.position,Vector2.down,.5f)){
				if (ray.collider.gameObject != this.gameObject){
					if (ray.collider.tag == "DotTile")
					{
						TileBelow = ray.collider.gameObject;
						break;
					}
				}
			}
			foreach (RaycastHit2D ray in Physics2D.RaycastAll(transform.position,Vector2.left,.5f)){
				if (ray.collider.gameObject != this.gameObject){
					if (ray.collider.tag == "DotTile")
					{
						TileLeft = ray.collider.gameObject;
						break;
					}
				}
			}
			foreach (RaycastHit2D ray in Physics2D.RaycastAll(transform.position,Vector2.right,.5f)){
				if (ray.collider.gameObject != this.gameObject){
					if (ray.collider.tag == "DotTile")
					{
						TileRight = ray.collider.gameObject;
						break;
					}
				}
			}
			Tested = true;
		}
	}
}
