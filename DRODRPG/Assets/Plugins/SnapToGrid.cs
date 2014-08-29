using UnityEngine;
using System.Collections;

public class SnapToGrid : MonoBehaviour
{
	int gridSpacing = 4;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = new Vector3(Mathf.Round(transform.position.x / gridSpacing) * gridSpacing, Mathf.Round(transform.position.y / gridSpacing) * gridSpacing, Mathf.Round(transform.position.z / gridSpacing) * gridSpacing);
	}
}
