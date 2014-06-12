using UnityEngine;
using System.Collections;

public class Goal : Item {

	void OnMouseDown()
	{
		if (GameManager.Instance.CanBuy (value)) {

			LevelChallenge.Instance.TryToBuyNewGoal(this);
		}
		else
		{
			GameManager.Instance.CreateWarning("No enough money!", Color.red);
		}
	}

}
