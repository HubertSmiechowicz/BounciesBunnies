using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.TextServer;

namespace BounciesBunnies.scenes.bunny.state_machine
{
	public partial class Swing : State
	{
		[Export]
		[ExportGroup("Properties")]
		public Ground _Ground;

		public override void _OnEnter()
		{
			var direction = Input.GetAxis("move_left", "move_right");
			if (direction == 0)
				direction = 1f;
			_Bunny._Swing(direction);

			_AnimationTree.Set("parameters/conditions/swing", true);
		}

		public override void _OnExit()
		{
			_Bunny._EndSwing();
			_AnimationTree.Set("parameters/conditions/swing", false);
		}
	}
}
