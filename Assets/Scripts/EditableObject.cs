using UnityEngine;
using System.Collections;
using System.Reflection;

public abstract class EditableObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public abstract void ValueChanged(object field, object value);
}
