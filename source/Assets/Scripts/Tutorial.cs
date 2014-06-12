using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {

	public GUISkin newSkin;

	public GameObject items;
	public GameObject colors;
	public GameObject rules;
	public GameObject mouse;
	public GameObject anyKey;

	private int index = 0;
	
	void Start () {

		items.SetActive (true);
		colors.SetActive (false);

		rules.SetActive (false);

		mouse.SetActive (false);
		anyKey.SetActive (false);
	}

	void Update () 
	{
		if (Input.anyKeyDown)
		{
			index++;

			switch (index) {
				case 1:
					colors.SetActive(true);
					break;
				case 2:
					items.SetActive(false);
					colors.SetActive(false);

					rules.SetActive(true);
					break;
				case 3:
				default:
					rules.SetActive(false);

					mouse.SetActive(true);
					anyKey.SetActive(true);
					break;
			}
		}
	}

	void OnGUI()
	{
		if (index <= 2)
		{
			//newSkin.label.fontSize = 40;
			//newSkin.label.normal.textColor = Color.yellow;
			GUI.skin = newSkin;
			
			string text = "Press any key to continue!";
			Vector2 vectText = GUI.skin.GetStyle("Label").CalcSize(new GUIContent(text));
			Rect textRect = new Rect(Screen.width / 2 - vectText.x / 2, 
			                         Screen.height - vectText.y * 1.5f, 
			                         vectText.x, vectText.y);
	//		Rect textRect = new Rect(Screen.width / 2 - vectText.x / 2f, 
	//		                         Screen.height / 2 - vectText.y * 2.5f, 
	//		                         vectText.x, vectText.y);
			
			GUI.Label(textRect, text);
		}
		else
		{
			//newSkin.label.fontSize = 40;
			//newSkin.label.normal.textColor = Color.white;
		}
	}
}
