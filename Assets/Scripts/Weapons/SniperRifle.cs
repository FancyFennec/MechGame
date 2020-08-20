using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SniperRifle : Weapon
{
	public SniperRifle() : base(
		WeaponType.SEMI_AUTOMATIC, 
		"Projectiles/SniperBullet",
		"Audio/Sniper_Rifle",
		10, 3)
	{
		CreateRecoilPattern();
	}

	private void CreateRecoilPattern()
	{
		recoilPattern.Add(new Vector2(10f, 0f));
		recoilPattern.Add(new Vector2(10f, 2f));
		recoilPattern.Add(new Vector2(10f, 4f));
		recoilPattern.Add(new Vector2(10f, 8f));
		recoilPattern.Add(new Vector2(10f, 10f));
		recoilPattern.Add(new Vector2(10f, 0f));
		recoilPattern.Add(new Vector2(10f, -10));
		recoilPattern.Add(new Vector2(10f, -10f));
		recoilPattern.Add(new Vector2(10f, -15f));
		recoilPattern.Add(new Vector2(10f, -15f));
	}
}
