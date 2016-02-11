using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class ControlScript : MonoBehaviour {
    public enum Mode
    {
        Build,
        Connect,
        Play
    };
	public Vector3 PlayerStartPos;
	public GameObject DrawObject;
	public List<GameObject> PlaceableObjs = new List<GameObject>();
	public List<GameObject> ConnectionObjs = new List<GameObject>();
	public GameObject BuildGUI;
	public GameObject ConnectionGUI;
    public GameObject ObjectPlacementButton;
    public Mode CurrentMode = Mode.Build;
    public GameObject canvas;
	public float PlayModeZoom;
	Mode lastMode = Mode.Build;
	GameObject editModeButton;
	GameObject eraseButton;
	Vector3 posBeforePlay;
	float zoomBeforePlay;
	GameObject drawObjBeforePlay;
	// Use this for initialization
	void Start () {
		Screen.fullScreen = false; 
		PlayerStartPos = GameObject.Find("Player").transform.position;

		CreateGUIButtons();
		editModeButton = GameObject.Find("EditModeButton");
		eraseButton = GameObject.Find("EraseButton");
    }
	void CreateGUIButtons(){
        float y = Screen.height - (25 / (400 / (float)Screen.height));
        float x = Screen.width/10;
		//Build GUI
        foreach (GameObject obj in PlaceableObjs)
        {
            GameObject tempButton = Instantiate(ObjectPlacementButton);
            
			tempButton.GetComponent<RectTransform>().SetParent(BuildGUI.GetComponent<RectTransform>().transform);
            //tempButton.GetComponent<RectTransform>().position = new Vector3(x, y, 0);
            tempButton.transform.position = new Vector3(x, y, 0);
            tempButton.transform.localScale = new Vector3(1,1,1);
            tempButton.GetComponent<ObjectPlacementButtonScript>().PlaceObject = obj;
            y -= 25/ (400/(float)Screen.height);
        }
		//Connect GUI
		y = Screen.height - (25 / (400 / (float)Screen.height));
		x = Screen.width/10;
		foreach (GameObject obj in ConnectionObjs)
		{
			GameObject tempButton = Instantiate(ObjectPlacementButton);
			
			tempButton.GetComponent<RectTransform>().SetParent(ConnectionGUI.GetComponent<RectTransform>().transform);
			//tempButton.GetComponent<RectTransform>().position = new Vector3(x, y, 0);
			tempButton.transform.position = new Vector3(x, y, 0);
			tempButton.transform.localScale = new Vector3(1,1,1);
			tempButton.GetComponent<ObjectPlacementButtonScript>().PlaceObject = obj;
			y -= 25/ (400/(float)Screen.height);
		}
		ConnectionGUI.SetActive(false);
	}
    public void ChangeModeBetweenPlaceAndEdit()
    {
        if (CurrentMode == Mode.Connect)
        {
			ChangeConnectionObjectVisability();
			BuildGUI.SetActive(true);
			ConnectionGUI.SetActive(false);
			CurrentMode = Mode.Build;
			DrawObject = PlaceableObjs[0];
        }
        else if (CurrentMode == Mode.Build)
        {
			ChangeConnectionObjectVisability();
			BuildGUI.SetActive(false);
			ConnectionGUI.SetActive(true);
			CurrentMode = Mode.Connect;
			DrawObject = ConnectionObjs[0];
        }
        
    }
    public void SetToPlayMode()
    {
		if (CurrentMode != Mode.Play)
		{
			GameObject.Find("Player").transform.position = PlayerStartPos;
			if (CurrentMode == Mode.Connect)
			{
				ChangeConnectionObjectVisability();
			}
			drawObjBeforePlay = DrawObject;
			posBeforePlay = Camera.main.transform.position;
			zoomBeforePlay = Camera.main.orthographicSize;
			Camera.main.orthographicSize = PlayModeZoom;
			DrawObject = null;
			editModeButton.SetActive(false);
			eraseButton.SetActive(false);
			BuildGUI.SetActive(false);
			ConnectionGUI.SetActive(false);
			lastMode = CurrentMode;
        	CurrentMode = Mode.Play;
		}
		else{
			GameObject.Find("Player").transform.position = PlayerStartPos;
			eraseButton.SetActive(true);
			DrawObject = drawObjBeforePlay;
			CurrentMode = lastMode;
			Camera.main.transform.position = posBeforePlay;
			Camera.main.orthographicSize = zoomBeforePlay;
			editModeButton.SetActive(true);
			if (CurrentMode == Mode.Connect)
			{
				ChangeConnectionObjectVisability();
				ConnectionGUI.SetActive(true);
			}
			if (CurrentMode == Mode.Build)
			{
				BuildGUI.SetActive(true);
			}
		}
		
    }
	private void ChangeConnectionObjectVisability()
	{
		if (FindGameObjectsWithLayer(10) != null)
		{
			foreach (GameObject obj in FindGameObjectsWithLayer(10))
			{
				obj.GetComponent<SpriteRenderer>().enabled = !obj.GetComponent<SpriteRenderer>().enabled;
			}
		}
		if (GameObject.FindGameObjectsWithTag("PowerLine") != null){
			foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PowerLine"))
			{
				obj.GetComponent<LineRenderer>().enabled = !obj.GetComponent<LineRenderer>().enabled;
				if (obj.GetComponentInChildren<SpriteRenderer>() != null)
					obj.GetComponentInChildren<SpriteRenderer>().enabled = !obj.GetComponentInChildren<SpriteRenderer>().enabled;
			}
		}
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("DotTile"))
		{
			obj.GetComponent<SpriteRenderer>().enabled = !obj.GetComponent<SpriteRenderer>().enabled;
		}
	}
	public GameObject[] FindGameObjectsWithLayer (int layer){
		GameObject[] goArray = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
		List<GameObject> goList = new List<GameObject>();
		for (var i = 0; i < goArray.Length; i++) {
			if (goArray[i].layer == layer) {
				goList.Add(goArray[i]);
			}
		}
		if (goList.Count == 0) {
			return null;
		}
		return goList.ToArray();
	}
	// Update is called once per frame
	void Update () {

	}
}
