using UnityEngine;
using System.Collections;

public class LogicGate : TempPowerOutput {
	//[Editable(true)]
    public enum GateType
    {
        AND,
        XOR
    };


	bool GateOpen = false;
    [Editable(true)]
    public GateType gateType = GateType.AND;
	// Update is called once per frame
	void Update () {
		TestConnection();
        if (gateType == GateType.AND)
        {
            TestANDGate();
        }
        else if (gateType == GateType.XOR)
        {
            TestXORGate();
        }
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
    void TestXORGate()
    {
        if (Inputs.Count >= 2 && Outputs.Count >= 1)
        {
            GateOpen = false;
            int poweredCount = 0;
            tempPowerOutput = 0;
            foreach (GameObject input in Inputs)
            {
                if (input.GetComponent<PowerLineScript>().Power > 0)
                {
                    poweredCount++;
                }
            }
            if (poweredCount == 1)
            {
                tempPowerOutput = this.PowerOutput;
                foreach (GameObject output in Outputs)
                {
                    if (output.GetComponent<PowerLineScript>().Power == 0)
                        output.GetComponent<PowerLineScript>().SetPower(tempPowerOutput, gameObject, dotTile.GetComponent<DotTileScript>().PowerSourceObj);
                }
                GateOpen = true;
            }
            else
            {
                tempPowerOutput = 0;
                foreach (GameObject output in Outputs)
                {
                    output.GetComponent<PowerLineScript>().SetPower(tempPowerOutput, gameObject, dotTile.GetComponent<DotTileScript>().PowerSourceObj);
                }
                GateOpen = false;
            }
        }
        else
        {
            foreach (GameObject output in Outputs)
            {
                output.GetComponent<PowerLineScript>().SetPower(tempPowerOutput, gameObject, dotTile.GetComponent<DotTileScript>().PowerSourceObj);
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
        print(sender.ToString());
        if (sender.ToString() == "LogicGate+GateType gateType")
        {
            if (int.Parse(value.ToString()) == 0)
            {
                gateType = GateType.AND;
            }
            if (int.Parse(value.ToString()) == 1)
            {
                gateType = GateType.XOR;
            }
        }
	}
}
