using BounciesBunnies.scenes.bunny;
using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

public partial class BunnyStateMachine : Node
{
    #region Properties

    [Export]
	[ExportGroup("Properties")]
	public State _CurrentState;

	public State _PreviousState;

	[Export]
	[ExportGroup("Properties")]
	private Bunny _Bunny;

	[Export]
	[ExportGroup("Properties")]
	private AnimationTree _AnimationTree;

	[Export]
	[ExportGroup("Properties")]
	private State _Hurt;

	[Export]
	[ExportGroup("Properties")]
	private State _Swing;

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

				state._Bunny = _Bunny;
				state._AnimationTree = _AnimationTree;
			}
			else
				GD.PushWarning($"Child {child.Name} is not a State for BunnyStateMachine");
		}
	}

	#endregion

	#region _PhysicsProcess()

	public override void _PhysicsProcess(double delta)
	{
		if (_Bunny._IsHurt)
		{
			_SwitchStates(_Hurt);
		}
		else
		{
			if (!_Bunny.IsOnFloor() && _CurrentState is Ground ground)
				_SwitchStates(ground._AirState);

			if (_Bunny._IsSwinging)
				_SwitchStates(_Swing);

			if (_CurrentState._NextState != null)
				_SwitchStates(_CurrentState._NextState);
		}


		_CurrentState._StateProccess(delta);
	}

	#endregion

	#region _Input()

	public override void _Input(InputEvent @event)
	{
		_CurrentState._StateInput(@event);
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
