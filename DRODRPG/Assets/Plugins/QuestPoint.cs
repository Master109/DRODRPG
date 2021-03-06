﻿using UnityEngine;
using System.Collections;

public class QuestPoint : MonoBehaviour
{
	public string questEvent;
	public bool destroy = true;
	public bool visible = true;
	public string dialogGoName = "";

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!visible)
			renderer.enabled = false;
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.name == "Player")
		{
			Parley.GetInstance().TriggerQuestEvent(questEvent);
			if (dialogGoName != "")
				GameObject.Find (dialogGoName).GetComponent<Dialog>().TriggerDialog();
			renderer.enabled = false;
			if (destroy)
				Destroy(gameObject);
		}
	}
}
