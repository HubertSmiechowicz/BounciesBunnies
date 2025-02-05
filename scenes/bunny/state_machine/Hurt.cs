using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BounciesBunnies.scenes.bunny.state_machine
{
	public partial class Hurt : State
	{
		public override void _OnEnter()
		{
			_AnimationTree.Set("parameters/conditions/is_hurt", true);
		}

		public override void _OnExit()
		{
			_AnimationTree.Set("parameters/conditions/is_hurt", false);
		}
	}
}
