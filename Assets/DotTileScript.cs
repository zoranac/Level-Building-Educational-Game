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
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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
