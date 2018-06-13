using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource ingameMusic;
	public AudioSource flames;
	private bool playFlames = false;
    public static AudioManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
		flames.volume = 0.0f;
        ingameMusic.Play();
    }        

	void Update() {
		if (playFlames == true) {
			flames.volume += Time.deltaTime;
			flames.volume = Mathf.Min (flames.volume, 1.0f);
		} 
		else 
		{
			flames.volume -= Time.deltaTime;
			flames.volume = Mathf.Max (flames.volume, 0.0f);
		}
		//playFlames = false;
	}
    
    public void PlayMusic(AudioClip clip)
    {
        ingameMusic.clip = clip;
    }

    // Use this for initialization
    void Start () {
	}

    public void PlayShootSound()
    {
        AudioSource shoot = gameObject.AddComponent<AudioSource>();

        shoot.clip = Resources.Load<AudioClip>("Audio/Effects/Shoot");
        shoot.pitch = Random.Range(0.5f, 1.5f);
        shoot.volume = 1.0f;
        shoot.Play();
        Destroy(shoot, 1.0f);
    }

    public void PlayPowerUpSound(int type)
    {
        AudioSource powerUp = gameObject.AddComponent<AudioSource>();
		switch (type) 
		{
		case 1:
			powerUp.clip = Resources.Load<AudioClip>("Audio/Effects/PowerUp/SpeedUp");
			break;
		case 2:
			powerUp.clip = Resources.Load<AudioClip>("Audio/Effects/PowerUp/DamageUp");
			break;
		case 3:
			powerUp.clip = Resources.Load<AudioClip>("Audio/Effects/PowerUp/ArmorUp");
			break;
		case 4:
			powerUp.clip = Resources.Load<AudioClip>("Audio/Effects/PowerUp/RepairKit");
			break;
		case 5:
			powerUp.clip = Resources.Load<AudioClip>("Audio/Effects/PowerUp/Nitro");
			break;
		}
        powerUp.Play();
        Destroy(powerUp, 3.0f);
    }

    public void PlayCrateCollisionSound()
    {
        AudioSource collision = gameObject.AddComponent<AudioSource>();

        collision.clip = Resources.Load<AudioClip>("Audio/Effects/Collision/Crate");
        collision.Play();
        Destroy(collision, 1.0f);
    }

    public void PlayTankCollisionSound()
    {
        AudioSource collision = gameObject.AddComponent<AudioSource>();

        collision.clip = Resources.Load<AudioClip>("Audio/Effects/Collision/Tank");
        collision.Play();
        Destroy(collision, 1.0f);
    }

    public void PlayRockCollisionSound()
    {
        AudioSource collision = gameObject.AddComponent<AudioSource>();

        collision.clip = Resources.Load<AudioClip>("Audio/Effects/Collision/Rock");
        collision.Play();
        Destroy(collision, 1.0f);
    }

    public void PlayMapImpactSound()
    {
        AudioSource impact = gameObject.AddComponent<AudioSource>();

        impact.clip = Resources.Load<AudioClip>("Audio/Effects/ImpactEnvironment");
        impact.Play();
        Destroy(impact, 1.0f);
    }

    public void PlayTankImpactSound()
    {
        AudioSource impact = gameObject.AddComponent<AudioSource>();

        impact.clip = Resources.Load<AudioClip>("Audio/Effects/ImpactTank");
        impact.Play();
        Destroy(impact, 1.0f);
    }

    public void PlayCrateImpactSound()
    {
        AudioSource impact = gameObject.AddComponent<AudioSource>();

		impact.clip = Resources.Load<AudioClip>("Audio/Effects/ImpactEnvironment");
        impact.Play();
        Destroy(impact, 1.0f);
    }

    public void PlayRockImpactSound()
    {
        AudioSource impact = gameObject.AddComponent<AudioSource>();

		impact.clip = Resources.Load<AudioClip>("Audio/Effects/ImpactEnvironment");
        impact.Play();
        Destroy(impact, 1.0f);
    }

    public void PlayMinePlacedSound()
    {
        AudioSource minePlaced = gameObject.AddComponent<AudioSource>();

        minePlaced.clip = Resources.Load<AudioClip>("Audio/Effects/MinePlaced");
        minePlaced.Play();
        Destroy(minePlaced, 1.0f);
    }

    public void PlayMineDetonationSound()
    {
        AudioSource mineDetonation = gameObject.AddComponent<AudioSource>();

        mineDetonation.clip = Resources.Load<AudioClip>("Audio/Effects/MineDetonation");
        mineDetonation.Play();
        Destroy(mineDetonation, 5.0f);
    }

    public void PlayInvulnerabilitySound()
    {
        AudioSource invulnerability = gameObject.AddComponent<AudioSource>();

        invulnerability.clip = Resources.Load<AudioClip>("Audio/Effects/Invulnerability");
        invulnerability.Play();
		invulnerability.volume = 0.5f;
        Destroy(invulnerability, 6.0f);
    }

    public void PlayInvisibilitySound()
    {
        AudioSource invisibility = gameObject.AddComponent<AudioSource>();

        invisibility.clip = Resources.Load<AudioClip>("Audio/Effects/Invisibility");
        invisibility.Play();
		invisibility.volume = 0.1f;
        Destroy(invisibility, 5.0f);
    }

	public void StopFlameSound()
	{
		playFlames = false;	
	}	

    public void PlayFlameSound()
    {
		playFlames = true;
    }
}
