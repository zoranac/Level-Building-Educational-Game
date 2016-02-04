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
        UpdateSwitchState();
        ApplySwitch();
    }
    void ApplySwitch()
    {
        if (dotTile.GetComponent<DotTileScript>().Powered)
        {
            foreach (GameObject obj in dotTile.GetComponent<DotTileScript>().Connections)
            {
                if (SwitchState)
                {
                    if (obj.GetComponent<PowerLineScript>().Power == 0)
                        obj.GetComponent<PowerLineScript>().SetPower(PowerOutput.MaxPower,gameObject);
                }
                else {
                    //if ((obj.transform.position.x > dotTile.transform.position.x && obj.GetComponent<PowerLineScript>().CurrentFlowDirection == PowerLineScript.FlowDirection.) ||
                    //    (obj.transform.position.x < dotTile.transform.position.x && OutputLocation == Location.Right) ||
                    //    (obj.transform.position.y > dotTile.transform.position.y && OutputLocation == Location.Top) ||
                    //    (obj.transform.position.y < dotTile.transform.position.y && OutputLocation == Location.Bottom))
                    obj.GetComponent<PowerLineScript>().SetPower(0,gameObject);
                }
            }
        }
    }
	void UpdateSwitchState()
	{
        if (connectedObject != null)
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
