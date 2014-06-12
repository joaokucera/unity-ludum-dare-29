using System;
using UnityEngine;
using System.Collections;

public enum VegetableType
{
	Broccoli = 16, // 13 + 2 = 15 (add: 1)
	Carrot = 19, // 13 + 4 = 17 (add: 2)
	Corn = 22, // 13 + 6 = 19 (add: 3)
	Onion = 25, // 13 + 8 = 21 (add: 4)
	Potato = 28, // 13 + 10 = 23 (add: 5)
	Tomato = 31 // 13 + 12 = 25 (add: 6)
}

public class Vegetable : MonoBehaviour {

	public float value;
	
	private VegetableType vegetableType;
	
	void Start()
	{
		string name = gameObject.name;
		
		vegetableType = (VegetableType)Enum.Parse (typeof(VegetableType), name);
		
		value = (int)vegetableType;
	}
}
