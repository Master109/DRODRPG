using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	public Vector3 vel;
	float moveTimer;
	public float moveRate;
	public int damage;
	public int range;
	public GameObject shooter;
	int movesMade;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		moveTimer += Time.deltaTime;
		if (moveTimer > moveRate)
		{
			moveTimer = 0;
			movesMade ++;
			if (movesMade > range)
				Destroy(gameObject);
			else
				transform.position += vel;
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.name == "Player")
		{
			other.GetComponent<Player>().hp -= damage;
			if (other.GetComponent<Player>().hp <= 0)
				Application.LoadLevel(0);
			Destroy(gameObject);
		}
	}
}
