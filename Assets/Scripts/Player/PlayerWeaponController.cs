using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PlayerRecoilController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerWeaponController : MonoBehaviour
{
	private PlayerRecoilController recoilController;
	private AudioSource audioSource;

	private Weapon PrimaryWeapon;
	private Weapon SecondaryWeapon;

	public int PrimaryIndex { get => primaries.ToList().IndexOf(PrimaryWeapon); }
	public int SecondaryIndex { get => secondaries.ToList().IndexOf(SecondaryWeapon); }
	public int PrimaryAmmo { get => PrimaryWeapon.Ammo; }
	public int SecondaryAmmo { get => SecondaryWeapon.Ammo; }
	public int PrimaryClipSize { get => PrimaryWeapon.ClipSize; }
	public int SecondaryClipSize { get => SecondaryWeapon.ClipSize; }
	public Weapon.WeaponType PrimaryWeaponType { get => PrimaryWeapon.Type; }
	public Weapon.WeaponType SecondaryWeaponType { get => SecondaryWeapon.Type; }
	public Boolean CanZoom { get => typeof(SniperRifle).IsInstanceOfType(PrimaryWeapon) || typeof(SniperRifle).IsInstanceOfType(SecondaryWeapon); }
	public Boolean CanSecondaryZoom { get => typeof(SniperRifle).IsInstanceOfType(SecondaryWeapon); }
	public Boolean CanPrimaryFire { get => PrimaryWeapon.CanWeaponFire(); }
	public Boolean CanSecondaryFire { get => SecondaryWeapon.CanWeaponFire(); }

	private readonly List<String> weaponKeys = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
	private readonly List<Weapon> primaries = new List<Weapon> {
		new SniperRifle(),
		new AssaultRifle(),
		new RocketLauncher(),
		new AutomaticRocketLauncher(),
		new GrenadeLauncher()
	};

	private readonly List<Weapon> secondaries = new List<Weapon> {
		new SniperRifle(),
		new AssaultRifle(),
		new RocketLauncher(),
		new AutomaticRocketLauncher(),
		new GrenadeLauncher()
	};

	public void SwitchPrimaryWeapon(int index)
	{
		PrimaryWeapon = primaries[index];
	}
	public void SwitchSecondaryWeapon(int index)
	{
		SecondaryWeapon = secondaries[index];
	}

	private void Awake()
	{
		recoilController = GetComponent<PlayerRecoilController>();
		audioSource = GetComponent<AudioSource>();
		PlayerHealth.instance.PlayerDiedEvent += () => { enabled = false; };
	}
	void Start()
	{
		PrimaryWeapon = primaries[0];
		SecondaryWeapon = secondaries[1];

		LoadAudioClips(primaries);
		LoadAudioClips(secondaries);
	}

	public void LateUpdate()
	{
		PrimaryWeapon.UpdateCooldownTimer(Time.deltaTime);
		SecondaryWeapon.UpdateCooldownTimer(Time.deltaTime);

		if (recoilController.IsRecoilReset)
		{
			PrimaryWeapon.ResetRecoil();
			SecondaryWeapon.ResetRecoil();
		}
		ChangeWeaponOnKeyPress();
	}

	public void Reload() {
		PrimaryWeapon.Reload();
		SecondaryWeapon.Reload();
	}

	public void FirePrimary()
	{
		SpawnPrimaryProjectile();
		AddPrimaryRecoil();
		audioSource.PlayOneShot(PrimaryWeapon.AudioClips[Random.Range(0, PrimaryWeapon.AudioClips.Count)]);
	}

	public void FireSecondary()
	{
		SpawnSecondaryProjectile();
		AddSecondaryRecoil();
		audioSource.PlayOneShot(SecondaryWeapon.AudioClips[Random.Range(0, SecondaryWeapon.AudioClips.Count)]);
	}

	private void AddPrimaryRecoil() => recoilController.AddRecoil(PrimaryWeapon.Shoot());
	private void AddSecondaryRecoil() => recoilController.AddRecoil(SecondaryWeapon.Shoot());

	private void SpawnPrimaryProjectile()
	{
		Instantiate(
						PrimaryWeapon.Projectile,
						transform.Find("PrimaryWeapons").position +
						Camera.main.transform.right * (-0.5f) +
						Camera.main.transform.forward,
						Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up)
						);
	}

	private void SpawnSecondaryProjectile()
	{
		Instantiate(
						SecondaryWeapon.Projectile,
						transform.Find("SecondaryWeapons").position +
						Camera.main.transform.right * (0.5f) +
						Camera.main.transform.forward,
						Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up)
						);
	}

	private void ChangeWeaponOnKeyPress()
	{
		weaponKeys.ForEach((i) =>
		{
			int index = weaponKeys.IndexOf(i);
			if (Input.GetKeyDown(i) && index < primaries.Count)
			{
				PrimaryWeapon = primaries[index];
			}
		});
	}

	public bool DoesPlayerWantToShootPrimary()
    {
		switch (PrimaryWeapon.Type)
		{
			case Weapon.WeaponType.SEMI_AUTOMATIC:
				return Input.GetMouseButtonDown(0);
			case Weapon.WeaponType.AUTOMATIC:
				return Input.GetMouseButton(0);
			default:
				return false;
		}
	}
	public bool DoesPlayerWantToShootSecondary()
	{
		switch (SecondaryWeapon.Type)
		{
			case Weapon.WeaponType.SEMI_AUTOMATIC:
				return Input.GetMouseButtonDown(1);
			case Weapon.WeaponType.AUTOMATIC:
				return Input.GetMouseButton(1);
			default:
				return false;
		}
	}

	private void LoadAudioClips(List<Weapon> weapons)
	{
		foreach (Weapon weapon in weapons)
		{
			weapon.Projectile = Resources.Load<GameObject>(weapon.ProjectileAssetName);
			Resources.LoadAll<AudioClip>(weapon.AudioClipFolderName).ToList().ForEach(
				audioclip => weapon.AudioClips.Add(audioclip));
		}
	}
}
