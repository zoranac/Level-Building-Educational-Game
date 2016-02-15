using UnityEngine;
using System.Collections;

public class SwitchScript : InteractableObject {
    [Editable(true)]
	public bool On;
    public bool Connected = true;
    public GameObject ConnectorSwitchPrefab;
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
            GameObject temp = (GameObject)Instantiate(ConnectorSwitchPrefab, transform.position, Quaternion.identity);
            temp.transform.parent = gameObject.transform;
            temp.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            Connected = false;
            GameObject temp = (GameObject)Instantiate(ConnectorSwitchPrefab, transform.position, Quaternion.identity);
            temp.transform.parent = gameObject.transform;
            temp.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    override public void Interact()
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
