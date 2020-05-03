﻿using System;
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
	public float rpm { get; private set; }
	public int ammo { get; set; }
	public int clipSize { get; private set; }
	public int clipCount { get; set; }
	public int maxClipCount { get; private set; }

	private float cooldownTimer = 0f;
	private float cooldown;

	public Weapon(Type type, int damage, int clipsize, float rpm)
	{
		this.ammo = clipsize;
		this.clipSize = clipSize;
		this.type = type;
		this.rpm = rpm;
		this.cooldown = 1f / rpm;
	}

	public void UpdateTimer(float delta)
	{
		cooldownTimer = Mathf.Clamp(cooldownTimer - delta, 0f, 1f / rpm);
	}

	public bool OnCooldown()
	{
		return cooldownTimer != 0f;
	}

	public void Shoot()
	{
		cooldownTimer = cooldown;
	}

}
