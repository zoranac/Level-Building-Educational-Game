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
			BuildGUI.SetActive(true);
			ConnectionGUI.SetActive(false);
			CurrentMode = Mode.Build;
			DrawObject = PlaceableObjs[0];
        }
        else if (CurrentMode == Mode.Build)
        {
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
				ConnectionGUI.SetActive(true);
			}
			if (CurrentMode == Mode.Build)
			{
				BuildGUI.SetActive(true);
			}
		}
		
    }
	private void HideConnectionObjects()
	{

	}
	// Update is called once per frame
	void Update () {

	}
}
