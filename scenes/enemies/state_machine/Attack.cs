using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BounciesBunnies.scenes.enemies.state_machine
{
	public partial class Attack : State
	{
		[Export]
		[ExportGroup("Properties")]
		public State _NextAttackState;


		public override void _OnEnter()
		{
			var direction = 1f;
			if (_Enemy._Bunny.Position.X > _Enemy.Position.X)
				direction = 1f;
			else
				direction = -1f;
			_Enemy._Attack(direction);

			_AnimationTree.Set("parameters/conditions/attack", true);
		}

		public override void _OnExit()
		{
			_AnimationTree.Set("parameters/conditions/attack", false);
		}
	}
}
