using UnityEngine;
using System.Collections;

public class RandomLights : MonoBehaviour
{
	public int number;
	public int height;
	public GameObject light;
	public bool makeLights;
	int gridSpacing = 4;
	GameObject go;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (makeLights)
		{
			makeLights = false;
			MakeLights ();
		}
	}

	void MakeLights ()
	{
		for (int i = 0; i < number; i ++)
		{
			int r = Mathf.RoundToInt(Random.Range(0, GameObject.FindGameObjectsWithTag("Finished").Length));
			go = (GameObject) GameObject.Instantiate(light, GameObject.FindGameObjectsWithTag("Finished")[r].transform.position + (Vector3.up * height), Quaternion.identity);
		}
	}
}
