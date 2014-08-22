using UnityEngine;
using System.Linq;

[DontStore]
[AddComponentMenu("Storage/Empty Object Identifier")]
[ExecuteInEditMode]
public class EmptyObjectIdentifier : StoreInformation2
{
	protected override void Awake ()
	{
		base.Awake ();
		if(!gameObject.GetComponent<StoreMaterials>()) 
			gameObject.AddComponent<StoreMaterials>();
	}
	
	public static void FlagAll(GameObject gameObject)
	{
		foreach(var c in gameObject.GetComponentsInChildren<Transform>().Where(c=>!c.GetComponent<UniqueIdentifier2>()))
				c.gameObject.AddComponent<EmptyObjectIdentifier>();
	}
	
}