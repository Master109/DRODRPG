using UnityEngine;
using System.Collections;

public class RandomMap : MonoBehaviour
{
	public Material[] materials;
	public float[] materialStartChance;
	public float[] materialEndChance;
	public float destroyStartChance;
	public float destroyEndChance;
	public GameObject cube;
	public int mapSizeX;
	public int mapSizeZ;
	int gridSpacing = 4;
	GameObject[,] gos;
	string message;

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
		for (int x3 = 0; x3 < mapSizeX; x3 ++)
		{
			for (int z3 = 0; z3 < mapSizeZ; z3 ++)
			{
				gos[x3, z3] = (GameObject) GameObject.Instantiate(cube, new Vector3(x3 * gridSpacing - mapSizeX * 2 + 2, 0, z3 * gridSpacing - mapSizeX * 2 + 2), Quaternion.identity);
				gos[x3, z3].tag = "Other";
			}
		}
		for (int x2 = 0; x2 < mapSizeX; x2 ++)
		{
			for (int z2 = 0; z2 < mapSizeZ; z2 ++)
			{
				GameObject.Instantiate(cube, new Vector3(x2 * gridSpacing - mapSizeX * 2 + 2, -4, z2 * gridSpacing - mapSizeX * 2 + 2), Quaternion.identity);
			}
		}
		int x = Mathf.RoundToInt(Random.Range(0, mapSizeX));
		int z = Mathf.RoundToInt(Random.Range(0, mapSizeZ));
		Vector3 pIterationPos = new Vector3(x, 0, z);
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
							//SetSpace (x, z, pIterationPos);
						}
						else
						{
							Destroy(gos[x, z]);
						}
						break;
					}
					else if (gos[(int) pIterationPos.x, (int) pIterationPos.z].renderer.material == materials[j])
					{
						if (Random.Range(0, 101) < materialEndChance[j])
						{
							//SetSpace (x, z, pIterationPos);
						}
						else
						{
							gos[x, z].renderer.material = materials[j];
						}
						break;
					}
				}
			}
			gos[x, z].tag = "Untagged";
			if (GameObject.FindGameObjectsWithTag("Other").Length == 0)
				yield return new WaitForSeconds(1);
			Vector3 createPos = PickNextSpace (x, z, pIterationPos);
			pIterationPos = new Vector3(x, 0, z);
			x = (int) createPos.x;
			z = (int) createPos.z;
			i ++;
		}
		message = "Finished";
		yield return new WaitForSeconds(1);
	}

	void SetSpace (int x, int z, Vector3 pIterationPos)
	{
		if (Random.Range(0, 101) < destroyStartChance)
		{
			Destroy(gos[x, z]);
		}
		else
		{
			ArrayList usedMaterials = new ArrayList();
			int r = Mathf.RoundToInt(Random.Range(0, materials.Length));
			for (int j = 0; j < materials.Length; j ++)
			{
				while (usedMaterials.Contains(r))
					r = Mathf.RoundToInt(Random.Range(0, materials.Length));
				usedMaterials.Add(r);
				if (Random.Range(0, 101) < materialStartChance[j])
				{
					gos[x, z].renderer.material = materials[r];
					return;
				}
			}
			Destroy(gos[x, z]);
		}
	}

	Vector3 PickNextSpace (int x, int z, Vector3 pIterationPos)
	{
		ArrayList gos2 = new ArrayList();
		while (true)
		{
			while (x >= mapSizeX || z >= mapSizeZ || x < 0 || z < 0 || new Vector3(x, 0, z) == pIterationPos)
			{
				x = (int) pIterationPos.x + Mathf.RoundToInt(Random.Range(-1, 2));
				z = (int) pIterationPos.z + Mathf.RoundToInt(Random.Range(-1, 2));
			}
			if (gos[x, z].tag == "Other" && !gos2.Contains(gos[x, z]))
			{
				gos2.Add(gos[x, z]);
				if (gos2.Count == 8)
					return new Vector3(x, 0, z);
			}
			else
			{
				while (gos[x, z].tag == "Other")
				{
					x = Mathf.RoundToInt(Random.Range(0, mapSizeX));
					z = Mathf.RoundToInt(Random.Range(0, mapSizeZ));
				}
				return new Vector3(x, 0, z);
			}
		}
	}
}
