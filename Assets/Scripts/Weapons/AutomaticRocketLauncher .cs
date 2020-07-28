using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using UnityEngine;

public class AutomaticRocketLauncher : Weapon
{

	public AutomaticRocketLauncher() : base(WeaponType.AUTOMATIC, ProjectileType.EXPLOSIVE, 50, 30, 7)
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
		recoilPattern.Add(new Vector2(6f, 22.3f));
		recoilPattern.Add(new Vector2(6f, 2f));
		recoilPattern.Add(new Vector2(6f, 1.5f));
		recoilPattern.Add(new Vector2(6f, 0.5f));

		recoilPattern.Add(new Vector2(6f, 0f));
		recoilPattern.Add(new Vector2(6f, -1f));
		recoilPattern.Add(new Vector2(6f, -2f));
		recoilPattern.Add(new Vector2(6f, -3f));
		recoilPattern.Add(new Vector2(6f, -3.5f));
		recoilPattern.Add(new Vector2(6f, -3.5f));
		recoilPattern.Add(new Vector2(6f, -3f));
		recoilPattern.Add(new Vector2(6f, -2f));
		recoilPattern.Add(new Vector2(6f, -1.5f));
		recoilPattern.Add(new Vector2(6f, -0.5f));

		recoilPattern.Add(new Vector2(6f, 0f));
		recoilPattern.Add(new Vector2(6f, 1f));
		recoilPattern.Add(new Vector2(6f, 2f));
		recoilPattern.Add(new Vector2(6f, 3f));
		recoilPattern.Add(new Vector2(6f, 3.5f));
		recoilPattern.Add(new Vector2(6f, 3.5f));
		recoilPattern.Add(new Vector2(6f, 3f));
		recoilPattern.Add(new Vector2(6f, 2f));
		recoilPattern.Add(new Vector2(6f, 1.5f));
		recoilPattern.Add(new Vector2(6f, 0.5f));
	}
}
