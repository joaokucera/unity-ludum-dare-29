using UnityEngine;
using System.Collections;

public class Cashier : MonoBehaviour {
	
	public AudioClip soundMoney;

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Money")
		{
			audio.PlayOneShot(soundMoney);

			GameManager.Instance.AddMoney(collider.GetComponent<Money>().value);
			Destroy(collider.gameObject);
		}
	}
}
