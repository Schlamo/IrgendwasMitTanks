using UnityEngine;

using Enumerators;

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

<<<<<<< HEAD
        flames.volume = 0.0f;
        ingameMusic.Play();
=======
        DontDestroyOnLoad(gameObject);
		flames.volume = 0.0f;
        //ingameMusic.Play();
>>>>>>> 1059c7cecd707a3a31dfada716a182e31b89eb1c
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
        shoot.volume = 0.5f;
        shoot.Play();
        Destroy(shoot, 1.0f);
    }

    public void PlayPowerUpSound(PowerUpType type)
    {
        AudioSource powerUp = gameObject.AddComponent<AudioSource>();
		switch (type) 
		{
		case PowerUpType.SpeedUp:
			powerUp.clip = Resources.Load<AudioClip>("Audio/Effects/PowerUp/SpeedUp");
<<<<<<< HEAD
			break;
		case PowerUpType.DamageUp:
=======
                powerUp.volume = 0.5f;
                break;
		case 2:
>>>>>>> 1059c7cecd707a3a31dfada716a182e31b89eb1c
			powerUp.clip = Resources.Load<AudioClip>("Audio/Effects/PowerUp/DamageUp");
                powerUp.volume = 2.0f;
			break;
		case PowerUpType.ArmorUp:
			powerUp.clip = Resources.Load<AudioClip>("Audio/Effects/PowerUp/ArmorUp");
<<<<<<< HEAD
			break;
		case PowerUpType.RepairKit:
=======
                powerUp.volume = 0.5f;
                break;
		case 4:
>>>>>>> 1059c7cecd707a3a31dfada716a182e31b89eb1c
			powerUp.clip = Resources.Load<AudioClip>("Audio/Effects/PowerUp/RepairKit");
			break;
		case PowerUpType.Nitro:
			powerUp.clip = Resources.Load<AudioClip>("Audio/Effects/PowerUp/Nitro");
			break;
		}
        powerUp.Play();
        Destroy(powerUp, 3.0f);
    }

    public void PlayCrateCollisionSound()
    {
        AudioSource collision = gameObject.AddComponent<AudioSource>();

        collision.clip = Resources.Load<AudioClip>("Audio/Effects/ImpactEnvironment");
        collision.volume = 0.25f;
        collision.Play();
        Destroy(collision, 1.0f);
    }

    public void PlayTankCollisionSound()
    {
        AudioSource collision = gameObject.AddComponent<AudioSource>();

        collision.clip = Resources.Load<AudioClip>("Audio/Effects/ImpactTank");
        collision.volume = 0.2f;
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

    public void PlayTankImpactSound()
    {
        AudioSource impact = gameObject.AddComponent<AudioSource>();

        impact.clip = Resources.Load<AudioClip>("Audio/Effects/ImpactTank");
        impact.Play();
        Destroy(impact, 1.0f);
    }

    public void PlayMapImpactSound()
    {
        AudioSource impact = gameObject.AddComponent<AudioSource>();

        impact.clip = Resources.Load<AudioClip>("Audio/Effects/ImpactEnvironment");
        impact.Play();
        impact.volume = 0.25f;
        Destroy(impact, 1.0f);
    }

    public void PlayCrateImpactSound()
    {
        AudioSource impact = gameObject.AddComponent<AudioSource>();

		impact.clip = Resources.Load<AudioClip>("Audio/Effects/ImpactEnvironment");
        impact.volume = 0.25f;
        impact.Play();
        Destroy(impact, 1.0f);
    }

    public void PlayRockImpactSound()
    {
        AudioSource impact = gameObject.AddComponent<AudioSource>();

		impact.clip = Resources.Load<AudioClip>("Audio/Effects/ImpactEnvironment");
        impact.volume = 0.25f;
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
        mineDetonation.volume = 0.5f;
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
