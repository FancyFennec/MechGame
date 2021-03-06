﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using UnityEngine;

public class GrenadeLauncher : Weapon
{
	public GrenadeLauncher() : base(
		WeaponType.AUTOMATIC, 
		"Projectiles/Grenade",
		"Audio/Rocket_Shot",
		20, 5)
	{
		CreateRecoilPattern();
	}

	private void CreateRecoilPattern()
	{
		recoilPattern.Add(new Vector2(4f, 0f));
		recoilPattern.Add(new Vector2(5f, 0.2f));
		recoilPattern.Add(new Vector2(6f, 0.5f));
		recoilPattern.Add(new Vector2(5f, 0.8f));
		recoilPattern.Add(new Vector2(6f, 1.5f));
		recoilPattern.Add(new Vector2(6f, 2.2f));
		recoilPattern.Add(new Vector2(6f, 2.3f));
		recoilPattern.Add(new Vector2(6f, 2f));
		recoilPattern.Add(new Vector2(6f, 1.5f));
		recoilPattern.Add(new Vector2(6f, 0.5f));

		recoilPattern.Add(new Vector2(5f, 0f));
		recoilPattern.Add(new Vector2(5f, -1f));
		recoilPattern.Add(new Vector2(5f, -2f));
		recoilPattern.Add(new Vector2(5f, -3f));
		recoilPattern.Add(new Vector2(5f, -3.5f));
		recoilPattern.Add(new Vector2(5f, -3.5f));
		recoilPattern.Add(new Vector2(4f, -3f));
		recoilPattern.Add(new Vector2(4f, -2f));
		recoilPattern.Add(new Vector2(4f, -1.5f));
		recoilPattern.Add(new Vector2(4f, -0.5f));

		recoilPattern.Add(new Vector2(3f, 0f));
		recoilPattern.Add(new Vector2(3f, 1f));
		recoilPattern.Add(new Vector2(3f, 2f));
		recoilPattern.Add(new Vector2(3f, 3f));
		recoilPattern.Add(new Vector2(3f, 3.5f));
		recoilPattern.Add(new Vector2(3f, 3.5f));
		recoilPattern.Add(new Vector2(2f, 3f));
		recoilPattern.Add(new Vector2(2f, 2f));
		recoilPattern.Add(new Vector2(2f, 1.5f));
		recoilPattern.Add(new Vector2(2f, 0.5f));
	}
}
