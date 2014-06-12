using System;
using UnityEngine;
using System.Collections;

public enum ItemType
{
	Broccoli,
	Carrot,
	Corn,
	Onion,
	Potato,
	Tomato,
	Spade,
	Chicken,
	Piggy,
	Scarecrow,
	Irrigator9000,
	WateringCan
}

public class Item : MonoBehaviour {

	public ItemType itemType;

	[HideInInspector]
	public float value;

	private GUIText guiTextChild;

	void Start () {

		string name = gameObject.name;

		itemType = (ItemType)Enum.Parse (typeof(ItemType), name);

		value = ItemsMarket.Instance.GetValue (itemType);

		guiTextChild = GetComponentInChildren<GUIText> ();
		guiTextChild.text = value.ToString ("C2");
	}	

	protected void OnClickToBuy()
	{
		if (GameManager.Instance.CanBuy (value)) {
			
			GameManager.Instance.TryToBuyNewItem (gameObject);
		}
		else
		{
			GameManager.Instance.CreateWarning("No enough money!", Color.red);
		}
	}
}