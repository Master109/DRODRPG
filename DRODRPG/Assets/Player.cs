using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public float moveRate;
	float moveTime;
	RaycastHit hit;
	int moveDist = 4;
	public LayerMask whatIsGround;
	public LayerMask whatIsNonWalkable;
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
	int currentQuestPoint;
	int currentDialog;
	public int kills;
	public bool canSayBye = true;
	public bool canChangeSayBye = true;
	public bool canChangeExitArea = true;
	public string messageAfterQuestAccept = "";
	public bool restart;

	// Use this for initialization
	void Start ()
	{
		if (restart)
			PlayerPrefs.DeleteAll();
		GameObject.Find ("InitialSpace").GetComponent<Dialog>().TriggerDialog();
		moveRate -= moveDelayTime;
		moveDelayTimer = -1;
		if (PlayerPrefs.GetInt("Playing", 1) == 0 && PlayerPrefs.GetInt("Saved", 0) == 1)
		{
			PlayerPrefs.SetInt("Playing", 1);
			Load ();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameObject.Find("Scripts").GetComponent<Global>().timeScale2 == 0)
			return;
		moveTime += Time.deltaTime * GameObject.Find("Scripts").GetComponent<Global>().timeScale2;
		attackTimer += Time.deltaTime * GameObject.Find("Scripts").GetComponent<Global>().timeScale2;
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
			moveDelayTimer += Time.deltaTime * GameObject.Find("Scripts").GetComponent<Global>().timeScale2;
			if (moveDelayTimer > moveDelayTime)
			{
				if (turnAxis == 0)
				{
					if (xAxis != 0)
						zAxis = (int) Input.GetAxisRaw("Z");
					else
						xAxis = (int) Input.GetAxisRaw("X");
					if ((xAxis != 0 && zAxis == 0) && Physics.Raycast(new Ray(transform.position + Vector3.right * Input.GetAxisRaw("X") * moveDist, Vector3.down), moveDist, whatIsGround) && !Physics.Raycast(new Ray(transform.position + Vector3.right * Input.GetAxisRaw("X") * moveDist, Vector3.down), moveDist, whatIsNonWalkable))
					{
						moveTime = 0;
						transform.Translate(Vector3.right * Input.GetAxisRaw("X") * moveDist, Space.World);
					}
					else if ((xAxis == 0 && zAxis != 0) && Physics.Raycast(new Ray(transform.position + Vector3.forward * Input.GetAxisRaw("Z") * moveDist, Vector3.down), moveDist, whatIsGround) && !Physics.Raycast(new Ray(transform.position + Vector3.forward * Input.GetAxisRaw("Z") * moveDist, Vector3.down), moveDist, whatIsNonWalkable))
					{
						moveTime = 0;
						transform.Translate(Vector3.forward * Input.GetAxisRaw("Z") * moveDist, Space.World);
					}
					else if ((xAxis != 0 && zAxis != 0) && Physics.Raycast(new Ray(transform.position + (Vector3.right * Input.GetAxisRaw("X") * moveDist) + (Vector3.forward * Input.GetAxisRaw("Z") * moveDist), Vector3.down), moveDist, whatIsGround) && !Physics.Raycast(new Ray(transform.position + (Vector3.right * Input.GetAxisRaw("X") * moveDist) + (Vector3.forward * Input.GetAxisRaw("Z") * moveDist), Vector3.down), moveDist, whatIsNonWalkable))
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

	public void CreateQuestPoint (string goName)
	{
		if (GameObject.Find(goName) != null)
		{
			GameObject.Find(goName).renderer.enabled = true;
			GameObject.Find(goName).collider.enabled = true;
		}
		currentQuestPoint ++;
	}

	public void StartDialog (string goName)
	{
		GameObject.Find(goName).GetComponent<Dialog>().TriggerDialog();
		currentDialog ++;
	}

	public void SayByeFalse ()
	{
		if (canChangeSayBye)
			canSayBye = false;
	}

	public void SayByeTrue ()
	{
		if (canChangeSayBye)
			canSayBye = true;
	}

	public void CanChangeSayByeFalse ()
	{
		canChangeSayBye = false;
	}
	
	public void CanChangeSayByeTrue ()
	{
		canChangeSayBye = true;
	}

	public void ExitAreaTrue ()
	{
		if (canChangeExitArea)
			whatIsNonWalkable = LayerMask.GetMask();
	}

	public void ExitAreaFalse ()
	{
		if (canChangeExitArea)
			whatIsNonWalkable = LayerMask.GetMask("Beacon");
	}

	public void CanChangeExitAreaTrue ()
	{
		canChangeExitArea = true;
	}
	
	public void CanChangeExitAreaFalse ()
	{
		canChangeExitArea = false;
	}

	public void ObjectToDefaultLayer (string name)
	{
		GameObject.Find(name).layer = LayerMask.NameToLayer("Default");
	}

	public void MessageAfterQuestAccept (string message)
	{
		messageAfterQuestAccept = message;
	}

	public void Save ()
	{
		LevelSerializer.SerializeLevelToFile("DROD RPG Savefile.txt");
		//gameObject.StartExtendedCoroutine(SaveGameManager.Instance.GetComponent<TestSerialization>().Save ());
		PlayerPrefs.SetInt("Saved", 1);
	}

	public void Load ()
	{
		LevelSerializer.LoadSavedLevelFromFile("DROD RPG Savefile.txt");
		//gameObject.StartExtendedCoroutine(SaveGameManager.Instance.GetComponent<TestSerialization>().Load ());
	}

	void OnApplicationQuit ()
	{
		PlayerPrefs.SetInt("Playing", 0);
		if (restart)
			PlayerPrefs.DeleteAll();
	}
	
	void OnGUI ()
	{
		GUI.Label(new Rect(0, 0, Screen.width, 50), "Health: " + hp);
		if (survival)
		{
			GUI.Label (new Rect (0, 10, Screen.width, 50), "Best score: " + PlayerPrefs.GetInt("Score", 0));
			GUI.Label(new Rect(0, 20, Screen.width, 50), "Score: " + score);
		}
		else
		{
			GUI.Label(new Rect(0, 10, Screen.width, 50), "Gold: " + gold);
		}
	}
}
