using UnityEngine;
using System.Collections;
using System.Linq;

[DontStore]
[AddComponentMenu("Storage/Prefab Identifier")]
[ExecuteInEditMode]
public class PrefabIdentifier : StoreInformation2
{
	bool inScenePrefab;
	
	public bool IsInScene()
	{
		return inScenePrefab;
	}
	
	protected override void Awake ()
	{
		inScenePrefab = true;
		base.Awake();
		foreach (var c in GetComponents<UniqueIdentifier2>().Where(t=>t.GetType() == typeof(UniqueIdentifier2) || 
			(t.GetType() == typeof(PrefabIdentifier) && t != this) ||
			t.GetType() == typeof(StoreInformation2)
			))
			DestroyImmediate (c);
	}	
}