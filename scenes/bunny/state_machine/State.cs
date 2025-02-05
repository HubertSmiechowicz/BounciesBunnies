using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BounciesBunnies.scenes.bunny
{
    public partial class State : Node
    {
        [Export]
        [ExportGroup("Properties")]
        public bool _CanMove;

        public Bunny _Bunny;
        public AnimationTree _AnimationTree;
        public State _NextState;

        public virtual void _StateProccess(double delta)
        {
        }

        internal virtual void _StateInput(InputEvent @event)
        {
        }

        public virtual void _OnEnter()
        {
        }

        public virtual void _OnExit()
        {
        }
    }
}
