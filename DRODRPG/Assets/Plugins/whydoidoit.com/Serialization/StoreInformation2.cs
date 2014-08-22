using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[DontStore]
[ExecuteInEditMode]
[AddComponentMenu("Storage/Store Information")]
public class StoreInformation2 : UniqueIdentifier2
{
	public bool StoreAllComponents = true;
	[HideInInspector]
	public List<string> Components = new List<string>();
	
	protected override void Awake()
	{
		base.Awake();
		foreach(var c in GetComponents<UniqueIdentifier2>().Where(t=>t.GetType() == typeof(UniqueIdentifier2) || 
			(t.GetType() == typeof(StoreInformation2) && t != this)))
			DestroyImmediate(c);
	}
	
}

