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

	// Use this for initialization
	void Start ()
	{
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
	}
	
	IEnumerator MakeMap ()
	{
		gos = new GameObject[mapSizeX, mapSizeZ];
		gosBottom = new GameObject[mapSizeX, mapSizeZ];
		for (int x3 = 0; x3 < mapSizeX; x3 ++)
		{
			for (int z3 = 0; z3 < mapSizeZ; z3 ++)
			{
				gos[x3, z3] = (GameObject) GameObject.Instantiate(cube, new Vector3(x3 * gridSpacing - mapSizeX * 2 + 2 + transform.position.x, 0, z3 * gridSpacing - mapSizeX * 2 + 2 + transform.position.z), Quaternion.identity);
				gos[x3, z3].tag = "Other";
			}
		}
		for (int x2 = 0; x2 < mapSizeX; x2 ++)
		{
			for (int z2 = 0; z2 < mapSizeZ; z2 ++)
			{
				gosBottom[x2, z2] = (GameObject) GameObject.Instantiate(cube, new Vector3(x2 * gridSpacing - mapSizeX * 2 + 2 + transform.position.x, -4, z2 * gridSpacing - mapSizeX * 2 + 2 + transform.position.z), Quaternion.identity);
				gosBottom[x2, z2].renderer.material = null;
			}
		}
		int x = Mathf.RoundToInt(Random.Range(0, mapSizeX));
		int z = Mathf.RoundToInt(Random.Range(0, mapSizeZ));
		pIterationPos = new Vector3(x, 0, z);
		int i = 0;
		while (GameObject.FindGameObjectsWithTag("Other").Length > 0)
		{
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
						{
							SetSpace (x, z, pIterationPos);
						}
						else
						{
							Destroy(gos[x, z]);
						}
						break;
					}
					else if (gos[(int) pIterationPos.x, (int) pIterationPos.z].renderer.sharedMaterial == materials[j])
					{
						if (Random.Range(0, 101) < materialEndChance[j])
						{
							SetSpace (x, z, pIterationPos);
						}
						else
						{
							if (gos[x, z] != null)
								gos[x, z].renderer.material = materials[j];
							else
							{
								while (gos[x, z] == null || gos[x, z].tag == "Untagged")
								{
									x = Mathf.RoundToInt(Random.Range(0, mapSizeX));
									z = Mathf.RoundToInt(Random.Range(0, mapSizeZ));
								}
							}
						}
						break;
					}
				}
			}
			if (gos[x, z] != null)
			{
				gos[x, z].tag = "Untagged";
				Destroy(gosBottom[x, z]);
			}
			if (GameObject.FindGameObjectsWithTag("Other").Length == 0)
				StopCoroutine("MakeMap");
			x2 = x;
			z2 = z;
			keepRunning = false;
			StartCoroutine("PickNextSpace");
			while (!keepRunning)
			{
				yield return new WaitForSeconds(0f);
			}
			pIterationPos = new Vector3(x, 0, z);
			x = x2;
			z = z2;
			i ++;
			message = "" + i;
			yield return new WaitForSeconds(0f);
		}
		yield return new WaitForSeconds(0f);
	}
	
	void SetSpace (int x, int z, Vector3 pIterationPos)
	{
		float r2 = Random.Range(0, 3);
		if (Random.Range(0, 101) < destroyStartChance)
		{
			Destroy(gos[x, z]);
		}
		else
		{
			while (gos[x, z].renderer.sharedMaterial == null)
			{
				int r = Mathf.RoundToInt(Random.Range(0, materials.Length));
				if (Random.Range(0, 101) < materialStartChance[r])
				{
					gos[x, z].renderer.material = materials[r];
					return;
				}
			}
		}
	}
	
	IEnumerator PickNextSpace ()
	{
		bool shouldBreak = false;
		ArrayList gos2 = new ArrayList();
		for (int i = 0; i <= 8; i ++)
		{
			bool keepRunning2 = false;
			bool b = false;
			while (x2 >= mapSizeX || z2 >= mapSizeZ || x2 < 0 || z2 < 0 || new Vector3(x2, 0, z2) == pIterationPos || !keepRunning2)
			{
				x2 = (int) pIterationPos.x + Mathf.RoundToInt(Random.Range(-1, 2));
				z2 = (int) pIterationPos.z + Mathf.RoundToInt(Random.Range(-1, 2));
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
				//yield return new WaitForSeconds(.5f);
			}
			if (gos[x2, z2] != null)
			{
				if (gos[x2, z2].tag == "Untagged")
				{
					if (!gos2.Contains(gos[x2, z2]))
					{
						gos2.Add(gos[x2, z2]);
						if (gos2.Count == 8)
						{
							x2 = Mathf.RoundToInt(Random.Range(0, mapSizeX));
							z2 = Mathf.RoundToInt(Random.Range(0, mapSizeZ));
							message = "Finished iteration";
							//yield return new WaitForSeconds(.5f);
							createLoc = new Vector3(x2, 0, z2);
							shouldBreak = true;
						}
					}
				}
				else
				{
					createLoc = new Vector3(x2, 0, z2);
					shouldBreak = true;
				}
			}
			if (shouldBreak)
				break;
			message = "Finished iteration tier 2";
			//yield return new WaitForSeconds(.5f);
		}
		keepRunning = true;
		yield return new WaitForSeconds(.5f);
	}
}
 