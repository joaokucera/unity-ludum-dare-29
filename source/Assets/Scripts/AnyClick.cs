using UnityEngine;
using System.Collections;

public enum SceneType
{
	Menu,
	TheEnd,
	Tutorial,
	PreLevel,
	Level
}

public class AnyClick : MonoBehaviour {

	public GUISkin newSkin;
	public SceneType sceneType = SceneType.PreLevel;
	public SceneType nextSceneType = SceneType.Level;
	public AudioClip audioClick;
	public string complementText;

	private string nextLevel;

	void Start()
	{
		if (sceneType == SceneType.PreLevel)
		{
			nextLevel = Application.loadedLevelName.Replace ("Pre", string.Empty);
		}
		else if (sceneType == SceneType.Tutorial)
		{
			nextLevel = "PreLevel1";
		}
		else
		{
			nextLevel = nextSceneType.ToString();
		}

		Debug.Log ("Próxima fase: " + nextLevel);
	}

	void Update () 
	{
		if (Input.anyKeyDown)
		{
			audio.PlayOneShot(audioClick);
			DontDestroyOnLoad(SoundManager.Instance.gameObject);

			//AutoFade.LoadLevel(nextLevel, 1.5f, 1.5f, Color.black);
			Application.LoadLevel(nextLevel);
		}
	}

	void OnGUI()
	{
		if (sceneType == SceneType.Menu || sceneType == SceneType.TheEnd)
		{
			newSkin.label.fontSize = 60;
		}

		GUI.skin = newSkin;

		string text = "Press any key to start!";
		if (!string.IsNullOrEmpty(complementText))
		{
			text = string.Format("Press any key to {0}!", complementText);
		}

		Vector2 vectText = GUI.skin.GetStyle("Label").CalcSize(new GUIContent(text));
		Rect textRect = new Rect(Screen.width / 2 - vectText.x / 2, 
			                    Screen.height - vectText.y * 1.5f, 
		                        vectText.x, vectText.y);

		GUI.Label(textRect, text);
	}
}
