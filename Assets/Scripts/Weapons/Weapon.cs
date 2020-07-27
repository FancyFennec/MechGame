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

	public WeaponType weaponType { get; private set; }
	public Projectile.ProjectileType projectileType { get; private set; }
	public int damage { get; private set; }
	public float rps { get; private set; }
	public int ammo { get; set; }
	public int clipSize { get; set; }
	public int clipCount { get; set; }
	public int maxClipCount { get; private set; }

	private float weaponTimer = 0f;
	private float weaponCooldown;
	private bool isOnCoolDown = false;

	private float reloadTimer = 0f;
	private float reloadCooldown = 2f;
	private bool isReloading = false;

	protected List<Vector2> recoilPattern = new List<Vector2>();
	private int recoilIndex = 0;

	public Weapon(WeaponType weaponType, Projectile.ProjectileType projectileType, int damage, int clipSize, float rps)
	{
		this.damage = damage;
		this.ammo = clipSize;
		this.clipSize = clipSize;
		this.clipCount = 1;
		this.weaponType = weaponType;
		this.projectileType = projectileType;
		this.rps = rps;
		this.weaponCooldown = 1f / rps;
	}

	public void UpdateCooldownTimer(float delta)
	{
		if (isReloading)
		{
			reloadTimer = Mathf.Clamp(reloadTimer - delta, 0f, reloadCooldown);
			if (reloadTimer <= 0f)
			{
				ammo = clipSize;
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
		return !(isOnCoolDown || ammo < 1 || isReloading);
	}

	public virtual Vector2 Shoot()
	{
		isOnCoolDown = true;
		weaponTimer = weaponCooldown;
		ammo--;
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
