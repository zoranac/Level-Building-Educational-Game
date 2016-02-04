using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PowerLineScript : MonoBehaviour {
    LineRenderer lineRenderer;
	public List<GameObject> ConnectedDots = new List<GameObject>();
	public int MaxPower;
	public int Power = 0;
	int indexPos = 0;
	Vector3[] positions;
	GameObject Arrow;
    // Use this for initialization
    void Start () {
		GetComponent<BoxCollider2D>().enabled = false;
		Arrow = transform.FindChild("Arrow").gameObject;
    }
	public void SetPower(int power){
		Power = power;
	}
	void TestPower()
	{
		int highestPower = 0;
		bool fromPowerSource = false;
		bool DontTestForPower = false;
		int highestPowerDir = 0;
		GameObject highestPowerObj = null;
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
		if (highestPower > 0)
		{
			Arrow.SetActive(true);
			if (fromPowerSource)
				Power = highestPower;
			else
				Power = highestPower-1;


			if (highestPowerObj.transform.position.y > Arrow.transform.position.y)
			{
				Arrow.transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x,transform.rotation.y,transform.rotation.z-90));
			}
			if (highestPowerObj.transform.position.y < Arrow.transform.position.y)
			{
				Arrow.transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x,transform.rotation.y,transform.rotation.z+90));
			}
			if (highestPowerObj.transform.position.x < Arrow.transform.position.x)
			{
				Arrow.transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x,transform.rotation.y,0));
			}
			if (highestPowerObj.transform.position.x > Arrow.transform.position.x)
			{
				Arrow.transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x,transform.rotation.y,-180));
			}
		}
		else
		{
			Power = 0;
			Arrow.SetActive(false);
		}
		if (lineRenderer != null)
			lineRenderer.SetColors(new Color((float)Power/(float)MaxPower,(float)Power/(float)MaxPower,0),
		                           new Color((float)Power/(float)MaxPower,(float)Power/(float)MaxPower,0));
	}
	// Update is called once per frame
	void Update () {
		TestPower();


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
