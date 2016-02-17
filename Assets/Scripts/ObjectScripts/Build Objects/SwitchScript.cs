using UnityEngine;
using System.Collections;

public class SwitchScript : InteractableObject {
    [Editable(true)]
	public bool On;
    public bool Connected = false;
    public GameObject ConnectorSwitchPrefab;
    public GameObject DisconnectedObj;
	// Use this for initialization
	void Start () {
        bool create = true;
        foreach (Collider2D col in Physics2D.OverlapPointAll(transform.position))
        {
            if (col.gameObject.layer == 10)
            {
                create = false;
                break;
            }
        }
        if (create)
        {
            GameObject temp = (GameObject)Instantiate(ConnectorSwitchPrefab, new Vector3(transform.position.x, transform.position.y,-1.25f), Quaternion.identity);
            temp.transform.parent = gameObject.transform;
            temp.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            Connected = false;
            GameObject temp = (GameObject)Instantiate(ConnectorSwitchPrefab, new Vector3(transform.position.x, transform.position.y, -1.25f), Quaternion.identity);
            temp.transform.parent = gameObject.transform;
            temp.SetActive(false);
        }
    }
    public void TestIfConnected()
    {
        Connected = false;
        foreach(Collider2D col in Physics2D.OverlapPointAll(transform.position))
        {
            if (col.gameObject.layer == 10)
            {
                if (col.gameObject.GetComponent<ConnectorSwitch>() != null)
                {
                    Connected = true;
                }
            }
        }
    }
    void Update()
    {
        TestIfConnected();
        if (!Connected)
        {
            if (GameObject.Find("Control").GetComponent<ControlScript>().CurrentMode == ControlScript.Mode.Connect)
                DisconnectedObj.SetActive(!Connected);
            else
                DisconnectedObj.SetActive(false);
        }
        else
        {
            DisconnectedObj.SetActive(false);
        }
    }
    virtual public void Interact()
	{
		On = !On;
	}
    public override void ValueChanged(object sender, object value)
    {
        if (sender.ToString() == "System.Boolean On")
        {
            On = bool.Parse(value.ToString());
        }
    }
}
