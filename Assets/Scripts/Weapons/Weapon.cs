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
		SINGLE_SHOT
	}

	public Type type { get; private set; }
	public int damage { get; private set; }
	public float rps { get; private set; }
	public int ammo { get; set; }
	public int clipSize { get; private set; }
	public int clipCount { get; set; }
	public int maxClipCount { get; private set; }

	private float cooldownTimer = 0f;
	private float cooldown;

	protected List<Vector2> recoilPattern = new List<Vector2>();
	private int recoilIndex = 0;

	public Weapon(Type type, int damage, int clipsize, float rps)
	{
		this.damage = damage;
		this.ammo = clipsize;
		this.clipSize = clipSize;
		this.clipCount = 1;
		this.type = type;
		this.rps = rps;
		this.cooldown = 1f / rps;
	}

	public void UpdateTimer(float delta)
	{
		cooldownTimer = Mathf.Clamp(cooldownTimer - delta, 0f, 1f / rps);
	}

	public bool OnCooldown()
	{
		return cooldownTimer != 0f;
	}

	public virtual Vector2 Shoot()
	{
		if (!OnCooldown() && recoilIndex < recoilPattern.Count)
		{
			cooldownTimer = cooldown;
			return recoilPattern[recoilIndex++];
		}
		return Vector2.zero;
	}

	public void ResetRecoil()
	{
		cooldownTimer = 0f;
		recoilIndex = 0;
	}
}
