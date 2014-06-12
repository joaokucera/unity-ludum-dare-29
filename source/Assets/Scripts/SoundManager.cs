using UnityEngine;
using System.Collections;

public enum SoundControl
{
    IN,
    OUT
}

public class SoundManager : MonoBehaviour
{
	public static SoundManager Instance;

    public SoundControl control = SoundControl.IN;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

    void Start()
    {
        audio.volume = 0f;
        audio.Play();
        audio.loop = true;
    }

    void Update()
    {
        if (control == SoundControl.IN)
        {
            audio.volume += Time.deltaTime / 10f;
            audio.volume = Mathf.Clamp(audio.volume, 0f, 0.5f);
        }
        else if (control == SoundControl.OUT)
        {
            audio.volume -= Time.deltaTime / 5f;
            audio.volume = Mathf.Clamp(audio.volume, 0f, 1f);
        }
    }
}
