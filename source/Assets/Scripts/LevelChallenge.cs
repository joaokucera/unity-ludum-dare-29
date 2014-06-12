using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelChallenge : MonoBehaviour {

	public static LevelChallenge Instance;

	public List<SpriteRenderer> spritesChallenge;
	public List<GUIText> guiTextsChallenge;

	public Material matDefault;
	public Material matDiffuse;

	private int chickens = 0;
	private int piggies = 0;
	private int scarecrows = 0;

	public int chickensRequired = 0;
	public int piggiesRequired = 0;
	public int scarecrowsRequired = 0;

	void Awake () {
	
		if (Instance == null)
		{
			Instance = this;
		}
	}

	void Start()
	{
		// Desabilitando todos os sprites.
		//spritesChallenge.ForEach(s => 
 		//{
			//s.gameObject.SetActive(false);
		//});
		guiTextsChallenge.ForEach(s => 
        {
			s.color = Color.yellow;
		});

		if (piggiesRequired <= 0) 
		{
			spritesChallenge [1].material = matDiffuse;
			Destroy(spritesChallenge [1].gameObject.collider2D);
			Destroy(spritesChallenge [1].gameObject.GetComponent<Goal>());

			//guiTextsChallenge [1].enabled = false;
			guiTextsChallenge [1].color = Color.gray;
			Destroy(guiTextsChallenge[1].transform.parent.GetComponentInChildren<GUIText>());
		}

		if (scarecrowsRequired <= 0) 
		{
			spritesChallenge [2].material = matDiffuse;
			Destroy(spritesChallenge [2].gameObject.collider2D);
			Destroy(spritesChallenge [2].gameObject.GetComponent<Goal>());

			//guiTextsChallenge [2].enabled = false;
			guiTextsChallenge [2].color = Color.gray;
			Destroy(guiTextsChallenge[2].transform.parent.GetComponentInChildren<GUIText>());
		}


	}

	void OnGUI()
	{
		//spritesChallenge [0].gameObject.SetActive(true);
		guiTextsChallenge [0].text = string.Format("{0}/{1}", chickens, chickensRequired);

		//if (piggiesRequired > 0)
		//{
			//spritesChallenge [1].gameObject.SetActive(true);
		guiTextsChallenge [1].text = string.Format("{0}/{1}", piggies, piggiesRequired);
		//}

		//if (scarecrowsRequired > 0)
		//{
			//spritesChallenge [2].gameObject.SetActive(true);
		guiTextsChallenge [2].text = string.Format("{0}/{1}", scarecrows, scarecrowsRequired);
		//}
	}

	public void TryToBuyNewGoal(Goal item)
	{
		GameManager.Instance.UseMoney (item.value);

		switch (item.itemType) 
		{
			case ItemType.Chicken:
				chickens++;
				break;
			case ItemType.Piggy:
				piggies++;
				break;
			case ItemType.Scarecrow:
			default:
				scarecrows++;
				break;
		}

		if (chickens >= chickensRequired && 
		    piggies >= piggiesRequired && 
		    scarecrows >= scarecrowsRequired)
		{
			GameManager.Instance.Victory();
		}
	}
}
