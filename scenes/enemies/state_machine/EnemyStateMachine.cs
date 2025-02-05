using BounciesBunnies.scenes.bunny.state_machine;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BounciesBunnies.scenes.enemies.state_machine
{
	public partial class EnemyStateMachine : Node
	{
        #region Properties

        [Export]
		[ExportGroup("Properties")]
		public State _CurrentState;

		public State _PreviousState;

		[Export]
		[ExportGroup("Properties")]
		private Enemy _Enemy;

		[Export]
		[ExportGroup("Properties")]
		private AnimationTree _AnimationTree;

		[Export]
		[ExportGroup("Properties")]
		private State _Hurt;

        [Export]
        [ExportGroup("Properties")]
        private State _Attack;

        private List<State> _States = new List<State>();

        #endregion

        #region _Ready()

        public override void _Ready()
		{
			foreach (var child in GetChildren())
			{
				if (child is State state)
				{
					_States.Add(state);

					state._Enemy = _Enemy;
					state._AnimationTree = _AnimationTree;
				}
				else
					GD.PushWarning($"Child {child.Name} is not a State for EnemyStateMachine");
			}
		}

		#endregion

		#region _PhysicsProcess()

		public override void _PhysicsProcess(double delta)
		{
			if (_Enemy._IsHurt)
			{
				_SwitchStates(_Hurt);
			}
			else
			{
				if (_CurrentState._NextState != null)
					_SwitchStates(_CurrentState._NextState);

                if (_Enemy._IsAttacking)
                    _SwitchStates(_Attack);
            }

			_CurrentState._StateProccess(delta);
		}

        #endregion

        #region _CanMove()

        public bool _CanMove()
		{
			return _CurrentState._CanMove;
		}

		#endregion

		#region _SwitchStates()

		public void _SwitchStates(State newState)
		{
			if (_CurrentState != newState)
			{
				if (_CurrentState != null)
				{
					_CurrentState._OnExit();
					_CurrentState._NextState = null;
				}

				_CurrentState = newState;
				_CurrentState._OnEnter();
			}
		}

        #endregion
    }
}
