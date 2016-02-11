using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PowerLineScript : MonoBehaviour {
    public enum FlowDirection
    {
        Up,
        Down,
        Left,
        Right,
        None
    };
    public FlowDirection CurrentFlowDirection = FlowDirection.None;
    LineRenderer lineRenderer;
	public List<GameObject> ConnectedDots = new List<GameObject>();
	public int Power = 0;
	int indexPos = 0;
	Vector3[] positions;
	GameObject Arrow;
    public GameObject highestPowerObj;
	public GameObject PowerSourceObj;
    GameObject control;
    bool ForcedPowerOff = false;
	public bool DontTestForPower = false;						//If it is not tested for power, only true if is an output of a temp power object
	bool IsInput = false;										//will be used for Sprite Management, shoing flow direction and I/O direction
	bool IsOutput = false;										//will be used for Sprite Management, shoing flow direction and I/O direction
	// Use this for initialization
    void Start () {
		GetComponent<BoxCollider2D>().enabled = false;
		Arrow = transform.FindChild("Arrow").gameObject;
        control = GameObject.Find("Control");
    }
	public void SetPower(int power,GameObject supplier,GameObject source){
        Power = power;
		PowerSourceObj = source;
        ForcedPowerOff = false;
        if (Power > 0)
            highestPowerObj = supplier;
        else
            ForcedPowerOff = true;
    }
	void TestIfInInputsList(GameObject obj,bool Add)
	{
		bool inList = false;
		foreach (GameObject o in obj.GetComponent<TempPowerOutput>().Inputs)
		{
			if (o == gameObject)
			{
				inList = true;
				break;
			}
		}
		if (!inList && Add)
		{
			obj.GetComponent<TempPowerOutput>().Inputs.Add(gameObject);
		}
		else if (inList && !Add)
		{
			obj.GetComponent<TempPowerOutput>().Inputs.Remove(gameObject);
		}
	}
	void TestIfInOutputsList(GameObject obj,bool Add)
	{
		bool inList = false;
		foreach (GameObject o in obj.GetComponent<TempPowerOutput>().Outputs)
		{
			if (o == gameObject)
			{
				inList = true;
				break;
			}
		}
		if (!inList && Add)
		{
			obj.GetComponent<TempPowerOutput>().Outputs.Add(gameObject);
		}
		else if (inList && !Add)
		{
			obj.GetComponent<TempPowerOutput>().Outputs.Remove(gameObject);
		}
	}
	void TestPower()
	{
		IsInput = false;
		IsOutput = false;
		DontTestForPower = false;
        bool PowerOutput = false;
        bool OtherConnectorObject = false;
        GameObject otherConnectorObject1 = null;
        GameObject otherConnectorObject2 = null; 
		bool ignoreDot2 = false; //if dot in the ConnectedDots[1] is a temp power obj, ignore it.

		//Test to see if you need to run test (If it is not an output of a temp power obj)
		//Test collision of Dot 0
        foreach (Collider2D col in Physics2D.OverlapPointAll(ConnectedDots[0].transform.position))
        {
            if (col.gameObject.layer == 10)
			{
                if (col.gameObject.GetComponent<PowerOutput>() != null)
                {
                    PowerOutput = true;
                    break;
                }
                else
                {
                    OtherConnectorObject = true;
                    otherConnectorObject1 = col.gameObject;
                    break;
                }
            }
        }
		//Test collision of Dot 1
        foreach (Collider2D col in Physics2D.OverlapPointAll(ConnectedDots[1].transform.position))
        {
            
            if (col.gameObject.layer == 10)
            {
                if (col.gameObject.GetComponent<PowerOutput>() != null)
                {
                    PowerOutput = true;
                    break;
                }
                else
                {
                    OtherConnectorObject = true;
                    otherConnectorObject2 = col.gameObject;
                    break;
                }
            }
        }
        if (PowerOutput)
        {
            DontTestForPower = false;
        }
        else if (OtherConnectorObject)
        {
           	if (otherConnectorObject1 != null && otherConnectorObject1.GetComponent<TempPowerOutput>() != null)
			{
				IsOutput = true;
				TestIfInOutputsList(otherConnectorObject1,true);
				TestIfInInputsList(otherConnectorObject1,false);
				DontTestForPower = true;
			}
			if (otherConnectorObject2 != null && otherConnectorObject2.GetComponent<TempPowerOutput>() != null)
			{
				IsInput = true;
				TestIfInInputsList(otherConnectorObject2,true);
				TestIfInOutputsList(otherConnectorObject2,false);
				DontTestForPower = false;
				ignoreDot2 = true;
			}
        }

        //foreach (GameObject obj in ConnectedDots)
        //{
        //    bool hasConnectorObject = false;
        //    //DontTestForPower = false;
        //    foreach (Collider2D col in Physics2D.OverlapPointAll(obj.transform.position))
        //    {
        //        //if there is a connection object on the dot
        //        if (col.gameObject.layer == 10)
        //        {
        //            hasConnectorObject = true;
        //            //if it has a poweroutput script test for power
        //            if (col.gameObject.GetComponent<PowerOutput>() != null)
        //            {
        //                DontTestForPower = false;
        //                break;
        //            }
        //            //if it doesnt dont test for power
        //            else
        //            {
        //                if (i == 0)
        //                {
        //                    DontTestForPower = true;
        //                }
        //                break;
        //            }
        //        }
        //    }
        //    //if dont test for power
        //    if (!hasConnectorObject && DontTestForPower)
        //    {
        //        //foreach (GameObject con in obj.GetComponent<DotTileScript>().Connections)
        //        //{

        //        //}
        //        //if (!obj.GetComponent<DotTileScript>().Powered)
        //            DontTestForPower = false;
        //    }
        //    if (DontTestForPower == false && hasConnectorObject)
        //        break;

        //    i++;
        //}
        if (DontTestForPower)
        {
            return;
        }

        int highestPower = 0;
		bool fromPowerSource = false;
		
//		int highestPowerDir = 0;
		int highestDotPower = 0;
		highestPowerObj = null;
		foreach(GameObject obj in ConnectedDots)
		{
			if (obj.GetComponent<DotTileScript>().Power>highestDotPower) 
				highestDotPower = obj.GetComponent<DotTileScript>().Power;
			foreach(Collider2D col in Physics2D.OverlapPointAll(obj.transform.position))
			{
				if (col.gameObject.layer == 10)
				{
					if (col.gameObject.GetComponent<PowerOutput>() != null)
					{
						if (col.gameObject.GetComponent<PowerOutput>().powerOutput > highestPower)
						{
							highestPower = col.gameObject.GetComponent<PowerOutput>().powerOutput;
							fromPowerSource = true;
							highestPowerObj = col.gameObject;
							PowerSourceObj = col.gameObject;
                           // DontTestForPower = false;
						}
					}
					else
					{
						break;
					}
				}
			}
			foreach (GameObject connection in obj.GetComponent<DotTileScript>().Connections)
			{
				if (connection != gameObject && connection.GetComponent<PowerLineScript>().ConnectedDots.Count >= 2)
				{
					if (connection.GetComponent<PowerLineScript>().Power > highestPower)
					{
						highestPower = connection.GetComponent<PowerLineScript>().Power;
						fromPowerSource = false;
						highestPowerObj = obj;
						PowerSourceObj = connection.GetComponent<PowerLineScript>().PowerSourceObj;
					}
				}
			}
			if (ignoreDot2)
			{
				break;
			}
		}
        if (!DontTestForPower)
        {
//			if (PowerSourceObj == null)
//			{
//				Power = 0;
//			}
//			else 
				if (highestDotPower > 0)
			{
                if (fromPowerSource)
					Power = highestDotPower;
				else
					Power = highestDotPower - 1;
            }
            else
            {
                Power = 0;
            }
        }
		
	}
	// Update is called once per frame
    void TestPowerFlow()
    {
		if (highestPowerObj == null)
		{
			Power = 0;
		}
        if (Power > 0 && ConnectedDots.Count >= 2)
        {
            Arrow.SetActive(true);

            if (highestPowerObj.transform.position.y > Arrow.transform.position.y)
            {
                Arrow.transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z - 90));
                CurrentFlowDirection = FlowDirection.Down;
            }
            if (highestPowerObj.transform.position.y < Arrow.transform.position.y)
            {
                Arrow.transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + 90));
                CurrentFlowDirection = FlowDirection.Up;
            }
            if (highestPowerObj.transform.position.x < Arrow.transform.position.x)
            {
                Arrow.transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, 0));
                CurrentFlowDirection = FlowDirection.Right;
            }
            if (highestPowerObj.transform.position.x > Arrow.transform.position.x)
            {
                Arrow.transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, -180));
                CurrentFlowDirection = FlowDirection.Left;
            }
        }
        else
        {
            Arrow.SetActive(false);
            CurrentFlowDirection = FlowDirection.None;
        }
        if (lineRenderer != null)
		{
            lineRenderer.SetColors(new Color((float)Power / (float)PowerOutput.MaxPower, (float)Power / (float)PowerOutput.MaxPower, 0),
                                   new Color((float)Power / (float)PowerOutput.MaxPower, (float)Power / (float)PowerOutput.MaxPower, 0));
		}
		if (Arrow.GetComponent<SpriteRenderer>() != null){
			Arrow.GetComponent<SpriteRenderer>().color = new Color((float)Power / (float)PowerOutput.MaxPower, (float)Power / (float)PowerOutput.MaxPower, 0);
		}
    }
	void Update () {
        if (ConnectedDots.Count >= 2)
		    TestPower();
        TestPowerFlow();
        if (control.GetComponent<ControlScript>().CurrentMode != ControlScript.Mode.Connect)
        {
            Arrow.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            Arrow.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
	public void SetUp(Vector3 pos){
		indexPos = 0;
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.SetWidth(.1f,.1f);
		lineRenderer.SetVertexCount(2);
		lineRenderer.SetPosition(indexPos, pos);
		positions = new Vector3[3];
		positions[indexPos] = pos;
		indexPos++;
	}
	public void UpdateLinePos(Vector3 pos){
		if (ConnectedDots.Count > 1)
			lineRenderer.SetPosition(indexPos-1, pos);
		else
			lineRenderer.SetPosition(indexPos, pos);
	}
	public void SetPosAndRot(){
		if (positions[0].x < positions[1].x){
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y,-90);
			transform.position = new Vector3(transform.position.x+.25f,Mathf.Floor(transform.position.y * 2) / 2,transform.position.z);
		}
		if (positions[0].x > positions[1].x){
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y,90);
			transform.position = new Vector3(transform.position.x-.25f,Mathf.Floor(transform.position.y * 2) / 2,transform.position.z);
		}
		if (positions[0].y > positions[1].y){
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y,180);
			transform.position = new Vector3(transform.position.x,(Mathf.Floor((transform.position.y) * 2) / 2)-.25f,transform.position.z);
		}
		if (positions[0].y < positions[1].y){
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y,0);
			transform.position = new Vector3(transform.position.x,(Mathf.Floor((transform.position.y) * 2) / 2)+.25f,transform.position.z);
		}
	}
    public void SetLinePos(Vector3 pos)
    {
		lineRenderer.SetVertexCount(indexPos+1);
		lineRenderer.SetPosition(indexPos, pos);
		positions[indexPos] = pos;
		if (indexPos == 1){
			SetPosAndRot();
		}
		indexPos++;
    }
	public void RemoveLinePos(){
		lineRenderer.SetVertexCount(indexPos-1);
		indexPos--;
	}
	public void DestroyMe(){
		foreach(GameObject obj in ConnectedDots)
		{
			obj.GetComponent<DotTileScript>().PowerSourceObj = null;
			if (obj.GetComponent<DotTileScript>().ObjectOnMe != null && 
			    obj.GetComponent<DotTileScript>().ObjectOnMe.GetComponent<TempPowerOutput>() != null)
			{
				obj.GetComponent<DotTileScript>().ObjectOnMe.GetComponent<TempPowerOutput>().ResetPower();
				obj.GetComponent<DotTileScript>().ObjectOnMe.GetComponent<TempPowerOutput>().Inputs.Remove(gameObject);
				obj.GetComponent<DotTileScript>().ObjectOnMe.GetComponent<TempPowerOutput>().Outputs.Remove(gameObject);
			}
			obj.GetComponent<DotTileScript>().Connections.Remove(gameObject);
		}
		Destroy(gameObject);
	}
	public void OnTriggerEnter2D(Collider2D col){
		if (col.tag == gameObject.tag && col.gameObject != gameObject){
			DestroyMe();
		}
	}
}
