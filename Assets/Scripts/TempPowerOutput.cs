using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TempPowerOutput : EditableObject {
	[Editable(true)]
	public int PowerOutput;

	public int tempPowerOutput;
	public GameObject dotTile;
	public List<GameObject> Inputs = new List<GameObject>();
	public List<GameObject> Outputs = new List<GameObject>();
	// Use this for initialization
	void Start () {
		tempPowerOutput = 0;
        SetDotTile();
        Move(gameObject.transform.position);
	}
    public void SetDotTile()
    {
        foreach (Collider2D col in Physics2D.OverlapPointAll(transform.position))
        {
            if (col.tag == "DotTile")
            {
                dotTile = col.gameObject;
                dotTile.GetComponent<DotTileScript>().ObjectOnMe = gameObject;
                break;
            }
        }
    }
	public void ResetPower(){
		tempPowerOutput = 0;
		dotTile.GetComponent<DotTileScript>().PowerSourceObj = null;
		dotTile.GetComponent<DotTileScript>().Power = 0;
	}
    public void ResetIO()
    {
        Inputs.Clear();
        Outputs.Clear();
    }
	public override void ValueChanged(object sender, object value)
	{
		if (sender.ToString() == "System.Int32 PowerOutput")
		{
			PowerOutput = int.Parse(value.ToString());
		}
	}
    public override void Move(Vector3 MoveToPos)
    {
        //ResetPower();
        ResetIO();
        dotTile.GetComponent<DotTileScript>().ObjectOnMe = null;
        //dotTile.GetComponent<DotTileScript>().PowerSourceObj = null;
        //dotTile.GetComponent<DotTileScript>().Power = 0;
        foreach (GameObject obj in dotTile.GetComponent<DotTileScript>().Connections)
        {
            obj.GetComponent<PowerLineScript>().Power = 0;
            obj.GetComponent<PowerLineScript>().highestPowerObj = null;
            obj.GetComponent<PowerLineScript>().PowerSourceObj = null;
        }
        transform.position = MoveToPos;
        SetDotTile();
    }
}
