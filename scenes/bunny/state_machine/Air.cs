using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BounciesBunnies.scenes.bunny
{
	public partial class Air : State
	{
		[Export]
		[ExportGroup("Properties")]
		private State _GroundState;

		public override void _StateProccess(double delta)
		{
			if (_Bunny.IsOnFloor())
			{
				_NextState = _GroundState;
                _AnimationTree.Set("parameters/conditions/is_jump", false);
                _AnimationTree.Set("parameters/conditions/move", true);
            }
		}

        public override void _OnEnter()
        {
            _AnimationTree.Set("parameters/conditions/is_jump", true);
        }

        public override void _OnExit()
        {
            _AnimationTree.Set("parameters/conditions/is_jump", false);
        }
    }
}
