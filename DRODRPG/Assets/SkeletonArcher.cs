using UnityEngine;
using System.Collections;

public class SkeletonArcher : MonoBehaviour
{
	public float hp = 1;
	int moveDist = 4;
	float moveTimer;
	public float moveRate;
	float attackTimer;
	public float attackRate;
	public GameObject bullet;
	bool awake;
	public int awakeRadius;
	RaycastHit hit;
	Player player;
	Vector3 playerSwordPos;
	//public int damage;
	public LayerMask whatIsGround;
	public LayerMask whatIsNonWalkable;
	GameObject go;

	void Start ()
	{
		player = GameObject.Find("Player").GetComponent<Player>();
	}
	
	void Update ()
	{
		playerSwordPos = GameObject.Find("PlayerSword").transform.position;
		if (!awake)
		{
			if (CheckForPlayer (awakeRadius))
				awake = true;
			return;
		}
		attackTimer += Time.deltaTime;
		Vector3 toPlayer = player.transform.position - transform.position;
		bool inLineWithPlayer = (Mathf.Abs(toPlayer.normalized.x) == 0 || Mathf.Abs(toPlayer.normalized.x) == 1) && (Mathf.Abs(toPlayer.normalized.z) == 0 || Mathf.Abs(toPlayer.normalized.z) == 1);
		bool canFire = inLineWithPlayer && CheckForPlayer (bullet.GetComponent<Bullet>().range);
		if (attackTimer > attackRate && canFire)
		{
			attackTimer = 0;
			go = (GameObject) GameObject.Instantiate(bullet, transform.position, bullet.transform.rotation);
			go.GetComponent<Bullet>().shooter = gameObject;
			go.GetComponent<Bullet>().vel = toPlayer.normalized * moveDist;
			//go.GetComponent<Bullet>().damage = damage;
		}

		moveTimer += Time.deltaTime;
		if (moveTimer > moveRate && !canFire)
		{
			Vector3 moveToPos = transform.position;
			if (transform.position.x - player.transform.position.x > 0 && !Physics.Raycast(new Vector3(moveToPos.x - moveDist, moveDist * 2, moveToPos.z), Vector3.down, moveDist * 2, whatIsNonWalkable) && Physics.Raycast(new Vector3(moveToPos.x - moveDist, moveDist * 2, moveToPos.z), Vector3.down, moveDist * 2, whatIsGround))
			{
				moveToPos -= Vector3.right * moveDist;
				if ((Mathf.Abs(toPlayer.normalized.x) == 0 || Mathf.Abs(toPlayer.normalized.x) == 1) && (Mathf.Abs(toPlayer.normalized.z) == 0 || Mathf.Abs(toPlayer.normalized.z) == 1))
				{
					transform.position = moveToPos;
					moveTimer = 0;
					return;
				}
			}
			else if (transform.position.x - player.transform.position.x < 0 && !Physics.Raycast(new Vector3(moveToPos.x + moveDist, moveDist * 2, moveToPos.z), Vector3.down, moveDist * 2, whatIsNonWalkable) && Physics.Raycast(new Vector3(moveToPos.x + moveDist, moveDist * 2, moveToPos.z), Vector3.down, moveDist * 2, whatIsGround))
			{
				moveToPos += Vector3.right * moveDist;
				if ((Mathf.Abs(toPlayer.normalized.x) == 0 || Mathf.Abs(toPlayer.normalized.x) == 1) && (Mathf.Abs(toPlayer.normalized.z) == 0 || Mathf.Abs(toPlayer.normalized.z) == 1))
				{
					transform.position = moveToPos;
					moveTimer = 0;
					return;
				}
			}
			if (transform.position.z - player.transform.position.z > 0 && !Physics.Raycast(new Vector3(moveToPos.x, moveDist * 2, moveToPos.z - moveDist), Vector3.down, moveDist * 2, whatIsNonWalkable) && Physics.Raycast(new Vector3(moveToPos.x, moveDist * 2, moveToPos.z - moveDist), Vector3.down, moveDist * 2, whatIsGround))
			{
				moveToPos -= Vector3.forward * moveDist;
				if ((Mathf.Abs(toPlayer.normalized.x) == 0 || Mathf.Abs(toPlayer.normalized.x) == 1) && (Mathf.Abs(toPlayer.normalized.z) == 0 || Mathf.Abs(toPlayer.normalized.z) == 1))
				{
					transform.position = moveToPos;
					moveTimer = 0;
					return;
				}
			}
			else if (transform.position.z - player.transform.position.z < 0 && !Physics.Raycast(new Vector3(moveToPos.x, moveDist * 2, moveToPos.z + moveDist), Vector3.down, moveDist * 2, whatIsNonWalkable) && Physics.Raycast(new Vector3(moveToPos.x, moveDist * 2, moveToPos.x + moveDist), Vector3.down, moveDist * 2, whatIsGround))
			{
				moveToPos += Vector3.forward * moveDist;
				if ((Mathf.Abs(toPlayer.normalized.x) == 0 || Mathf.Abs(toPlayer.normalized.x) == 1) && (Mathf.Abs(toPlayer.normalized.z) == 0 || Mathf.Abs(toPlayer.normalized.z) == 1))
				{
					transform.position = moveToPos;
					moveTimer = 0;
					return;
				}
			}

			Vector3 moveToPos2 = transform.position;
			if (transform.position.z - player.transform.position.z > 0 && !Physics.Raycast(new Vector3(moveToPos2.x, moveDist * 2, moveToPos2.z - moveDist), Vector3.down, moveDist * 2, whatIsNonWalkable) && Physics.Raycast(new Vector3(moveToPos2.x, moveDist * 2, moveToPos2.z - moveDist), Vector3.down, moveDist * 2, whatIsGround))
			{
				moveToPos2 -= Vector3.forward * moveDist;
				if ((Mathf.Abs(toPlayer.normalized.x) == 0 || Mathf.Abs(toPlayer.normalized.x) == 1) && (Mathf.Abs(toPlayer.normalized.z) == 0 || Mathf.Abs(toPlayer.normalized.z) == 1))
				{
					transform.position = moveToPos2;
					moveTimer = 0;
					return;
				}
			}
			else if (transform.position.z - player.transform.position.z < 0 && !Physics.Raycast(new Vector3(moveToPos2.x, moveDist * 2, moveToPos2.z + moveDist), Vector3.down, moveDist * 2, whatIsNonWalkable) && Physics.Raycast(new Vector3(moveToPos2.x, moveDist * 2, moveToPos2.x + moveDist), Vector3.down, moveDist * 2, whatIsGround))
			{
				moveToPos2 += Vector3.forward * moveDist;
				if ((Mathf.Abs(toPlayer.normalized.x) == 0 || Mathf.Abs(toPlayer.normalized.x) == 1) && (Mathf.Abs(toPlayer.normalized.z) == 0 || Mathf.Abs(toPlayer.normalized.z) == 1))
				{
					transform.position = moveToPos2;
					moveTimer = 0;
					return;
				}
			}
			if (transform.position.x - player.transform.position.x > 0 && !Physics.Raycast(new Vector3(moveToPos2.x - moveDist, moveDist * 2, moveToPos2.z), Vector3.down, moveDist * 2, whatIsNonWalkable) && Physics.Raycast(new Vector3(moveToPos2.x - moveDist, moveDist * 2, moveToPos2.z), Vector3.down, moveDist * 2, whatIsGround))
			{
				moveToPos2 -= Vector3.right * moveDist;
				if ((Mathf.Abs(toPlayer.normalized.x) == 0 || Mathf.Abs(toPlayer.normalized.x) == 1) && (Mathf.Abs(toPlayer.normalized.z) == 0 || Mathf.Abs(toPlayer.normalized.z) == 1))
				{
					transform.position = moveToPos2;
					moveTimer = 0;
					return;
				}
			}
			else if (transform.position.x - player.transform.position.x < 0 && !Physics.Raycast(new Vector3(moveToPos2.x + moveDist, moveDist * 2, moveToPos2.z), Vector3.down, moveDist * 2, whatIsNonWalkable) && Physics.Raycast(new Vector3(moveToPos2.x + moveDist, moveDist * 2, moveToPos2.z), Vector3.down, moveDist * 2, whatIsGround))
			{
				moveToPos2 += Vector3.right * moveDist;
				if ((Mathf.Abs(toPlayer.normalized.x) == 0 || Mathf.Abs(toPlayer.normalized.x) == 1) && (Mathf.Abs(toPlayer.normalized.z) == 0 || Mathf.Abs(toPlayer.normalized.z) == 1))
				{
					transform.position = moveToPos2;
					moveTimer = 0;
					return;
				}
			}
			
			Vector3 idealMoveToPos;
			if ((!CheckForPlayer (bullet.GetComponent<Bullet>().range) && Vector3.Distance(moveToPos, player.transform.position) < Vector3.Distance(moveToPos2, player.transform.position)) || (CheckForPlayer (bullet.GetComponent<Bullet>().range) && Vector3.Distance(moveToPos, player.transform.position) > Vector3.Distance(moveToPos2, player.transform.position)))
				idealMoveToPos = moveToPos;
			else
				idealMoveToPos = moveToPos2;
			
			if (Physics.Raycast(new Vector3(idealMoveToPos.x, moveDist * 2, idealMoveToPos.z), Vector3.down, moveDist * 2, whatIsGround))
			{
				transform.position = idealMoveToPos;
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
		if (other.tag == "Bullet" && other.GetComponent<Bullet>().shooter != gameObject)
		{
			hp -= other.GetComponent<Bullet>().damage;
			Destroy(other.gameObject);
		}
	}

	void OnTriggerStay (Collider other)
	{
		if (other.gameObject.name == "PlayerSword" && player.playerAttackTimer > player.playerAttackRate)
		{
			player.playerAttackTimer = 0;
			hp --;
			if (hp <= 0)
			{
				player.score += 1;
				if (player.score > PlayerPrefs.GetInt("Score", 0))
					PlayerPrefs.SetInt("Score", player.score);
				Destroy(gameObject);
			}
		}
	}
}
