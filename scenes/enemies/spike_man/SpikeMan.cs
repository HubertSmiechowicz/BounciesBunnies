using BounciesBunnies.scenes.enemies;
using BounciesBunnies.scenes.enemies.state_machine;
using Godot;
using System;

public partial class SpikeMan : Enemy
{
	public override void _Ready()
	{
		_AnimationTree = GetNode<AnimationTree>("AnimationTree");
		_Timer = GetNode<Timer>("Timer");
		_EnemyStateMachine = GetNode<EnemyStateMachine>("EnemyStateMachine");
		base._Ready();
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
	}

	public void _OnBunnyEntered(Node2D body)
	{
		if (body is Bunny bunny)
		{
			var direction = 1f;

			if (bunny.Position.X > Position.X)
				direction = 1f;
			else
				direction = -1f;

			bunny._Hurt(direction);
		}
	}
}
