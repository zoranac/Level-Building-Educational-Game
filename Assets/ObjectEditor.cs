using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using System.IO;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public struct FieldStruct{
	public FieldStruct(FieldInfo field,string bname,string name){
		Field = field;
		BaseName = bname;
		Name = name;
	}
	public FieldInfo Field;
	public string BaseName;
	public string Name;
}
public class ObjectEditor : MonoBehaviour {
	public GameObject SelectedObject;
    public GameObject NameObj;
	public List<Transform> UI_transforms = new List<Transform>();
	public List<FieldStruct> editableFields = new List<FieldStruct>();
	public List<Component> components = new List<Component>();
	List<GameObject> UIObjects = new List<GameObject>();
	public GameObject slider;
	public GameObject toggle;
    public GameObject dropdown;
	// Use this for initialization
	string[] GetFieldName(string text)
	{
		char[] delimiterChars = {' '};
		string[] words = new string[10];
		words = text.Split(delimiterChars);
		
		words[0] = words[1];
		char[] newWord = new char[100];
		int w = 0;
		foreach(char character in words[0])
		{
			if (char.IsUpper(character) && w > 0)
			{
				newWord[w] = ' ';
				newWord[w+1] = character;
				w+=2;
			}
			else
			{
				if (w == 0)
					newWord[w] = char.ToUpper(character);
				else
					newWord[w] = character;
				w++;
			}
		}
		text = new string(newWord);
		string[] names = new string[2]{words[0],text};
		return names;
	}
	public void SetSelectedObject(GameObject obj){

		SelectedObject = obj;
        NameObj.GetComponent<Text>().text = SelectedObject.name;
		foreach(GameObject uiObj in UIObjects){
			Destroy(uiObj);
		}
		UIObjects.Clear();
		components.Clear();
		editableFields.Clear();

		Component[] scripts = SelectedObject.GetComponents(typeof(MonoBehaviour));
		const BindingFlags flags = /*BindingFlags.NonPublic | */BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

		int i = 0;
		while (i < scripts.Length)
		{
			FieldInfo[] fields =  scripts[i].GetType().GetFields(flags);
			foreach (FieldInfo fieldInfo in fields)
			{
				
				//Debug.Log("Obj: " + scripts[i].name + ", Field: " + fieldInfo.Name);
				foreach (System.Attribute a in fieldInfo.GetCustomAttributes(true))
				{
					if (a.ToString() == "Editable")
					{
						string text = fieldInfo.ToString();
						string[] names = GetFieldName(text);
						editableFields.Add(new FieldStruct(fieldInfo,names[0],names[1]));
						components.Add(scripts[i]);
                        
					}
				}
			}
     
			i++;
		}
		i = 0;
		foreach (FieldStruct f in editableFields)
		{
			FieldInfo info = f.Field;
			EditableObject tempObject = SelectedObject.GetComponent<EditableObject>();
			if (info.FieldType == typeof(bool))
			{
				GameObject Temp = (GameObject)Instantiate(toggle,UI_transforms[i].position,UI_transforms[i].rotation);
				Temp.transform.SetParent(gameObject.transform,true);
				Temp.transform.localScale = new Vector2(1f,1f);
				Temp.GetComponentInChildren<Text>().text = f.Name;
				object temp = info.GetValue(components[0]);
				Temp.GetComponent<Toggle>().isOn = (bool)temp;
				Temp.GetComponent<Toggle>().onValueChanged.AddListener(delegate {tempObject.ValueChanged(info,Temp.GetComponent<Toggle>().isOn);});	
				UIObjects.Add(Temp);
				i++;
				print("bool");
			}
			if (info.FieldType == typeof(int))
			{
				GameObject Temp = (GameObject)Instantiate(slider,UI_transforms[i].position,UI_transforms[i].rotation);
				Temp.transform.SetParent(gameObject.transform,true);
				Temp.transform.localScale = new Vector2(1f,1f);
				Temp.GetComponentInChildren<Text>().text = f.Name;
				Temp.GetComponent<Slider>().maxValue = PowerOutput.MaxPower;
				Temp.GetComponent<Slider>().minValue = 0;
				Temp.GetComponent<Slider>().wholeNumbers = true;
				object temp = info.GetValue(components[0]);
				Temp.GetComponent<Slider>().value = (int)temp;
				Temp.GetComponent<Slider>().onValueChanged.AddListener(delegate {tempObject.ValueChanged(info,Temp.GetComponent<Slider>().value);});
				UIObjects.Add(Temp);
				i++;
				print("int");
			}
			if (info.FieldType == typeof(float))
			{
				print("float");
			}
            if (info.FieldType == typeof(LogicGate.GateType))
            {
                GameObject Temp = (GameObject)Instantiate(dropdown, UI_transforms[i].position, UI_transforms[i].rotation);
                Temp.transform.SetParent(gameObject.transform, true);
                Temp.transform.localScale = new Vector2(1f, 1f);
                Temp.GetComponentInChildren<Text>().text = f.Name;
                Temp.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData("AND"));
                Temp.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData("XOR"));
                Temp.GetComponent<Dropdown>().value = 0;
                Temp.GetComponent<Dropdown>().onValueChanged.AddListener(delegate { tempObject.ValueChanged(info, Temp.GetComponent<Dropdown>().value); });
                UIObjects.Add(Temp);
                i++;
            }
		}
	}
	void Start () {
	


	}
	// Update is called once per frame
	void Update () {

	}
}
