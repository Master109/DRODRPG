using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour
{
	public Transform target;
	public float smoothTime = 0.3f;
	Vector2 vel;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = new Vector3(Mathf.SmoothDamp(transform.position.x, target.position.x, ref vel.x, smoothTime), transform.position.y, Mathf.SmoothDamp(transform.position.z, target.position.z, ref vel.y, smoothTime));
	}
}
