using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource ingameMusic;
    public static AudioManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        ingameMusic.Play();
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
        shoot.pitch = Random.Range(1.0f, 1.25f);
        shoot.volume = 0.25f;
        shoot.Play();
        Destroy(shoot, 1.0f);
    }

    public void PlayPowerUpSound(int type)
    {
        AudioSource powerUp = gameObject.AddComponent<AudioSource>();

        powerUp.clip = Resources.Load<AudioClip>("Audio/Effects/PowerUp");
        powerUp.Play();
        Destroy(powerUp, 1.0f);
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

        impact.clip = Resources.Load<AudioClip>("Audio/Effects/Impact/Map");
        impact.Play();
        Destroy(impact, 1.0f);
    }

    public void PlayTankImpactSound()
    {
        AudioSource impact = gameObject.AddComponent<AudioSource>();

        impact.clip = Resources.Load<AudioClip>("Audio/Effects/Impact/Tank");
        impact.Play();
        Destroy(impact, 1.0f);
    }

    public void PlayCrateImpactSound()
    {
        AudioSource impact = gameObject.AddComponent<AudioSource>();

        impact.clip = Resources.Load<AudioClip>("Audio/Effects/Impact/Crate");
        impact.Play();
        Destroy(impact, 1.0f);
    }

    public void PlayRockImpactSound()
    {
        AudioSource impact = gameObject.AddComponent<AudioSource>();

        impact.clip = Resources.Load<AudioClip>("Audio/Effects/Impact/Rock");
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
        Destroy(mineDetonation, 1.0f);
    }

    public void PlayInvulnerabilitySound()
    {
        AudioSource invulnerability = gameObject.AddComponent<AudioSource>();

        invulnerability.clip = Resources.Load<AudioClip>("Audio/Effects/Invulnerability");
        invulnerability.Play();
        Destroy(invulnerability, 1.0f);
    }

    public void PlayInvisibilitySound()
    {
        AudioSource invisibility = gameObject.AddComponent<AudioSource>();

        invisibility.clip = Resources.Load<AudioClip>("Audio/Effects/Invisibility");
        invisibility.Play();
        Destroy(invisibility, 1.0f);
    }

    public void PlayFlameSound()
    {
        AudioSource flames = gameObject.AddComponent<AudioSource>();

        flames.clip = Resources.Load<AudioClip>("Audio/Effects/Flames");
        flames.Play();
        Destroy(flames, 1.0f);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
