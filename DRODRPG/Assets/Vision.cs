using UnityEngine;
using System.Collections;

public class Vision : MonoBehaviour
{
	Roach r;
	SkeletonArcher sA;

	// Use this for initialization
	void Start ()
	{
		if (transform.root.name.Contains("Roach"))
			r = transform.root.GetComponent<Roach>();
		else if (transform.root.name.Contains("SkeletonArcher"))
			sA = transform.root.GetComponent<SkeletonArcher>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.name == "Player")
		{
			if (r != null)
				r.enabled = true;
			else if (sA != null)
				sA.enabled = true;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.name == "Player")
		{
			if (r != null)
				r.enabled = false;
			else if (sA != null)
				sA.enabled = false;
		}
	}
}
