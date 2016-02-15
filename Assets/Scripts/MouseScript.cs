using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MouseScript : MonoBehaviour {
	//float distance = 3;
	public float buffer;
	ControlScript control;
	GameObject CreateObj;
	public bool Skip = false;
    public GameObject Highlight;
    public GameObject PowerLine;
	public GameObject SelectGameObject;
    public GameObject MoveGameObject;
	GameObject objectEditor;
    bool PlacingPowerline = false;
    GameObject tempPowerLine;
	// Use this for initialization
//	public void ResetTempPowerLine(){
//		tempPowerLine = null;
//		PlacingPowerline = false;
//	}
	void Start () {
		control = GameObject.Find("Control").GetComponent<ControlScript>();
		objectEditor = GameObject.Find("ObjectEditor");
	}

	// Update is called once per frame
	void Update () {

        if (((Input.mousePosition.x < 0 +buffer || Input.mousePosition.x > Screen.width - buffer) 
		    || (Input.mousePosition.y < 0 +buffer || Input.mousePosition.y > Screen.height - buffer)) && !Skip){
			Skip = true;
		}
		//print (Input.mousePosition + " " + Screen.width);

		CreateObj = control.DrawObject;
		//IF MODE IS EDIT
        if (control.GetComponent<ControlScript>().CurrentMode == ControlScript.Mode.Connect)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (CreateObj == SelectGameObject)
                    SelectObject();
                else if (CreateObj == MoveGameObject)
                    MoveObject();
                else if (CreateObj == PowerLine)
                    DrawPowerLine();
                else if (CreateObj == null)
                    EraseConnection();
                else
                    CreateConnectionObject();
            }
            if (Highlight.activeSelf)
				Highlight.SetActive(false);
        }
		else if (control.GetComponent<ControlScript>().CurrentMode == ControlScript.Mode.Play)
		{
			if (Highlight.activeSelf)
				Highlight.SetActive(false);
		}
        else
        {
			if (!Highlight.activeSelf)
				Highlight.SetActive(true);
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (CreateObj == SelectGameObject)
                    SelectObject();
                else if (CreateObj == MoveGameObject)
                    MoveObject();
                else if (CreateObj == null)
                    Erase();
                else
                    Draw();
            }
        }



        if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
        {
            if (Camera.main.orthographicSize < 4)
                Camera.main.orthographicSize = Camera.main.orthographicSize + .25f;

        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
        {
            if (Camera.main.orthographicSize > .5f)
                Camera.main.orthographicSize = Camera.main.orthographicSize - .25f;
        }

    }
    void MoveObject()
    {
        if (Input.GetMouseButtonDown(0) && MoveGameObject.GetComponent<MoveObjectScript>().MoveObject == null)
        {
			foreach (Collider2D col in Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
			{
				if ((col.gameObject.layer == 8 && control.CurrentMode == ControlScript.Mode.Build) ||
				    (col.gameObject.layer == 10 && control.CurrentMode == ControlScript.Mode.Connect))
				{
					MoveGameObject.GetComponent<MoveObjectScript>().SetMoveObject(col.gameObject);
				}
			}
           
        }
        else if (Input.GetMouseButtonDown(0) && MoveGameObject.GetComponent<MoveObjectScript>().MoveObject != null)
		{
            GameObject obj = null;
			foreach (Collider2D col in Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
			{
                if (col.gameObject.layer == 8 && control.CurrentMode == ControlScript.Mode.Build)
                {
                    obj = null;
                    break;
                }
                else if (col.gameObject.layer == 10 && control.CurrentMode == ControlScript.Mode.Connect)
                {
                    obj = null;
                    break;
                }
				if (MoveGameObject.GetComponent<MoveObjectScript>().MoveObject.layer == 8 && control.CurrentMode == ControlScript.Mode.Build)
				{
                    if (col.tag == "Tile")
                        obj = col.gameObject;
						
				}
				else if(MoveGameObject.GetComponent<MoveObjectScript>().MoveObject.layer == 10 && control.CurrentMode == ControlScript.Mode.Connect)
				{
					if (col.tag == "DotTile")
                        obj = col.gameObject;
				}
			}
            if (obj != null)
            {
                MoveGameObject.GetComponent<MoveObjectScript>().PlaceMoveObject(new Vector3(obj.transform.position.x, obj.transform.position.y, MoveGameObject.GetComponent<MoveObjectScript>().MoveObject.transform.position.z));
            }
		}
    }
	void SelectObject(){
		if (Input.GetMouseButtonDown(0))
		{
			foreach (Collider2D col in Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
			{
				if ((col.gameObject.layer == 8 && control.CurrentMode == ControlScript.Mode.Build) ||
				    (col.gameObject.layer == 10 && control.CurrentMode == ControlScript.Mode.Connect))
					objectEditor.GetComponent<ObjectEditor>().SetSelectedObject(col.gameObject);
			}
		}
	}
    void DrawPowerLine()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //distance = 3 + Input.mousePosition.x / 10;

        //Move end of the Line to the mouse pos
        if (PlacingPowerline)
        {
			tempPowerLine.GetComponent<PowerLineScript>().UpdateLinePos(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y,0));
        }
		//while left mouse button is down 
        if (Input.GetMouseButton(0))
        {
			foreach (RaycastHit2D obj in Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), ray.direction))
			{
				if (obj.collider.gameObject.tag == "DotTile")
                {
					int count = 0;
					if (tempPowerLine != null)
					{
						if (tempPowerLine.GetComponent<PowerLineScript>().ConnectedDots.Count > 0)
							count = tempPowerLine.GetComponent<PowerLineScript>().ConnectedDots.Count;
					}
                    if (!PlacingPowerline)
                    {
						GameObject temp = (GameObject)Instantiate(PowerLine, new Vector3(obj.transform.position.x, obj.transform.position.y,0),  Quaternion.identity);
                        temp.name = PowerLine.name.Replace("(Clone)", "");
                        tempPowerLine = temp;
						tempPowerLine.GetComponent<PowerLineScript>().SetUp(obj.transform.position);
                        tempPowerLine.GetComponent<PowerLineScript>().ConnectedDots.Add(obj.collider.gameObject);
						obj.collider.GetComponent<DotTileScript>().Connections.Add(tempPowerLine);
                        PlacingPowerline = true;
                        break;
                    }
                    else
                    {    
                        //tempPowerLine.GetComponent<PowerLineScript>().SetLinePos(new Vector3 (obj.transform.position.x, .5f, obj.transform.position.z));
                        //tempPowerLine.GetComponent<PowerLineScript>().ConnectedDots[0] = obj.collider.gameObject;
                        //tempPowerLine = null;
                        //PlacingPowerline = false;
//						if(){
//
//						}
						//if one of the dots next to the starting dot
						if (obj.collider.gameObject == tempPowerLine.GetComponent<PowerLineScript>().ConnectedDots[count-1].GetComponent<DotTileScript>().TileAbove ||
						    obj.collider.gameObject == tempPowerLine.GetComponent<PowerLineScript>().ConnectedDots[count-1].GetComponent<DotTileScript>().TileBelow ||
						    obj.collider.gameObject == tempPowerLine.GetComponent<PowerLineScript>().ConnectedDots[count-1].GetComponent<DotTileScript>().TileLeft ||
						    obj.collider.gameObject == tempPowerLine.GetComponent<PowerLineScript>().ConnectedDots[count-1].GetComponent<DotTileScript>().TileRight)
						{
							tempPowerLine.GetComponent<PowerLineScript>().ConnectedDots.Add(obj.collider.gameObject);
							tempPowerLine.GetComponent<PowerLineScript>().SetLinePos(new Vector3 (obj.transform.position.x, obj.transform.position.y,0));
							//add connection for the other dotTile
							obj.collider.GetComponent<DotTileScript>().Connections.Add(tempPowerLine);
							//Enable Collision on PowerLine
							tempPowerLine.GetComponent<BoxCollider2D>().enabled = true;
							//start new connection at last connection's ending point
							GameObject temp = (GameObject)Instantiate(PowerLine, new Vector3(obj.transform.position.x, obj.transform.position.y,0),  Quaternion.identity);
                            temp.name = PowerLine.name.Replace("(Clone)", "");
							tempPowerLine = temp;
							tempPowerLine.GetComponent<PowerLineScript>().SetUp(obj.transform.position);
							tempPowerLine.GetComponent<PowerLineScript>().ConnectedDots.Add(obj.collider.gameObject);
							obj.collider.GetComponent<DotTileScript>().Connections.Add(tempPowerLine);
							break;

						}
                    }
                }   
            }
        }
		if (Input.GetMouseButtonUp(0))
		{
			if (tempPowerLine != null)
			{
				if (tempPowerLine.GetComponent<PowerLineScript>().ConnectedDots.Count > 0)
				{
					//if the last connection point is not dead on a Dot Tile, remove the last point in the line
					if (Camera.main.ScreenToWorldPoint(Input.mousePosition) != tempPowerLine.GetComponent<PowerLineScript>().ConnectedDots[tempPowerLine.GetComponent<PowerLineScript>().ConnectedDots.Count-1].transform.position)
					{
						tempPowerLine.GetComponent<PowerLineScript>().RemoveLinePos();
					}
					//if the connection is incomplete, remove the connection from the dot tile and destory the connection
					if (tempPowerLine.GetComponent<PowerLineScript>().ConnectedDots.Count == 1)
					{
						tempPowerLine.GetComponent<PowerLineScript>().ConnectedDots[0].GetComponent<DotTileScript>().Connections.RemoveAt(
							tempPowerLine.GetComponent<PowerLineScript>().ConnectedDots[0].GetComponent<DotTileScript>().Connections.Count-1);
						Destroy(tempPowerLine);
					}
				}
			}
			tempPowerLine = null;
			PlacingPowerline = false;

		}
    }
    void Draw(){
        if (Skip)
        {
            Skip = false;
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //distance = 3 + Input.mousePosition.x / 10;

            bool instantiate = false;
            Collider2D Obj = new Collider2D();

            foreach (RaycastHit2D obj in Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), ray.direction))
            {
               
                if (obj.collider.tag == "Tile")
                {
                    Vector3 pos = obj.transform.position;
                    Highlight.transform.position = new Vector3(pos.x, pos.y, pos.z) ;
                }
                if (Input.GetMouseButton(0))
                {
                    if (obj.collider.tag == "Tile")
                    {
                        instantiate = true;
                        Obj = obj.collider;

                        Ray tileRay = new Ray(obj.transform.position - Vector3.forward, Vector3.forward);

                        foreach (RaycastHit2D obj2 in Physics2D.RaycastAll(obj.transform.position - Vector3.forward, tileRay.direction))
                        {
                            if (obj2.collider.gameObject.layer == 8)
                            {
                                instantiate = false;
                                break;
                            }
                        }
                   
                        
                        
                    }
                }
              
            }
            if (instantiate)
            {
                GameObject tempObj = (GameObject)Instantiate(CreateObj, new Vector3(Obj.transform.position.x, Obj.transform.position.y, 0), Obj.transform.rotation) as GameObject;
                tempObj.name = tempObj.name.Replace("(Clone)", "");
            }
        }
	}
	void Erase(){
		if (Skip){
			Skip = false;
		}else{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//distance = 3 + Input.mousePosition.x/10;
			
			RaycastHit2D destroyObj = new RaycastHit2D();
            foreach (RaycastHit2D obj in Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), ray.direction))
            { 
                if (obj.collider.tag == "Tile")
                {
                    Vector3 pos = obj.transform.position;
                    Highlight.transform.position = new Vector3(pos.x, pos.y, pos.z);

                    if (Input.GetMouseButton(0))
                    {
                        Ray tileRay = new Ray(obj.transform.position- Vector3.forward, Vector3.forward);

                        foreach (RaycastHit2D obj2 in Physics2D.RaycastAll(obj.transform.position - Vector3.forward, tileRay.direction))
                        {
                            if (obj2.collider.gameObject.layer == 8)
                            {
                                destroyObj = obj2;
                            }
                        }
                    }
                }
			}
			if (destroyObj.collider != null)
				Destroy(destroyObj.collider.gameObject);
		}
	}
	void EraseConnection(){
		if (Skip){
			Skip = false;
		}else{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//distance = 3 + Input.mousePosition.x/10;
			
			RaycastHit2D destroyObj = new RaycastHit2D();
			foreach (RaycastHit2D obj in Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), ray.direction))
			{ 
				if (obj.collider.tag == "DotTile")
				{
					Vector3 pos = obj.transform.position;
					Highlight.transform.position = new Vector3(pos.x, pos.y, pos.z);
					
					if (Input.GetMouseButton(0))
					{
						Ray tileRay = new Ray(obj.transform.position- Vector3.forward, Vector3.forward);
						
						foreach (RaycastHit2D obj2 in Physics2D.RaycastAll(obj.transform.position - Vector3.forward, tileRay.direction))
						{
							if (obj2.collider.gameObject.layer == 10)
							{
								destroyObj = obj2;
							}
						}
					}
				}
				else if (obj.collider.tag == "PowerLine")
				{
					if (Input.GetMouseButton(0))
					{
						destroyObj = obj;
						destroyObj.collider.GetComponent<PowerLineScript>().DestroyMe();
					}
				}
			}
			if (destroyObj.collider != null)
				Destroy(destroyObj.collider.gameObject);
		}
	}
	void CreateConnectionObject(){
		if (Skip)
		{
			Skip = false;
		}
		else
		{
			
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//distance = 3 + Input.mousePosition.x / 10;
			
			bool instantiate = false;
			Collider2D Obj = new Collider2D();
			
			foreach (RaycastHit2D obj in Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), ray.direction))
			{
				if (obj.collider.tag == "DotTile")
				{
					//Vector3 pos = obj.transform.position;
					//Highlight.transform.position = new Vector3(pos.x, pos.y, pos.z) ;
				}
				if (Input.GetMouseButton(0))
				{
					if (obj.collider.tag == "DotTile")
					{
						instantiate = true;
						Obj = obj.collider;
						
						Ray tileRay = new Ray(obj.transform.position - Vector3.forward, Vector3.forward);
						
						foreach (RaycastHit2D obj2 in Physics2D.RaycastAll(obj.transform.position - Vector3.forward, tileRay.direction))
						{
							if (obj2.collider.gameObject.layer == 10)
							{
								instantiate = false;
								break;
							}
						}
						
						
						
					}
				}
				
			}
			if (instantiate)
			{
				GameObject tempObj = (GameObject)Instantiate(CreateObj, new Vector3(Obj.transform.position.x, Obj.transform.position.y, -1), Obj.transform.rotation) as GameObject;
                tempObj.name = tempObj.name.Replace("(Clone)", "");
            }
		}
	}
}
