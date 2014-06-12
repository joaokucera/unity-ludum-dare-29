using UnityEngine;
using System.Collections;

public class Caudron : MonoBehaviour {
	
	public GameObject prefabMoney;
	private GUIText guiTextClock;

	public Material matSpriteDiffuse;
	public Material matSpriteDefault;

	private bool vegatableCatched = false;
	private float vegetableValue = 0;

	private ParticleSystem particles;

	private float cookTime = 5f;
	private float clockTimer;

	void Start()
	{
		guiTextClock = GetComponentInChildren<GUIText> ();

		particles = GetComponentInChildren<ParticleSystem> ();
		particles.enableEmission = false;
	}

	void Update()
	{
		clockTimer -= Time.deltaTime;
		clockTimer = Mathf.Clamp (clockTimer, 0, 99999);
		
		if (clockTimer <= 0)
		{
			renderer.material = matSpriteDefault;
			guiTextClock.color = Color.white;
		}
		else
		{
			renderer.material = matSpriteDiffuse;
			guiTextClock.color = Color.yellow;
		}
		
		guiTextClock.text = clockTimer.ToString ("00");
	}

	void ToCook()
	{
		particles.enableEmission = false;
		audio.Stop ();

		//float value = vegatable.GetComponent<Vegetable> ().value;
		//vegatable = null;
		vegatableCatched = false;

		GameObject newMoney = (GameObject)Instantiate (prefabMoney, transform.position, Quaternion.identity);
		newMoney.GetComponent<Money> ().value = vegetableValue;
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Vegetable")
		{
			//if (vegatable == null)
			if (!vegatableCatched)
			{
				vegatableCatched = true;
				vegetableValue = collider.gameObject.GetComponent<Vegetable> ().value;

				Destroy(collider.gameObject);
				//vegatable = collider.gameObject;
				//DestroyComponentsFromCollider (collider);
				clockTimer = cookTime;

				GameManager.Instance.CreateWarning(
					string.Format("The baked will be ready in {0}s!", (int)cookTime), 
					Color.yellow);

				particles.enableEmission = true;
				audio.Play ();

				Invoke("ToCook", cookTime);
			}
			else
			{
				GameManager.Instance.CreateWarning("The caudron is cooking a vegetable!", Color.red);
			}
		}
	}

//	void DestroyComponentsFromCollider (Collider2D collider)
//	{
//		Destroy (collider.gameObject.GetComponent<DragAndDrop> ());
//		Destroy (collider.gameObject.GetComponent<SpriteRenderer> ());
//		Destroy (collider);
//	}
}
