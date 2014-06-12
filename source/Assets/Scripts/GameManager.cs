using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameStatus
{
	Nothing,
	Victory,
	GameOver
}

public class GameManager : MonoBehaviour {

	public static GameManager Instance;
	public GUISkin newSkin;

	public AudioClip audioVictory;
	public AudioClip audioGameOver;

	public GUIText guiTextMoney;
	public GUIText guiTextTimer;
	public GUIText guiTextWarning;
	
	public GameObject cauldron;
	public float countDownSeconds;

	private float startTime = 0;
	private float restSeconds = 0;
	private int roundedRestSeconds = 0;
	private int displaySeconds = 0;
	private int displayMinutes = 0;

	public float money;
	private string nextLevel;
	private string previousLevel;

	private GameStatus gameStatus = GameStatus.Nothing;

	private Transform itemsGUITransform;
	public List<GUIText> guiTextsPrices;

	private GameObject textGO;

	void Awake () {
	
		if (Instance == null)
		{
			Instance = this;
		}
	}

	void CountGo1()
	{
		textGO.guiText.text = "2";
		Invoke ("CountGo2", 1f);
	}
	void CountGo2()
	{
		textGO.guiText.text = "1";
		Invoke ("CountGo3", 1f);
	}
	void CountGo3()
	{
		textGO.guiText.text = "Ready!";
		Invoke ("CountGo4", 1f);
	}
	void CountGo4()
	{
		textGO.guiText.text = "Go!";
		Invoke ("CountGo5", 1f);
	}
	void CountGo5()
	{
		Destroy (textGO);
	}

	void Start()
	{
		textGO = GameObject.FindGameObjectWithTag ("GO");
		textGO.guiText.text = "3";
		Invoke ("CountGo1", 1f);

		int index = Convert.ToInt32(Application.loadedLevelName.Replace ("Level", string.Empty));
		index++;

		if (index > 10)
		{
			nextLevel = "TheEnd";
		}
		else
		{
			nextLevel = String.Concat ("PreLevel", index);
		}

		previousLevel = Application.loadedLevelName;

		itemsGUITransform = GameObject.Find ("GUI Items").transform;

		guiTextTimer.text = "0";
		guiTextMoney.color = Color.yellow;

		guiTextWarning.text = "";
		guiTextWarning.enabled = false;

		if (countDownSeconds <= 0)
		{
			Debug.LogError("É preciso setar o tempo da fase!");
		}
	}

	void Update()
	{
		if (gameStatus == GameStatus.Nothing)
		{
			countDownSeconds -= Time.deltaTime;
			countDownSeconds = Mathf.Clamp (countDownSeconds, 0, 99999);
		
			if (countDownSeconds <= 0)
			{
				GameOver();
			}
		}
	}
	
	void OnGUI()
	{
		GUI.skin = newSkin;

		guiTextMoney.text = money.ToString ("F2");

		float guiTime = Time.deltaTime - startTime;
		restSeconds = countDownSeconds - guiTime;

		roundedRestSeconds = Mathf.CeilToInt (restSeconds);
		displaySeconds = roundedRestSeconds % 60;
		displayMinutes = roundedRestSeconds / 60;

		guiTextTimer.text = String.Format ("{0:00}:{1:00}", displayMinutes, displaySeconds);

		if (countDownSeconds <= 30) 
		{
			guiTextTimer.color = Color.red;		
		}		
		else if (countDownSeconds <= 60) 
		{
			guiTextTimer.color = Color.yellow;		
		}
		else
		{
			guiTextTimer.color = Color.white;
		}

		if ((gameStatus != GameStatus.Nothing && Input.anyKeyDown) || GUI.Button(new Rect(Screen.width - 130, Screen.height - 55, 120, 40), "Reload Level"))
		{
			string levelName = gameStatus == GameStatus.Victory ? nextLevel : previousLevel;

			DontDestroyOnLoad(SoundManager.Instance.gameObject);

			//AutoFade.LoadLevel(levelName, 1.5f, 1.5f, Color.black);
			Application.LoadLevel(levelName);
		}
	}
	
	public bool CanBuy (float value)
	{
		return value <= money;
	}

	public void TryToBuyNewItem (GameObject objectToClone)
	{
		// Cria um objeto para ficar no lugar.
		GameObject replacementGameObject = (GameObject)Instantiate (objectToClone, objectToClone.transform.position, Quaternion.identity);
		replacementGameObject.name = objectToClone.name;
		replacementGameObject.transform.parent = itemsGUITransform;

		// Desconta o valor.
		Item item = objectToClone.GetComponent<Item> ();
		UseMoney (item.value);

		// Altera o comportamento do objeto atual.
		Destroy(item);
		foreach (Transform child in objectToClone.transform) {
			Destroy(child.gameObject);
		}
		objectToClone.AddComponent<DragAndDrop> ();
	}

	public void CreateWarning (string newText, Color? newColor = null, bool hide = true)
	{
		if (!newColor.HasValue)
		{
			newColor = Color.white;
		}
		
		guiTextWarning.color = newColor.Value;
		guiTextWarning.text = newText;

		guiTextWarning.enabled = true;

		if (hide)
		{
			Invoke("BackToHide", 5f);
		}
		else
		{
			guiTextWarning.transform.position = new Vector3(0.5f, 0.5f, 0f);
		}
	}

	void BackToHide()
	{
		guiTextWarning.text = "";
		guiTextWarning.enabled = false;
	}

	public void AddMoney (float value)
	{
		money += value;
	}

	public void UseMoney (float value)
	{
		money -= value;
	}

	public void Victory ()
	{
		gameStatus = GameStatus.Victory;
		audio.PlayOneShot (audioVictory);

		StopGame ("Goal reached! Press any key to continue.", Color.green);
	}

	public void GameOver ()
	{
		gameStatus = GameStatus.GameOver;
		audio.PlayOneShot (audioGameOver);

		StopGame ("Goal NOT reached! Press any key to reload.", Color.red);
	}

	void StopGame(string message, Color newColor)
	{
		Destroy (itemsGUITransform.gameObject);
		//itemsGUITransform.gameObject.SetActive (false);
		guiTextsPrices.ForEach(g => 
       	{ 
			var parent = g.transform.parent;
			Destroy (parent.GetComponent<Goal> ());
			Destroy (parent.collider2D);

			g.enabled = false; 
		});

		guiTextWarning.fontSize = 60;
		GameManager.Instance.CreateWarning(message, newColor, false);
	}
}
