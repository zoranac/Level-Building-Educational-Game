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
    GameObject highestPowerObj;
    GameObject control;
    // Use this for initialization
    void Start () {
		GetComponent<BoxCollider2D>().enabled = false;
		Arrow = transform.FindChild("Arrow").gameObject;
        control = GameObject.Find("Control");
    }
	public void SetPower(int power,GameObject supplier){
        Power = power;
        highestPowerObj = supplier;
    }
	void TestPower()
	{
        bool DontTestForPower = false;
        foreach (GameObject obj in ConnectedDots)
        {
            foreach (Collider2D col in Physics2D.OverlapPointAll(obj.transform.position))
            {
                if (col.gameObject.layer == 10)
                {
                    if (col.gameObject.GetComponent<PowerOutput>() != null)
                    {
                        DontTestForPower = false;
                        break;
                    }
                    else
                    {
                        DontTestForPower = true;
                        break;
                    }
                }
            }
            if (DontTestForPower == false)
                break;
        }
        if (DontTestForPower)
        {
            return;
        }

        int highestPower = 0;
		bool fromPowerSource = false;
		
		int highestPowerDir = 0;
		highestPowerObj = null;
		foreach(GameObject obj in ConnectedDots)
		{
			foreach(Collider2D col in Physics2D.OverlapPointAll(obj.transform.position))
			{
				if (col.gameObject.layer == 10)
				{
					if (col.gameObject.GetComponent<PowerOutput>() != null)
					{
						if (col.gameObject.GetComponent<PowerOutput>().Power > highestPower)
						{
							highestPower = col.gameObject.GetComponent<PowerOutput>().Power;
							fromPowerSource = true;
							highestPowerObj = obj;
						}
					}
					else
					{
						DontTestForPower = true;
						break;
					}
				}
			}
			if (DontTestForPower)
			{
				DontTestForPower = false;
				continue;
			}
			foreach (GameObject connection in obj.GetComponent<DotTileScript>().Connections)
			{
				if (connection != gameObject)
				{
					if (connection.GetComponent<PowerLineScript>().Power > highestPower)
					{
						highestPower = connection.GetComponent<PowerLineScript>().Power;
						fromPowerSource = false;
						highestPowerObj = obj;
					}
				}
			}
		}
        if (!DontTestForPower)
        {
            if (highestPower > 0)
            {
                if (fromPowerSource)
                    Power = highestPower;
                else
                    Power = highestPower - 1;
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
        if (Power > 0)
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
            lineRenderer.SetColors(new Color((float)Power / (float)PowerOutput.MaxPower, (float)Power / (float)PowerOutput.MaxPower, 0),
                                   new Color((float)Power / (float)PowerOutput.MaxPower, (float)Power / (float)PowerOutput.MaxPower, 0));
    }
	void Update () {
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
	public void OnTriggerEnter2D(Collider2D col){
		if (col.tag == gameObject.tag && col.gameObject != gameObject){
			foreach(GameObject obj in ConnectedDots)
			{
				obj.GetComponent<DotTileScript>().Connections.Remove(gameObject);
			}
			Destroy(gameObject);
		}
	}
}
