using UnityEngine;
using System.Collections;

public class RandomMonsters : MonoBehaviour
{
	public int number;
	public GameObject[] monsters;
	public int[] monsterChances;
	public bool makeMonsters;
	int gridSpacing = 4;
	GameObject go;
	
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (makeMonsters)
		{
			makeMonsters = false;
			MakeMonsters ();
		}
	}
	
	void MakeMonsters ()
	{
		for (int i = 0; i < number; i ++)
		{
			int r = Mathf.RoundToInt(Random.Range(0, GameObject.FindGameObjectsWithTag("Finished").Length));
			while (true)
			{
				int r2 = Mathf.RoundToInt(Random.Range(0, monsters.Length));
				if (Random.Range(0, 101) < monsterChances[r2])
				{
					go = (GameObject) GameObject.Instantiate(monsters[r2], GameObject.FindGameObjectsWithTag("Finished")[r].transform.position + (Vector3.up * 4), Quaternion.Euler(90, Mathf.Round(Random.Range (1, 8)) * 45, 0));
					break;
				}
			}
		}
	}
}
