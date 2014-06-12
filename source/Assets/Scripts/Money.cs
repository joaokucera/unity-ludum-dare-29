using UnityEngine;
using System.Collections;

public class Money : MonoBehaviour {

	[HideInInspector]
	public float value;

	private Transform cashierTransform;

	void Start()
	{
		cashierTransform = GameObject.FindGameObjectWithTag ("Cashier").transform;
	}

	void Update () {
	
		transform.position = Vector2.MoveTowards(transform.position, 
		                                         cashierTransform.position, 
		                                         5f * Time.deltaTime);
	}
}