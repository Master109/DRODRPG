using UnityEngine;
using System.Collections;

public class Roach : MonoBehaviour
{
	public float hp = 1;
	int moveDist = 4;
	float moveTimer;
	public float moveRate;
	float attackTimer;
	public float attackRate;
	public bool awake;
	public int awakeRadius;
	RaycastHit hit;
	Player player;
	Vector3 playerSwordPos;
	public int damage;
	public LayerMask whatIsGround;
	public LayerMask whatIsNonWalkable;
	Vector3 pLoc;
	public int gold;

	void Start ()
	{
		player = GameObject.Find("Player").GetComponent<Player>();
		pLoc = transform.position;
	}
	
	void Update ()
	{
		if (GameObject.Find("Scripts").GetComponent<Global>().timeScale2 == 0)
			return;
		playerSwordPos = GameObject.Find("PlayerSword").transform.position;
		if (!awake)
		{
			if (CheckForPlayer (awakeRadius))
			{
				awake = true;
				//transform.Find ("Vision").GetComponent<Vision>().enabled = false;
			}
			return;
		}
		attackTimer += Time.deltaTime * GameObject.Find("Scripts").GetComponent<Global>().timeScale2;
		moveTimer += Time.deltaTime * GameObject.Find("Scripts").GetComponent<Global>().timeScale2;
		if (moveTimer > moveRate)
		{
			Vector3 moveToPos = transform.position;
			if (transform.position.x - player.transform.position.x > 0)
				moveToPos -= Vector3.right * moveDist;
			else if (transform.position.x - player.transform.position.x < 0)
				moveToPos += Vector3.right * moveDist;
			if (transform.position.z - player.transform.position.z > 0)
				moveToPos -= Vector3.forward * moveDist;
			else if (transform.position.z - player.transform.position.z < 0)
				moveToPos += Vector3.forward * moveDist;

			Vector3 moveToPos2 = transform.position;
			if (transform.position.z - player.transform.position.z > 0)
				moveToPos2 -= Vector3.forward * moveDist;
			else if (transform.position.z - player.transform.position.z < 0)
				moveToPos2 += Vector3.forward * moveDist;
			if (transform.position.x - player.transform.position.x > 0)
				moveToPos2 -= Vector3.right * moveDist;
			else if (transform.position.x - player.transform.position.x < 0)
				moveToPos2 += Vector3.right * moveDist;

			if (transform.position == moveToPos && transform.position == moveToPos2)
				return;

			pLoc = transform.position;

			Vector3 idealMoveToPos;
			if (Vector3.Distance(moveToPos, player.transform.position) < Vector3.Distance(moveToPos2, player.transform.position))
				idealMoveToPos = moveToPos;
			else
				idealMoveToPos = moveToPos2;

			if (Physics.Raycast(new Vector3(idealMoveToPos.x, moveDist * 2, idealMoveToPos.z), Vector3.down, moveDist * 2, whatIsGround) && !Physics.Raycast(new Vector3(idealMoveToPos.x, moveDist * 2, idealMoveToPos.z), Vector3.down, moveDist * 2, whatIsNonWalkable))
			{
				transform.position = idealMoveToPos;
				Vector3 pToCurrentPos = transform.position - pLoc;
				transform.rotation = Quaternion.Euler(90, Mathf.Atan2(pToCurrentPos.z, -pToCurrentPos.x) * Mathf.Rad2Deg, 0);
				moveTimer = 0;
			}
		}
	}
	
	bool CheckForPlayer (int range)
	{
		Vector3 vector = transform.position;
		for (var i = 0; i < range; i ++)
		{
			if (vector.x - player.transform.position.x > 0)
				vector -= Vector3.right * moveDist;
			else if (vector.x - player.transform.position.x < 0)
				vector += Vector3.right * moveDist;
			if (vector.z - player.transform.position.z > 0)
				vector -= Vector3.forward * moveDist;
			else if (vector.z - player.transform.position.z < 0)
				vector += Vector3.forward * moveDist;
			if (vector == player.transform.position)
				return true;
		}
		return false;
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Bullet")
		{
			hp -= other.GetComponent<Bullet>().damage;
			if (hp <= 0)
			{
				player.score += 1;
				if (player.score > PlayerPrefs.GetInt("Score", 0))
				{
					if (player.survival)
						PlayerPrefs.SetInt("Score", player.score);
				}
				player.gold += gold;
				Destroy(gameObject);
			}
			Destroy(other.gameObject);
		}
	}

	void OnTriggerStay (Collider other)
	{
		if (other.gameObject.name == "Player" && attackTimer > attackRate)
		{
			attackTimer = 0;
			player.hp --;
			if (player.hp <= 0)
			{
				if (Application.loadedLevelName == "Survival")
					Application.LoadLevel(Application.loadedLevel);
				if (PlayerPrefs.GetInt("Saved", 0) == 1)
					player.Load ();
				else
					Application.LoadLevel(0);
			}
		}
		else if (other.name == "PlayerSword" && player.attackTimer > player.attackRate && other.transform.position.normalized * Mathf.Round(other.transform.position.magnitude) == transform.position.normalized * Mathf.Round(transform.position.magnitude))
		{
			player.attackTimer = 0;
			hp -= damage;
			if (hp <= 0)
			{
				player.score += 1;
				if (player.score > PlayerPrefs.GetInt("Score", 0))
				{
					if (player.survival)
						PlayerPrefs.SetInt("Score", player.score);
				}
				player.kills ++;
				if (player.kills == 1)
				{
					Parley.GetInstance().TriggerQuestEvent("EnemyKilled");
				}
				player.gold += gold;
				Destroy(gameObject);
			}
		}
	}
}
