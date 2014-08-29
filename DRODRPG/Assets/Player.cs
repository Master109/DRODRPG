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
	public ArrayList combinedQuestEvents = new ArrayList();

	// Use this for initialization
	void Start ()
	{
		if (restart)
			PlayerPrefs.DeleteAll();
		moveRate -= moveDelayTime;
		moveDelayTimer = -1;
		if (PlayerPrefs.GetInt("Playing", 1) == 0 && PlayerPrefs.GetInt("Saved", 0) == 1)
		{
			PlayerPrefs.SetInt("Playing", 1);
			Load ();
		}
		else if (PlayerPrefs.GetInt("Saved", 0) == 0)
			GameObject.Find ("InitialSpace").GetComponent<Dialog>().TriggerDialog();
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
		foreach (string s in combinedQuestEvents)
		{
			TriggerCombinedQuestEvent(s);
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

	public void SetObjectLayer (string str)
	{
		int indexOfComma = str.IndexOf(",");
		string goName = str.Substring(0, indexOfComma);
		Debug.Log ("goName = " + goName);
		string layerName = str.Substring(indexOfComma + 1, str.Length - indexOfComma - 1);
		Debug.Log ("layerName = " + layerName);
		GameObject.Find(goName).layer = LayerMask.NameToLayer(layerName);
	}

	public void MessageAfterQuestAccept (string message)
	{
		messageAfterQuestAccept = message;
	}

	public void TriggerQuestEvent (string questEvent)
	{
		Parley.GetInstance().TriggerQuestEvent(questEvent);
	}

	public void TriggerCombinedQuestEvent (string str)
	{
		int indexOfComma1 = str.IndexOf(",");
		int indexOfComma2 = str.LastIndexOf(",");
		string questEvent1 = str.Substring(0, indexOfComma1);
		string questEvent2 = str.Substring(indexOfComma1 + 1, indexOfComma2 - indexOfComma1);
		string questEvent3 = str.Substring(indexOfComma2 + 1, str.Length - indexOfComma2 - 1);
		bool questEvent1True = (Parley.GetInstance().GetQuestEventSet().Contains(questEvent1) || (questEvent1.Contains("!") && !Parley.GetInstance().GetQuestEventSet().Contains(questEvent1)));
		bool questEvent2True = (Parley.GetInstance().GetQuestEventSet().Contains(questEvent2) || (questEvent2.Contains("!") && !Parley.GetInstance().GetQuestEventSet().Contains(questEvent2)));
		if (questEvent1True || questEvent2True)
		{
			Parley.GetInstance().TriggerQuestEvent(questEvent3);
			if (combinedQuestEvents.Contains(str))
				combinedQuestEvents.Remove(str);
		}
	}

	public void AddCombinedQuestEventListener (string str)
	{
		combinedQuestEvents.Add(str);
	}

	public void Save ()
	{
		Parley.GetInstance().GetComponent<SaveLoadGui>().Save("DROD RPG Savefile 2.txt");
		LevelSerializer.SerializeLevelToFile("DROD RPG Savefile.txt");
		PlayerPrefs.SetInt("Saved", 1);
	}

	public void Load ()
	{
		Parley.GetInstance().GetComponent<SaveLoadGui>().Load("DROD RPG Savefile 2.txt");
		LevelSerializer.LoadSavedLevelFromFile("DROD RPG Savefile.txt");
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
