using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AssaultRifle : Weapon
{
	public AssaultRifle() : base(WeaponType.AUTOMATIC, "Projectiles/Bullet", 10, 90, 20)
	{
		CreateRecoilPattern();
	}

	private void CreateRecoilPattern()
	{
		recoilPattern.Add(new Vector2(4f, 0f));
		recoilPattern.Add(new Vector2(5f, 0.1f));
		recoilPattern.Add(new Vector2(3f, 0.2f));
		recoilPattern.Add(new Vector2(3f, 0.3f));
		recoilPattern.Add(new Vector2(2.8f, 0.7f));
		recoilPattern.Add(new Vector2(2.5f, 1f));
		recoilPattern.Add(new Vector2(2.3f, 1f));
		recoilPattern.Add(new Vector2(2.2f, 1f));
		recoilPattern.Add(new Vector2(2.1f, 1.2f));
		recoilPattern.Add(new Vector2(2f, 1.3f));

		recoilPattern.Add(new Vector2(2f, 1.5f));
		recoilPattern.Add(new Vector2(2f, 1.5f));
		recoilPattern.Add(new Vector2(2f, 1.7f));
		recoilPattern.Add(new Vector2(2f, 1.7f));
		recoilPattern.Add(new Vector2(2f, 1f));
		recoilPattern.Add(new Vector2(1.8f, 1f));
		recoilPattern.Add(new Vector2(1.6f, 1.5f));
		recoilPattern.Add(new Vector2(1.4f, 1f));
		recoilPattern.Add(new Vector2(1.2f, 1.7f));
		recoilPattern.Add(new Vector2(1.2f, 1.3f));

		recoilPattern.Add(new Vector2(1f, 1f));
		recoilPattern.Add(new Vector2(1f, 1.5f));
		recoilPattern.Add(new Vector2(1f, 1f));
		recoilPattern.Add(new Vector2(1f, 1.5f));
		recoilPattern.Add(new Vector2(1f, 1f));
		recoilPattern.Add(new Vector2(1f, 1f));
		recoilPattern.Add(new Vector2(1f, 1.5f));
		recoilPattern.Add(new Vector2(1f, 1f));
		recoilPattern.Add(new Vector2(1f, 0.7f));
		recoilPattern.Add(new Vector2(1f, 0.3f));

		recoilPattern.Add(new Vector2(1f, 0f));
		recoilPattern.Add(new Vector2(1f, -0.5f));
		recoilPattern.Add(new Vector2(1f, -1f));
		recoilPattern.Add(new Vector2(1f, -1.5f));
		recoilPattern.Add(new Vector2(1f, -1f));
		recoilPattern.Add(new Vector2(1f, -1f));
		recoilPattern.Add(new Vector2(1f, -1.5f));
		recoilPattern.Add(new Vector2(1f, -1f));
		recoilPattern.Add(new Vector2(1f, -1.7f));
		recoilPattern.Add(new Vector2(1f, -1.3f));

		recoilPattern.Add(new Vector2(1f, -1f));
		recoilPattern.Add(new Vector2(1f, -1f));
		recoilPattern.Add(new Vector2(1f, -1.5f));
		recoilPattern.Add(new Vector2(1f, -1f));
		recoilPattern.Add(new Vector2(1f, -1.7f));
		recoilPattern.Add(new Vector2(1f, -1.3f));
		recoilPattern.Add(new Vector2(1f, -1.5f));
		recoilPattern.Add(new Vector2(1f, -1f));
		recoilPattern.Add(new Vector2(1f, -1.7f));
		recoilPattern.Add(new Vector2(1f, -1.3f));

		recoilPattern.Add(new Vector2(1f, -0f));
		recoilPattern.Add(new Vector2(1f, -0.5f));
		recoilPattern.Add(new Vector2(1f, -1f));
		recoilPattern.Add(new Vector2(1f, -1.5f));
		recoilPattern.Add(new Vector2(1f, -1f));
		recoilPattern.Add(new Vector2(1f, -1f));
		recoilPattern.Add(new Vector2(1f, -1.5f));
		recoilPattern.Add(new Vector2(1f, -1f));
		recoilPattern.Add(new Vector2(1f, -0.7f));
		recoilPattern.Add(new Vector2(1f, -0.3f));

		recoilPattern.Add(new Vector2(1f, 0f));
		recoilPattern.Add(new Vector2(1f, -0.5f));
		recoilPattern.Add(new Vector2(1f, -1f));
		recoilPattern.Add(new Vector2(1f, -1.5f));
		recoilPattern.Add(new Vector2(1f, -1f));
		recoilPattern.Add(new Vector2(1f, -1f));
		recoilPattern.Add(new Vector2(1f, -1.5f));
		recoilPattern.Add(new Vector2(1f, -1f));
		recoilPattern.Add(new Vector2(1f, -0.7f));
		recoilPattern.Add(new Vector2(1f, -0.3f));

		recoilPattern.Add(new Vector2(1f, 0f));
		recoilPattern.Add(new Vector2(1f, 0.5f));
		recoilPattern.Add(new Vector2(1f, 1f));
		recoilPattern.Add(new Vector2(1f, 1.5f));
		recoilPattern.Add(new Vector2(1f, 1f));
		recoilPattern.Add(new Vector2(1f, 1f));
		recoilPattern.Add(new Vector2(1f, 1.5f));
		recoilPattern.Add(new Vector2(1f, 1f));
		recoilPattern.Add(new Vector2(1f, 0.7f));
		recoilPattern.Add(new Vector2(1f, 0.3f));

		recoilPattern.Add(new Vector2(1f, 0f));
		recoilPattern.Add(new Vector2(1f, 0.5f));
		recoilPattern.Add(new Vector2(1f, 1f));
		recoilPattern.Add(new Vector2(1f, 1.5f));
		recoilPattern.Add(new Vector2(1f, 1f));
		recoilPattern.Add(new Vector2(1f, 1f));
		recoilPattern.Add(new Vector2(1f, 1.5f));
		recoilPattern.Add(new Vector2(1f, 1f));
		recoilPattern.Add(new Vector2(1f, 0.7f));
		recoilPattern.Add(new Vector2(1f, 0.3f));
	}
}
