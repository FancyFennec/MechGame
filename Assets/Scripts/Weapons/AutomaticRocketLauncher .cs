using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using UnityEngine;

public class AutomaticRocketLauncher : Weapon
{
	public AutomaticRocketLauncher() : base(
		WeaponType.AUTOMATIC, 
		"Projectiles/Rocket",
		"Audio/Rocket_Shot",
		30, 7)
	{
		CreateRecoilPattern();
	}

	private void CreateRecoilPattern()
	{
		recoilPattern.Add(new Vector2(3f, 0f));
		recoilPattern.Add(new Vector2(4f, 0.1f));
		recoilPattern.Add(new Vector2(4f, 0.3f));
		recoilPattern.Add(new Vector2(4f, 0.4f));
		recoilPattern.Add(new Vector2(4f, 0.5f));
		recoilPattern.Add(new Vector2(4f, 0.6f));
		recoilPattern.Add(new Vector2(4f, 0.7f));
		recoilPattern.Add(new Vector2(4f, 0.6f));
		recoilPattern.Add(new Vector2(4f, 0.5f));
		recoilPattern.Add(new Vector2(4f, 0.5f));

		recoilPattern.Add(new Vector2(4f, 0f));
		recoilPattern.Add(new Vector2(4f, -0.1f));
		recoilPattern.Add(new Vector2(4f, -0.3f));
		recoilPattern.Add(new Vector2(4f, -0.5f));
		recoilPattern.Add(new Vector2(4f, -0.5f));
		recoilPattern.Add(new Vector2(4f, -0.5f));
		recoilPattern.Add(new Vector2(4f, -0.5f));
		recoilPattern.Add(new Vector2(4f, -0.5f));
		recoilPattern.Add(new Vector2(4f, -0.3f));
		recoilPattern.Add(new Vector2(4f, -0.1f));

		recoilPattern.Add(new Vector2(3f, 0.1f));
		recoilPattern.Add(new Vector2(3f, 0.3f));
		recoilPattern.Add(new Vector2(3f, 0.5f));
		recoilPattern.Add(new Vector2(3f, 0.5f));
		recoilPattern.Add(new Vector2(3f, 0.5f));
		recoilPattern.Add(new Vector2(3f, 0.5f));
		recoilPattern.Add(new Vector2(2f, 0.5f));
		recoilPattern.Add(new Vector2(2f, 0.5f));
		recoilPattern.Add(new Vector2(2f, 0.3f));
		recoilPattern.Add(new Vector2(2f, 0.1f));
	}
}
