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
	[SerializeField] private PlayerRecoilController recoilController;
	[SerializeField] private AudioSource audioSource;

	public int Ammo { get => CurrentWeapon.Ammo; }
	public int ClipSize { get => CurrentWeapon.ClipSize; }
	public Weapon.WeaponType WeaponType { get => CurrentWeapon.Type; }
	public Boolean CanWeaponZoom { get => typeof(SniperRifle).IsInstanceOfType(CurrentWeapon); }
	public Boolean CanWeaponFire { get => CurrentWeapon.CanWeaponFire(); }

	private Weapon CurrentWeapon;
	private readonly List<String> weaponKeys = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
	private readonly List<Weapon> weapons = new List<Weapon> {
		new SniperRifle(),
		new AssaultRifle(),
		new RocketLauncher(),
		new AutomaticRocketLauncher(),
		new GrenadeLauncher()
	};

	private void Awake()
	{
		Health.instance.PlayerDiedEvent += () => { enabled = false; };
	}
	void Start()
	{
		CurrentWeapon = weapons[0];
		foreach (Weapon weapon in weapons)
		{
			weapon.Projectile = Resources.Load<GameObject>(weapon.ProjectileAssetName);
			Resources.LoadAll<AudioClip>(weapon.AudioClipFolderName).ToList().ForEach(
				audioclip => weapon.AudioClips.Add(audioclip));
		}
	}

	void Update()
	{
		CurrentWeapon.UpdateCooldownTimer(Time.deltaTime);
	}

	public void LateUpdate()
	{
		if (recoilController.IsRecoilReset)
		{
			CurrentWeapon.ResetRecoil();
		}
		ChangeWeaponOnKeyPress();
	}

	public void Reload() => CurrentWeapon.Reload();

	public void Fire()
	{
		SpawnProjectile();
		AddRecoil();
		audioSource.PlayOneShot(CurrentWeapon.AudioClips[Random.Range(0, CurrentWeapon.AudioClips.Count)]);
	}

	private void AddRecoil() => recoilController.AddRecoil(CurrentWeapon.Shoot());

	private void SpawnProjectile()
	{
		Instantiate(
						CurrentWeapon.Projectile,
						Camera.main.transform.position +
						Camera.main.transform.forward * 1.5f +
						Camera.main.transform.right * (AlternateFire() ? -.25f : .25f),
						Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up)
						);
	}

	private bool AlternateFire() => CurrentWeapon.Ammo % 2 == 0;

	private void ChangeWeaponOnKeyPress()
	{
		weaponKeys.ForEach((i) =>
		{
			int index = weaponKeys.IndexOf(i);
			if (Input.GetKeyDown(i) && index < weapons.Count)
			{
				CurrentWeapon = weapons[index];
			}
		});
	}

	public bool DoesPlayerWantToShoot()
    {
		switch (CurrentWeapon.Type)
		{
			case Weapon.WeaponType.SEMI_AUTOMATIC:
				return Input.GetMouseButtonDown(0);
			case Weapon.WeaponType.AUTOMATIC:
				return Input.GetMouseButton(0);
			default:
				return false;
		}
	}
}
