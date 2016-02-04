using UnityEngine;
using System.Collections;

public class ConnectorSwitch : MonoBehaviour {
	public enum Location
	{
		Top,
		Bottom,
		Left,
		Right
	}; 
	bool Connected = false;
	GameObject connectedObject;
	public bool SwitchState = false;
	GameObject dotTile;
	public Location InputLocation = Location.Top;
	public Location OutputLocation = Location.Bottom;
	//input and output directions

	// Use this for initialization
	void Start () {
		foreach(Collider2D col in Physics2D.OverlapPointAll(transform.position)){
			if (col.tag == "DotTile"){
				dotTile = col.gameObject;
				break;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		TestIfConnected();
		if (InputLocation == Location.Top)
		{
			if (dotTile.GetComponent<DotTileScript>().TileAbove != null)
			{
				if (dotTile.GetComponent<DotTileScript>().Powered)
				{
					if (dotTile.GetComponent<DotTileScript>().TileBelow != null)
					{

					}
				}
			}
		}
	}
	void UpdateSwitchState()
	{
		SwitchState = connectedObject.GetComponent<SwitchScript>().On;
	}
	void TestIfConnected(){
		Connected = false;
		connectedObject = null;
		foreach(Collider2D col in Physics2D.OverlapPointAll(transform.position))
		{
			if (col.gameObject.layer == 8)
			{
				if (col.GetComponent<SwitchScript>() != null)
				{
					Connected = true;
					connectedObject = col.gameObject;
				}
			}
		}
	}
}
