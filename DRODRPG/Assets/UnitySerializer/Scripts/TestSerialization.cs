using UnityEngine;
using System.Collections;
using Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;


public class TestSerialization : MonoBehaviour
{
	
	void OnEnable()
	{
		LevelSerializer.Progress += HandleLevelSerializerProgress;
	}
	
	void OnDisable()
	{
		LevelSerializer.Progress -= HandleLevelSerializerProgress;
	}

	static void HandleLevelSerializerProgress (string section, float complete)
	{
		Debug.Log(string.Format("Progress on {0} = {1:0.00%}", section, complete));
	}
	
	
	void OnGUI()
	{

	}

	public IEnumerator Save ()
	{
		var t = DateTime.Now;
		LevelSerializer.SerializeLevelToFile("DROD RPG Savefile.txt");
		Radical.CommitLog();
		Debug.Log(string.Format("{0:0.000}", (DateTime.Now - t).TotalSeconds));
		yield return new WaitForSeconds(0);
	}

	public IEnumerator Load ()
	{
		var t = DateTime.Now;
		LevelSerializer.LoadSavedLevelFromFile("DROD RPG Savefile.txt");
		Radical.CommitLog();
		Debug.Log(string.Format("{0:0.000}", (DateTime.Now - t).TotalSeconds));
		yield return new WaitForSeconds(0);
	}

	// Update is called once per frame
	void Update()
	{

	}
}


