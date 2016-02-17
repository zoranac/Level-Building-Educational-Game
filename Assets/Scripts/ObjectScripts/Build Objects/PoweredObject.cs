using UnityEngine;
using System.Collections;

public class PoweredObject : EditableObject {
    [Editable(true)]
    public bool StartPowered = false;
    public bool Powered = false;
	// Update is called once per frame
	void FixedUpdate () {
		TestIfPowered();
	}
	void TestIfPowered(){
		foreach (Collider2D col in Physics2D.OverlapPointAll(transform.position)){
			if (col.tag == "DotTile"){
                if (StartPowered)
				    Powered = !col.GetComponent<DotTileScript>().Powered;
                else
                    Powered = col.GetComponent<DotTileScript>().Powered;
			}
		}
	}
    public override void ValueChanged(object sender, object value)
    {
        if (sender.ToString() == "System.Boolean StartPowered")
        {
            StartPowered = bool.Parse(value.ToString());
        }
    }
}
