using UnityEngine;
using System.Collections;

public class Survival : MonoBehaviour
{
	public GameObject[] enemies;
	float[] createTimes;
	public float[] createRates;
	public float createRatesMultiplier;
	GameObject go;
	int gridSpacing = 4;

	// Use this for initialization
	void Start ()
	{
		//PlayerPrefs.DeleteAll();
		createTimes = new float[enemies.Length];
		createTimes[0] = createRates[0];
	}
	
	// Update is called once per frame
	void Update ()
	{
		for (int i = 0; i < enemies.Length; i ++)
		{
			createTimes[i] += Time.deltaTime;
			if (createTimes[i]  >= createRates[i])
			{
				createTimes[i] = 0;
				createRates[i] *= createRatesMultiplier;
				int r = Mathf.RoundToInt(Random.Range(0, GameObject.FindGameObjectsWithTag("Ground").Length));
				go = (GameObject) GameObject.Instantiate(enemies[i]);
				go.transform.position = GameObject.FindGameObjectsWithTag("Ground")[r].transform.position + (Vector3.up * gridSpacing);
				if (go.name.Contains("Roach"))
					go.GetComponent<Roach>().awakeRadius = 100;
				else if (go.name.Contains("SkeletonArcher"))
					go.GetComponent<SkeletonArcher>().awakeRadius = 100;
			}
		}
	}
}
