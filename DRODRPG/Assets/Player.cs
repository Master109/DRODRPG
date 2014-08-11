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
	public float playerAttackTimer;
	public float playerAttackRate;
	public int score;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		moveTime += Time.deltaTime;
		playerAttackTimer += Time.deltaTime;
		if (moveTime >= moveRate)
		{
			if (Input.GetAxisRaw("X") != 0 && Physics.Raycast(new Ray(transform.position + Vector3.right * Input.GetAxisRaw("X") * moveDist, Vector3.down), moveDist, whatIsGround))
			{
				moveTime = 0;
				transform.Translate(Vector3.right * Input.GetAxisRaw("X") * moveDist, Space.World);
			}
			else if (Input.GetAxisRaw("Z") != 0 && Physics.Raycast(new Ray(transform.position + Vector3.forward * Input.GetAxisRaw("Z") * moveDist, Vector3.down), moveDist, whatIsGround))
			{
				moveTime = 0;
				transform.Translate(Vector3.forward * Input.GetAxisRaw("Z") * moveDist, Space.World);
			}
			else if (Input.GetAxisRaw("+X-Z") != 0 && Physics.Raycast(new Ray(transform.position + (Vector3.right * Input.GetAxisRaw("+X-Z") * moveDist) + (Vector3.forward * -Input.GetAxisRaw("+X-Z") * moveDist), Vector3.down), moveDist, whatIsGround))
			{
				moveTime = 0;
				transform.Translate((Vector3.right * Input.GetAxisRaw("+X-Z") * moveDist) + (Vector3.forward * -Input.GetAxisRaw("+X-Z") * moveDist), Space.World);
			}
			else if (Input.GetAxisRaw("+X+Z") != 0 && Physics.Raycast(new Ray(transform.position + (Vector3.right * Input.GetAxisRaw("+X+Z") * moveDist) + (Vector3.forward * Input.GetAxisRaw("+X+Z") * moveDist), Vector3.down), moveDist, whatIsGround))
			{
				moveTime = 0;
				transform.Translate((Vector3.right * Input.GetAxisRaw("+X+Z") * moveDist) + (Vector3.forward * Input.GetAxisRaw("+X+Z") * moveDist), Space.World);
			}
			else if (Input.GetAxisRaw("Turn") != 0)
			{
				moveTime = 0;
				transform.Rotate(0, 0, -Input.GetAxisRaw("Turn") * 45);
				Camera.main.transform.rotation = Quaternion.Euler(90, 0, 0);
			}
		}
	}

	void OnGUI ()
	{
		GUI.Label(new Rect(0, 0, Screen.width, 50), "Health: " + hp);
		GUI.Label (new Rect (0, 10, Screen.width, 50), "Best score: " + PlayerPrefs.GetInt("Score", 0));
		GUI.Label(new Rect(0, 20, Screen.width, 50), "Score: " + score);
	}
}
