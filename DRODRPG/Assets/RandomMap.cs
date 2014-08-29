using UnityEngine;
using System.Collections;

public class RandomMap : MonoBehaviour
{
	public Material[] materials;
	public int[] materialStartChance;
	public int[] materialEndChance;
	public int destroyStartChance;
	public int destroyEndChance;
	public GameObject cube;
	public int mapSizeX;
	public int mapSizeZ;
	int gridSpacing = 4;
	GameObject[,] gos;
	GameObject[,] gosBottom;
	string message = "";
	Vector3 createLoc;
	Vector3 pIterationPos;
	int x2;
	int z2;
	bool keepRunning;
	public bool makeMap;
	int i2;
	public bool runStart = true;
	public bool expand;
	GameObject go;
	bool expanded;
	public GameObject beacon;
	public bool makeBeacons;
	bool runOnce = true;
	public Material mountains;
	public bool makeMountains;

	// Use this for initialization
	void Start ()
	{
		if (!runStart)
			return;
		gos = new GameObject[mapSizeX, mapSizeZ];
		gosBottom = new GameObject[mapSizeX, mapSizeZ];
		for (int x3 = 0; x3 < mapSizeX; x3 ++)
		{
			for (int z3 = 0; z3 < mapSizeZ; z3 ++)
			{
				gos[x3, z3] = (GameObject) GameObject.Instantiate(cube, new Vector3(x3 * gridSpacing - mapSizeX * 2 + 2, 0, z3 * gridSpacing - mapSizeZ * 2 + 2), Quaternion.identity);
				gos[x3, z3].tag = "Making";
			}
		}
		StartCoroutine ("MakeMap");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (message != "")
		{
			Debug.Log(message);
			message = "";
		}
		if (makeMap)
		{
			makeMap = false;
			StopAllCoroutines();
			StartCoroutine ("MakeMap");
		}
		if (makeBeacons)
		{
			makeBeacons = false;
			MakeBeacons ();
		}
		if (makeMountains)
		{
			makeMountains = false;
			StartCoroutine ("MakeMountains");
		}
	}

	IEnumerator MakeMountains ()
	{
		for (int x3 = 0; x3 < mapSizeX; x3 ++)
		{
			for (int z3 = 0; z3 < mapSizeZ; z3 ++)
			{
				if (!Physics.CheckSphere(new Vector3(x3 * gridSpacing - mapSizeX * 2 + 2, 0, z3 * gridSpacing - mapSizeZ * 2 + 2), 1, 8))
				{
					GameObject g = (GameObject) GameObject.Instantiate(cube, new Vector3(x3 * gridSpacing - mapSizeX * 2 + 2, -4, z3 * gridSpacing - mapSizeZ * 2 + 2), Quaternion.identity);
					g.renderer.sharedMaterial = mountains;
				}
				yield return new WaitForSeconds(0f);
			}
		}
		yield return new WaitForSeconds(0f);
	}

	IEnumerator MakeMap ()
	{
		int r = Mathf.RoundToInt(Random.Range(0, GameObject.FindGameObjectsWithTag("Making").Length));
		int x = (int) GameObject.FindGameObjectsWithTag("Making")[r].transform.position.x;
		int z = (int) GameObject.FindGameObjectsWithTag("Making")[r].transform.position.z;
		Vector3 offset = new Vector3(x / gridSpacing + mapSizeX / 2, 0, z / gridSpacing + mapSizeZ / 2);
		x = (int) (offset.x + .0f);
		z = (int) (offset.z + .0f);
		pIterationPos = new Vector3(x, 0, z);
		int i = 0;
		while (GameObject.FindGameObjectsWithTag("Making").Length > 0)
		{
			while (gos[x, z] == null || gos[x, z].renderer.sharedMaterial != null)
			{
				r = Mathf.RoundToInt(Random.Range(0, GameObject.FindGameObjectsWithTag("Making").Length));
				x = (int) GameObject.FindGameObjectsWithTag("Making")[r].transform.position.x;
				z = (int) GameObject.FindGameObjectsWithTag("Making")[r].transform.position.z;
				offset = new Vector3(x / gridSpacing + mapSizeX / 2, 0, z / gridSpacing + mapSizeZ / 2);
				x = (int) (offset.x + .0f);
				z = (int) (offset.z + .0f);
				yield return new WaitForSeconds(.0f);
			}
			if (i == 0)
			{
				SetSpace (x, z, pIterationPos);
			}
			else
			{
				for (int j = 0; j < materials.Length; j ++)
				{
					if (gos[(int) pIterationPos.x, (int) pIterationPos.z] == null)
					{
						if (Random.Range(0, 101) < destroyEndChance)
							SetSpace (x, z, pIterationPos);
						else
						{
							message = "Destroyed space: " + i2;
							Destroy(gos[x, z]);
						}
						break;
					}
					else if (gos[(int) pIterationPos.x, (int) pIterationPos.z].renderer.sharedMaterial == materials[j])
					{
						if (Random.Range(0, 101) < materialEndChance[j])
							SetSpace (x, z, pIterationPos);
						else
						{
							message = "Painted space: " + i2;
							gos[x, z].renderer.sharedMaterial = materials[j];
						}
						break;
					}
				}
			}
			gos[x, z].tag = "Finished";
			x2 = x;
			z2 = z;
			keepRunning = false;
			StartCoroutine("PickNextSpace");
			while (!keepRunning)
			{
				message = "Waiting";
				yield return new WaitForSeconds(.0f);
			}

			pIterationPos = new Vector3(x, 0, z);
			x = x2;
			z = z2;
			i ++;
			i2 ++;
			yield return new WaitForSeconds(0f);
		}
		for (int x3 = 0; x3 < mapSizeX; x3 ++)
		{
			for (int z3 = 0; z3 < mapSizeZ; z3 ++)
			{
				if (gos[x3, z3] == null)
				{
					gosBottom[x3, z3] = (GameObject) GameObject.Instantiate(cube, new Vector3(x3 * gridSpacing - mapSizeX * 2 + 2, -4, z3 * gridSpacing - mapSizeZ * 2 + 2), Quaternion.identity);
					gosBottom[x3, z3].renderer.sharedMaterial = mountains;
				}
				yield return new WaitForSeconds(0f);
			}
		}
		//MakeBeacons ();
	}

	void MakeBeacons ()
	{
		for (int x3 = (0) * gridSpacing - mapSizeX * 2 - 2; x3 < (mapSizeX) * gridSpacing - mapSizeX * 2 - 2; x3 += 100)
			for (int z3 = (0) * gridSpacing - mapSizeZ * 2 - 2; z3 < (mapSizeZ) * gridSpacing - mapSizeZ * 2 - 2; z3 += 100)
				go = (GameObject) GameObject.Instantiate(beacon, new Vector3(x3, beacon.transform.position.y, z3), Quaternion.identity);
	}
	
	void ExpandMap ()
	{
		mapSizeX += 2;
		mapSizeZ += 2;
		GameObject[,] gosCopy = gos;
		gos = new GameObject[mapSizeX, mapSizeZ];
		for (int x3 = 0; x3 < mapSizeX; x3 ++)
			for (int z3 = 0; z3 < mapSizeZ; z3 += mapSizeZ - 1)
			{
				go = (GameObject) GameObject.Instantiate(cube, new Vector3(x3 * gridSpacing - mapSizeX * 2 + 2, 0, z3 * gridSpacing - mapSizeZ * 2 + 2), Quaternion.identity);
				go.tag = "Making";
			}
		for (int x3 = 0; x3 < mapSizeX; x3 += mapSizeX - 1)
			for (int z3 = 1; z3 < mapSizeZ - 1; z3 ++)
			{
				go = (GameObject) GameObject.Instantiate(cube, new Vector3(x3 * gridSpacing - mapSizeX * 2 + 2, 0, z3 * gridSpacing - mapSizeZ * 2 + 2), Quaternion.identity);
				go.tag = "Making";
			}
		for (int x3 = 0; x3 < mapSizeX - 2; x3 ++)
			for (int z3 = 0; z3 < mapSizeZ - 2; z3 ++)
				gos[x3 + 1, z3 + 1] = gosCopy[x3, z3];
		expanded = true;
		runOnce = false;
		//yield return new WaitForSeconds(.0f);
	}
	
	void SetSpace (int x, int z, Vector3 pIterationPos)
	{
		float r2 = Random.Range(0, 3);
		if (Random.Range(0, 101) < destroyStartChance)
		{
			
		}
		else
		{
			while (true)
			{
				int r = Mathf.RoundToInt(Random.Range(0, materials.Length));
				if (Random.Range(0, 101) < materialStartChance[r])
				{
					message = "Painted space: " + i2;
					gos[x, z].renderer.sharedMaterial = materials[r];
					return;
				}
			}
		}
		message = "Destroyed space: " + i2;
		Destroy(gos[x, z]);
	}
	
	IEnumerator PickNextSpace ()
	{
		bool shouldBreak = false;
		ArrayList gos2 = new ArrayList();
		bool keepRunning2 = false;
		bool b = false;
		for (int i = 0; i <= 8; i ++)
		{
			while (x2 >= mapSizeX || z2 >= mapSizeZ || x2 < 0 || z2 < 0 || new Vector3(x2, 0, z2) == pIterationPos || !keepRunning2)
			{
				x2 = (int) pIterationPos.x + Mathf.RoundToInt(Random.Range(-1, 2));
				z2 = (int) pIterationPos.z + Mathf.RoundToInt(Random.Range(-1, 2));
				if (expand && runOnce && (x2 >= mapSizeX - 0 || z2 >= mapSizeZ - 0 || x2 < 0 || z2 < 0))
				{
					//StartCoroutine("ExpandMap");
					ExpandMap ();
					createLoc = new Vector3(x2, 0, z2);
					//break;
				}
				foreach (GameObject g in gos2)
				{
					if (g.transform.position == new Vector3(x2, 0, z2))
					{
						b = true;
						break;
					}
				}
				if (!b)
					keepRunning2 = true;
			}
			if (expanded)
			{
				expanded = false;
				if (x2 < 0)
					x2 = 0;
				if (z2 < 0)
					z2 = 0;
			}
			if (gos[x2, z2] != null && gos[x2, z2].tag == "Finished")
			{
				if (!gos2.Contains(gos[x2, z2]))
				{
					gos2.Add(gos[x2, z2]);
					if (gos2.Count == 8)
					{
						int r = Mathf.RoundToInt(Random.Range(0, GameObject.FindGameObjectsWithTag("Making").Length));
						x2 = (int) GameObject.FindGameObjectsWithTag("Making")[r].transform.position.x;
						z2 = (int) GameObject.FindGameObjectsWithTag("Making")[r].transform.position.z;
						Vector3 offset = new Vector3(x2 / gridSpacing + mapSizeX / 2, 0, z2 / gridSpacing + mapSizeZ / 2);
						x2 = (int) (offset.x + .0f);
						z2 = (int) (offset.z + .0f);
						createLoc = new Vector3(x2, 0, z2);
						shouldBreak = true;
						break;
					}
				}
			}
			else
				createLoc = new Vector3(x2, 0, z2);
			if (shouldBreak)
				break;
		}
		keepRunning = true;
		yield return new WaitForSeconds(.0f);
	}
}
