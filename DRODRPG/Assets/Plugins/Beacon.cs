using UnityEngine;
using System.Collections;

public class Beacon : MonoBehaviour
{
	ArrayList enemies = new ArrayList();
	static bool triggeredBeaconBeore = false;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Enemy")
			enemies.Add(other);
		else if (other.name == "Player")
		{
			if (!triggeredBeaconBeore)
			{
				Parley.GetInstance().TriggerQuestEvent("TriggeredBeacon");
				triggeredBeaconBeore = true;
			}
			foreach (Collider c in enemies)
			{
				if (c != null)
				{
					if (c.name.Contains("Roach"))
					{
						c.GetComponent<Roach>().enabled = true;
						c.GetComponent<Roach>().awake = true;
					}
					else if (c.name.Contains("SkeletonArcher"))
					{
						c.GetComponent<SkeletonArcher>().enabled = true;
						c.GetComponent<SkeletonArcher>().awake = true;
					}
				}
				//c.transform.Find ("Vision").GetComponent<Vision>().enabled = false;
			}
			renderer.material.color = new Color(1f, 1f, 1f, 0f);
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.name == "Player")
		{
			foreach (Collider c in enemies)
			{
				if (c != null)
				{
					if (c.name.Contains("Roach"))
						c.GetComponent<Roach>().enabled = false;
					else if (c.name.Contains("SkeletonArcher"))
						c.GetComponent<SkeletonArcher>().enabled = false;
				}
			}
			renderer.material.color += new Color(1f, 1f, 1f, .5f);
		}
	}
}
