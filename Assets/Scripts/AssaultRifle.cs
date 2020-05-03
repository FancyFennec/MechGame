using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AssaultRifle : Weapon
{

	public AssaultRifle() : base(Type.AUTOMATIC, 10, 30, 10)
	{
		CreateRecoilPattern();
	}

	public AssaultRifle(Type type, int damage, int clipsize, float rpm) : 
		base(type, damage, clipsize, rpm)
	{
		CreateRecoilPattern();
	}

	private void CreateRecoilPattern()
	{
		recoilPattern.Add(new Vector2(1f, 0f));
		recoilPattern.Add(new Vector2(2f, 0.1f));
		recoilPattern.Add(new Vector2(3f, 0.3f));
		recoilPattern.Add(new Vector2(4f, 0.7f));
		recoilPattern.Add(new Vector2(5f, 1.3f));
		recoilPattern.Add(new Vector2(5f, 2f));
		recoilPattern.Add(new Vector2(5f, 2f));
		recoilPattern.Add(new Vector2(5f, 2f));
		recoilPattern.Add(new Vector2(5f, 1.5f));
		recoilPattern.Add(new Vector2(5f, 0.5f));

		recoilPattern.Add(new Vector2(5f, 0f));
		recoilPattern.Add(new Vector2(5f, -1f));
		recoilPattern.Add(new Vector2(5f, -2f));
		recoilPattern.Add(new Vector2(5f, -3f));
		recoilPattern.Add(new Vector2(5f, -3.5f));
		recoilPattern.Add(new Vector2(5f, -3.5f));
		recoilPattern.Add(new Vector2(5f, -3f));
		recoilPattern.Add(new Vector2(5f, -2f));
		recoilPattern.Add(new Vector2(5f, -1.5f));
		recoilPattern.Add(new Vector2(5f, -0.5f));

		recoilPattern.Add(new Vector2(5f, 0f));
		recoilPattern.Add(new Vector2(5f, 1f));
		recoilPattern.Add(new Vector2(5f, 2f));
		recoilPattern.Add(new Vector2(5f, 3f));
		recoilPattern.Add(new Vector2(5f, 3.5f));
		recoilPattern.Add(new Vector2(5f, 3.5f));
		recoilPattern.Add(new Vector2(5f, 3f));
		recoilPattern.Add(new Vector2(5f, 2f));
		recoilPattern.Add(new Vector2(5f, 1.5f));
		recoilPattern.Add(new Vector2(5f, 0.5f));
	}
}
