using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Garden : MonoBehaviour {
	
	public List<GameObject> vegetables;

	private GUIText guiTextClock;
	public Material matSpriteDiffuse;
	public Material matSpriteDefault;

	public SpriteRenderer iconsSpade;
	public SpriteRenderer iconsSeed;
	public SpriteRenderer iconsWater;

	private GameObject seed;
	private GameObject tool;
	private GameObject water;

	private bool availableToSeed = false;
	private bool availableToReceiveWater = false;

	public const float GroundTime = 2f;
	public const float SeedTime = 2f;
	public const float WaterFastTime = 2f;
	public const float WaterCommonTime = 5f;
	private float waterTime;
	private float clockTimer;

	void Start()
	{
		guiTextClock = GetComponentInChildren<GUIText> ();

		DisableIcons ();
	}

	void DisableIcons()
	{
		iconsSpade.enabled = false;
		iconsSeed.enabled = false;
		iconsWater.enabled = false;
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

	void ToSeed()
	{
		availableToSeed = true;
	}

	void ToPlant()
	{
		availableToReceiveWater = true;
	}

	void ToReap()
	{
		GameObject singleVegetable = vegetables.Single (v => v.name == seed.name);
		GameObject newVegetable = (GameObject)Instantiate (singleVegetable, 
                                   new Vector3(transform.position.x, transform.position.y + 0.5f, 0),
                                   Quaternion.identity);
		newVegetable.name = singleVegetable.name;

		DisableIcons ();

		tool = null;
		seed = null;
		availableToSeed = false;
		water = null;
		availableToReceiveWater = false;
	}
	
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Tool")
		{
			if (tool == null)
			{
				tool = collider.gameObject;
				SetIcon (collider, iconsSpade);
				clockTimer = GroundTime;

				GameManager.Instance.CreateWarning(
					string.Format("The ground will be ready in {0}s!", (int)GroundTime), 
					Color.yellow);

				Invoke("ToSeed", GroundTime);
			}
			else
			{
				GameManager.Instance.CreateWarning("This orta is already prepared!", Color.red);
			}
		}

		if (collider.tag == "Seed")
		{
			if (tool == null)
			{
				GameManager.Instance.CreateWarning("Use a spade to prepare the ground!", Color.red);
			}
			else if (seed == null && availableToSeed)
			{
				seed = collider.gameObject;
				SetIcon (collider, iconsSeed);
				clockTimer = SeedTime;

				GameManager.Instance.CreateWarning(
					string.Format("The seeding will be ready in {0}s!", (int)SeedTime), 
					Color.yellow);

				Invoke("ToPlant", SeedTime);
			}
			else if (seed == null && !availableToSeed)
         	{
				GameManager.Instance.CreateWarning("The ground is been prepared!", Color.red);
			}
			else
			{
				GameManager.Instance.CreateWarning("This orta is already seeded!", Color.red);
			}
		}

		if (collider.tag == "Water")
		{
			if (tool == null)
			{
				GameManager.Instance.CreateWarning("Use a spade to prepare the ground!", Color.red);
			}
			else if (seed == null)
			{
				GameManager.Instance.CreateWarning("Use a seed to plant!", Color.red);
			}
			else if (water == null && availableToReceiveWater)
			{
				waterTime = collider.gameObject.name == "WateringCan" ? WaterCommonTime : WaterFastTime;

				water = collider.gameObject;
				SetIcon (collider, iconsWater);
				clockTimer = waterTime;

				GameManager.Instance.CreateWarning(
					string.Format("The irrigation will be ready in {0}s!", (int)waterTime),
					Color.yellow);

				Invoke("ToReap", waterTime);
			}
			else if (water == null && !availableToReceiveWater)
			{
				GameManager.Instance.CreateWarning("The ground is been seeded!", Color.red);
			}
			else
			{
				GameManager.Instance.CreateWarning("This orta is already irrigated!", Color.red);
			}
		}
	}

	void SetIcon (Collider2D collider, SpriteRenderer spriteIcon)
	{
		Sprite icon = DestroyComponentsFromCollider (collider);

		spriteIcon.sprite = icon;
		spriteIcon.enabled = true;
	}

	Sprite DestroyComponentsFromCollider (Collider2D collider)
	{
		SpriteRenderer spriteRenderer = collider.gameObject.GetComponent<SpriteRenderer> ();
		Sprite sprite = spriteRenderer.sprite;

		Destroy (collider.gameObject.GetComponent<DragAndDrop> ());
		Destroy (spriteRenderer);
		Destroy (collider);

		return sprite;
	}
}
