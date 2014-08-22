using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Store this component when saving data
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class StoreComponent : Attribute
{
	
}

[AttributeUsage(AttributeTargets.Class)]
public class DontStoreAttribute : Attribute
{
	
}

[ExecuteInEditMode]
[DontStore]
[AddComponentMenu("Storage/Unique Identifier")]
public class UniqueIdentifier2 : MonoBehaviour
{
	[HideInInspector]
	public bool IsDeserializing;
	
	public string _id = string.Empty;

	public string Id {
		get {
			if(gameObject==null)
				return _id;
			if(!string.IsNullOrEmpty(_id))
				return _id;
			return _id = SaveGameManager.GetId(gameObject);
		}
		set {
			_id = value;
			SaveGameManager.Instance.SetId (gameObject, value);
		}
	}

	public static GameObject GetByName (string id)
	{
		var result = SaveGameManager.Instance.GetById (id);
		return result ?? GameObject.Find (id);
	}

	private static List<UniqueIdentifier2> allIdentifiers = new List<UniqueIdentifier2> ();
	
	public static List<UniqueIdentifier2> AllIdentifiers
	{
		get
		{
			allIdentifiers = allIdentifiers.Where(a=>a!=null).ToList();
			return allIdentifiers;
		}
		set
		{
			allIdentifiers = value;
		}
		
	}
	
	
	[HideInInspector]
	public string classId = Guid.NewGuid ().ToString ();
	
	public string ClassId {
		get {

			return classId;
		}
		set {
			if (string.IsNullOrEmpty (value)) {
				value = Guid.NewGuid ().ToString ();
			}
			classId = value;
		}
	}

	public void FullConfigure ()
	{
		ConfigureId ();
		foreach (var c in GetComponentsInChildren<UniqueIdentifier2>(true).Where(c=>c.gameObject.active == false)) {
			c.ConfigureId ();
		}
	}

	protected virtual void Awake ()
	{
		
		
		foreach(var c in GetComponents<UniqueIdentifier2>().Where(t=>t.GetType() == typeof(UniqueIdentifier2) && t != this))
			DestroyImmediate(c);
		
		SaveGameManager.Initialize (() =>
		{
			if(gameObject==null)
				return;
			FullConfigure ();
		});
	}

	void ConfigureId ()
	{
		_id = SaveGameManager.GetId (gameObject);
			AllIdentifiers.Add (this);
	}

	void OnDestroy ()
	{
		if(AllIdentifiers.Count > 0)
			AllIdentifiers.Remove (this);
	}

}


public static class SerializationHelper
{
	public static bool IsDeserializing(this GameObject go)
	{
		var ui = go.GetComponent<UniqueIdentifier2>();
		return ui != null ? ui.IsDeserializing : false;
	}
}





