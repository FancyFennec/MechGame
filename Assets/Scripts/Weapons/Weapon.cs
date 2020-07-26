using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Weapon
{
	public enum Type
	{
		SEMI_AUTOMATIC,
		AUTOMATIC,
		ROCKET
	}

	public Type type { get; private set; }
	public int damage { get; private set; }
	public float rps { get; private set; }
	public int ammo { get; set; }
	public int clipSize { get;  set; }
	public int clipCount { get; set; }
	public int maxClipCount { get; private set; }

	private float cooldownTimer = 0f;
	private float cooldown;

	protected List<Vector2> recoilPattern = new List<Vector2>();
	private int recoilIndex = 0;

	public Weapon(Type type, int damage, int clipSize, float rps)
	{
		this.damage = damage;
		this.ammo = clipSize;
		this.clipSize = clipSize;
		this.clipCount = 1;
		this.type = type;
		this.rps = rps;
		this.cooldown = 1f / rps;
	}

	public void UpdateCooldownTimer(float delta)
	{
		cooldownTimer = Mathf.Clamp(cooldownTimer - delta, 0f, 1f / rps);
	}

	public bool OnCooldown()
	{
		return cooldownTimer != 0f || ammo < 1;
	}

	public virtual Vector2 Shoot()
	{
		if (!OnCooldown() && recoilIndex < recoilPattern.Count && ammo > 0)
		{
			cooldownTimer = cooldown;
			ammo--;
			return recoilPattern[recoilIndex++];
		}
		return Vector2.zero;
	}

	public void ResetRecoil()
	{
		cooldownTimer = 0f;
		recoilIndex = 0;
	}

	public void Reload()
	{
		ammo = clipSize;
	}
}
