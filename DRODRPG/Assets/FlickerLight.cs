using UnityEngine;
using System.Collections;

public class FlickerLight : MonoBehaviour
{
	float stage;
	public float stageChangeRate;
	public float waveMultiplier;
	public float waveAdditionFactor;
	float stage2;
	public float stage2ChangeRate;
	public float wave2Multiplier;
	public float wave2AdditionFactor;

	// Use this for initialization
	void Start ()
	{
		stage = Random.Range(0, 2);
		stage2 = Random.Range(0, 2);
	}
	
	// Update is called once per frame
	void Update ()
	{
		stage2 += stage2ChangeRate;
		stageChangeRate = Mathf.Sin(stage2) * wave2Multiplier + wave2AdditionFactor;
		stage += stageChangeRate;
		light.intensity = Mathf.Sin(stage) * waveMultiplier + waveAdditionFactor;
	}
}
