using UnityEngine;
using System.Collections;
using System.Reflection;
public class PowerOutput : EditableObject {
    public static int MaxPower = 10;
	[Editable(true)]
	public int powerOutput;
	public void Start(){
		foreach(Collider2D col in Physics2D.OverlapPointAll(transform.position)){
			if (col.tag == "DotTile"){
				col.gameObject.GetComponent<DotTileScript>().ObjectOnMe = gameObject;
				break;
			}
		}
	}
	public override void ValueChanged(object sender, object value)
	{
		print (sender.ToString());
		if (sender.ToString() == "System.Int32 powerOutput")
		{
			powerOutput = int.Parse(value.ToString());
		}
	}
}
