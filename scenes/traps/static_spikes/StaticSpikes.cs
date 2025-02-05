using Godot;
using System;

public partial class StaticSpikes : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _OnBodyEntered(Node2D body)
	{
		if (body is Bunny bunny)
		{
			var direction = 0f;
			if (bunny.Position.X > Position.X)
				direction = 1f;
			else
				direction = -1f;
			bunny._Hurt(direction);
		}
	}
}
