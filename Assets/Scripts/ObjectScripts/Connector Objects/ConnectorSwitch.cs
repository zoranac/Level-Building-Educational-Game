using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ConnectorSwitch : TempPowerOutput {
	public enum Location
	{
		Top,
		Bottom,
		Left,
		Right
	}; 
	bool Connected = false;
	GameObject connectedObject;
	[Editable(true)]
	public bool SwitchState = false;
	//input and output directions

	// Update is called once per frame
	void Update () {
		TestIfConnected();
        UpdateSwitchState();
        ApplySwitch();
    }
    void ApplySwitch()
    {
		//test power output based on switch bool
		if (SwitchState)
		{
			tempPowerOutput = this.PowerOutput;
			//print(Inputs.Count);
			if (Inputs.Count == 0)
			{
				ResetPower();
				foreach (GameObject obj in dotTile.GetComponent<DotTileScript>().Connections)
				{
					obj.GetComponent<PowerLineScript>().SetPower(tempPowerOutput,gameObject,dotTile.GetComponent<DotTileScript>().PowerSourceObj);
				}
			}
		}
		else
		{
			ResetPower();
		}
		//if the dot has power from a 
		if (dotTile.GetComponent<DotTileScript>().Powered)
        {
            foreach (GameObject obj in Outputs)
            {

                if (SwitchState)
                {
					if (obj.GetComponent<PowerLineScript>().Power == 0)
						obj.GetComponent<PowerLineScript>().SetPower(tempPowerOutput,gameObject,dotTile.GetComponent<DotTileScript>().PowerSourceObj);
                }
                else {
                    //if ((obj.transform.position.x > dotTile.transform.position.x && obj.GetComponent<PowerLineScript>().CurrentFlowDirection == PowerLineScript.FlowDirection.) ||
                    //    (obj.transform.position.x < dotTile.transform.position.x && OutputLocation == Location.Right) ||
                    //    (obj.transform.position.y > dotTile.transform.position.y && OutputLocation == Location.Top) ||
                    //    (obj.transform.position.y < dotTile.transform.position.y && OutputLocation == Location.Bottom))
                    //if (obj.GetComponent<PowerLineScript>().highestPowerObj == gameObject)
					obj.GetComponent<PowerLineScript>().SetPower(tempPowerOutput,gameObject,dotTile.GetComponent<DotTileScript>().PowerSourceObj);
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
	public override void ValueChanged(object sender, object value)
	{
		//System.Boolean
		//System.Int32
		//System.Decimal
		if (sender.ToString() == "System.Boolean SwitchState")
		{
			SwitchState = bool.Parse(value.ToString());
			if (connectedObject != null) 
				connectedObject.GetComponent<SwitchScript>().On = SwitchState;
		}
		if (sender.ToString() == "System.Int32 PowerOutput")
		{
			PowerOutput = int.Parse(value.ToString());
			if (SwitchState)
			{
				foreach (GameObject obj in Outputs)
				{
					obj.GetComponent<PowerLineScript>().SetPower(PowerOutput,gameObject,dotTile.GetComponent<DotTileScript>().PowerSourceObj);
				}
			}
		}
	}
}
