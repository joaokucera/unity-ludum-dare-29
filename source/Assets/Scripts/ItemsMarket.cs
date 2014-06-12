using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemsMarket : MonoBehaviour {

	public static ItemsMarket Instance;

	public GUIText guiTextWaterCommonTime;
	public GUIText guiTextWaterFastTime;

	private Dictionary<ItemType, float> seedsValue;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	void Start()
	{
		guiTextWaterCommonTime.text = Garden.WaterCommonTime + " sec";
		guiTextWaterFastTime.text = Garden.WaterFastTime + " sec";
	}	

	void FillDictionary()
	{
		seedsValue = new Dictionary<ItemType, float>
		{
			{ ItemType.Broccoli, 2 },
			{ ItemType.Carrot, 4 },
			{ ItemType.Corn, 6 },
			{ ItemType.Onion, 8 },
			{ ItemType.Potato, 10 },
			{ ItemType.Tomato, 12 },
			{ ItemType.Spade, 5 },
			{ ItemType.Chicken, 50 },
			{ ItemType.Piggy, 100 },
			{ ItemType.Scarecrow, 200 },
			{ ItemType.Irrigator9000, 8 },
			{ ItemType.WateringCan, 5 },
		};
	}
	
	public float GetValue (ItemType seed)
	{
		if (seedsValue == null || seedsValue.Count <= 0)
		{
			FillDictionary();
		}

		float value;

		seedsValue.TryGetValue(seed, out value);

		return value;
	}
}
