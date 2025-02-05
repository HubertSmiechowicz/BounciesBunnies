using Godot;
using System;

public partial class Spring : StaticBody2D
{
	private AnimationPlayer _Player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_Player = GetNode<AnimationPlayer>("Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _OnBodyEntered(Node2D body)
	{
		if (body is Bunny bunny)
		{
			_Player.Play("release");
			bunny.Velocity = new Vector2(bunny.Velocity.X, 2 * -bunny._JumpImpulse);
		}
	}
}
