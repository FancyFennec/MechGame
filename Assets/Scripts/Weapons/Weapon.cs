using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Weapon
{
	public enum WeaponType
	{
		SEMI_AUTOMATIC,
		AUTOMATIC
	}

	public WeaponType Type { get; private set; }
	public String ProjectileAssetName { get; private set; }
	public GameObject Projectile { get; set; }
	public String AudioClipFolderName { get; private set; }
	public List<AudioClip> AudioClips { get; } = new List<AudioClip>();
	public int Damage { get; private set; }
	public float Rps { get; private set; }
	public int Ammo { get; set; }
	public int ClipSize { get; private set; }
	public int ClipCount { get; private set; }
	public int MaxClipCount { get; private set; }

	private float weaponTimer = 0f;
	private readonly float weaponCooldown;
	private bool isOnCoolDown = false;

	private float reloadTimer = 0f;
	private readonly float reloadCooldown = 2f;
	private bool isReloading = false;

	protected List<Vector2> recoilPattern = new List<Vector2>();
	private int recoilIndex = 0;

	public Weapon(WeaponType weaponType, String projectileAssetName, int damage, int clipSize, float rps)
	{
		this.Damage = damage;
		this.Ammo = clipSize;
		this.ClipSize = clipSize;
		this.ClipCount = 1;
		this.Type = weaponType;
		this.ProjectileAssetName = projectileAssetName;
		this.Rps = rps;
		this.weaponCooldown = 1f / rps;
	}

	public void UpdateCooldownTimer(float delta)
	{
		if (isReloading)
		{
			reloadTimer = Mathf.Clamp(reloadTimer - delta, 0f, reloadCooldown);
			if (reloadTimer <= 0f)
			{
				Ammo = ClipSize;
				isReloading = false;
			}
		} else if(isOnCoolDown)
		{
			weaponTimer = Mathf.Clamp(weaponTimer - delta, 0f, weaponCooldown);
			if (weaponTimer <= 0f)
			{
				isOnCoolDown = false;
			}
		}
	}

	public bool CanWeaponFire()
	{
		return !(isOnCoolDown || Ammo < 1 || isReloading);
	}

	public virtual Vector2 Shoot()
	{
		isOnCoolDown = true;
		weaponTimer = weaponCooldown;
		Ammo--;
		return recoilPattern[recoilIndex++];
	}

	public void ResetRecoil()
	{
		recoilIndex = 0;
	}

	public void Reload()
	{
		if (!isReloading)
		{
			isReloading = true;
			reloadTimer = reloadCooldown;
		}
	}
}
