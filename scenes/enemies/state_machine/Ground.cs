using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BounciesBunnies.scenes.enemies.state_machine
{
	public partial class Ground : State
	{
        public override void _OnEnter()
        {
            _AnimationTree.Set("parameters/conditions/move", true);
        }

        public override void _OnExit()
        {
            _AnimationTree.Set("parameters/conditions/move", false);
        }
    }
}
