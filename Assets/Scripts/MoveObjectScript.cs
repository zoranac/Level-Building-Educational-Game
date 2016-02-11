using UnityEngine;
using System.Collections;

public class MoveObjectScript : MonoBehaviour {
    public GameObject MoveObject;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void SetMoveObject(GameObject obj)
    {
        MoveObject = obj;
    }
    public void PlaceMoveObject(Vector3 MoveToPos)
    {
        if (MoveObject.GetComponent<PlaceableObject>() != null)
        {
            MoveObject.GetComponent<PlaceableObject>().Move(MoveToPos);
        }
    }
}
