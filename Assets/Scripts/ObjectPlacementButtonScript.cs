using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ObjectPlacementButtonScript : MonoBehaviour {
public GameObject PlaceObject;
	public GameObject Control;
	public GameObject MyCamera;
	// Use this for initialization
	void Start () {
		Control = GameObject.Find("Control");
		MyCamera = GameObject.Find("Camera");
        if (PlaceObject != null)
            GetComponentInChildren<Text>().text = PlaceObject.name;
  
    }
	
	// Update is called once per frame
	void Update () {

	}
	public void ButtonPress(){
		Control.GetComponent<ControlScript>().DrawObject = PlaceObject;

	}
}
