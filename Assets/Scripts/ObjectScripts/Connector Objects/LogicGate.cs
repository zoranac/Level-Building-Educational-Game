using UnityEngine;
using System.Collections;

public class LogicGate : TempPowerOutput {
	//[Editable(true)]
	bool GateOpen = false;
	// Update is called once per frame
	void Update () {
		TestConnection();
		TestANDGate();
	}
	void TestANDGate(){
		if (Inputs.Count >= 2 && Outputs.Count >= 1)
		{
			GateOpen = false;
			tempPowerOutput = 0;
			foreach(GameObject input in Inputs)
			{
				if(input.GetComponent<PowerLineScript>().Power <= 0)
				{
					foreach(GameObject output in Outputs)
					{
						output.GetComponent<PowerLineScript>().SetPower(tempPowerOutput,gameObject,dotTile.GetComponent<DotTileScript>().PowerSourceObj);
					}
					GateOpen = false;
					return;
				}
			}
			tempPowerOutput = this.PowerOutput;
			foreach(GameObject output in Outputs)
			{
				if (output.GetComponent<PowerLineScript>().Power == 0)
					output.GetComponent<PowerLineScript>().SetPower(tempPowerOutput,gameObject,dotTile.GetComponent<DotTileScript>().PowerSourceObj);
			}
			GateOpen = true;
		}
		else
		{
			foreach(GameObject output in Outputs)
			{
				output.GetComponent<PowerLineScript>().SetPower(tempPowerOutput,gameObject,dotTile.GetComponent<DotTileScript>().PowerSourceObj);
			}
			GateOpen = false;
		}
	}
	void PowerOutupts(){

	}
	void TestConnection(){

	}
	public override void ValueChanged(object sender, object value)
	{
		if (sender.ToString() == "System.Int32 PowerOutput")
		{
			PowerOutput = int.Parse(value.ToString());
			if (GateOpen)
			{
				foreach (GameObject obj in Outputs)
				{
					obj.GetComponent<PowerLineScript>().SetPower(PowerOutput,gameObject,dotTile.GetComponent<DotTileScript>().PowerSourceObj);
				}
			}
		}
	}
}
