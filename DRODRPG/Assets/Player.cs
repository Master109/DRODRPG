using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public float moveRate;
	float moveTime;
	RaycastHit hit;
	int moveDist = 4;
	public LayerMask whatIsGround;
	public int hp;
	public float attackTimer;
	public float attackRate;
	public int score;
	public float moveDelayTime;
	float moveDelayTimer;
	int xAxis;
	int zAxis;
	int turnAxis;
	public int gold;
	public bool survival;

	// Use this for initialization
	void Start ()
	{
		moveRate -= moveDelayTime;
		moveDelayTimer = -1;
	}
	
	// Update is called once per frame
	void Update ()
	{
		moveTime += Time.deltaTime;
		attackTimer += Time.deltaTime;
		if (moveTime >= moveRate)
		{
			if (Input.GetAxisRaw("Turn") != 0)
			{
				xAxis = 0;
				zAxis = 0;
				turnAxis = (int) Input.GetAxisRaw("Turn");
				moveDelayTimer = 0;
				moveTime = 0;
			}
			else
			{
				turnAxis = 0;
				xAxis = (int) Input.GetAxisRaw("X");
				zAxis = (int) Input.GetAxisRaw("Z");
				moveDelayTimer = 0;
				moveTime = 0;
			}
		}
		if (moveDelayTimer >= 0)
		{
			moveDelayTimer += Time.deltaTime;
			if (moveDelayTimer > moveDelayTime)
			{
				if (turnAxis == 0)
				{
					if (xAxis != 0)
						zAxis = (int) Input.GetAxisRaw("Z");
					else
						xAxis = (int) Input.GetAxisRaw("X");
					if ((xAxis != 0 && zAxis == 0) && Physics.Raycast(new Ray(transform.position + Vector3.right * Input.GetAxisRaw("X") * moveDist, Vector3.down), moveDist, whatIsGround))
					{
						moveTime = 0;
						transform.Translate(Vector3.right * Input.GetAxisRaw("X") * moveDist, Space.World);
					}
					else if ((xAxis == 0 && zAxis != 0) && Physics.Raycast(new Ray(transform.position + Vector3.forward * Input.GetAxisRaw("Z") * moveDist, Vector3.down), moveDist, whatIsGround))
					{
						moveTime = 0;
						transform.Translate(Vector3.forward * Input.GetAxisRaw("Z") * moveDist, Space.World);
					}
					else if ((xAxis != 0 && zAxis != 0) && Physics.Raycast(new Ray(transform.position + (Vector3.right * Input.GetAxisRaw("X") * moveDist) + (Vector3.forward * Input.GetAxisRaw("Z") * moveDist), Vector3.down), moveDist, whatIsGround))
					{
						moveTime = 0;
						transform.Translate((Vector3.right * Input.GetAxisRaw("X") * moveDist) + (Vector3.forward * Input.GetAxisRaw("Z") * moveDist), Space.World);
					}
				}
				else
				{
					transform.Rotate(0, 0, -turnAxis * 45);
					Camera.main.transform.rotation = Quaternion.Euler(90, 0, 0);
				}
				moveDelayTimer = -1;
			}
		}
	}

	void OnGUI ()
	{
		GUI.Label(new Rect(0, 0, Screen.width, 50), "Health: " + hp);
		if (survival)
		{
			GUI.Label (new Rect (0, 10, Screen.width, 50), "Best score: " + PlayerPrefs.GetInt("Score", 0));
			GUI.Label(new Rect(0, 20, Screen.width, 50), "Score: " + score);
		}
	}
}
