using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using UnityEngine;

public class RocketLauncher : Weapon
{

	public RocketLauncher() : base(Type.SINGLE_SHOT, 200, 1, 1)
	{
		CreateRecoilPattern();
	}

	private void CreateRecoilPattern()
	{
		recoilPattern.Add(new Vector2(10f, 0f));
	}
}
