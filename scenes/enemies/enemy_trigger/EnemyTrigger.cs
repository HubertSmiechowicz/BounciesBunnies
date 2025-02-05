using BounciesBunnies.scenes.enemies;
using Godot;
using System;

public partial class EnemyTrigger : Area2D
{
	[Export]
	[ExportGroup("Enemy")]
	private Enemy _Enemy;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _OnBunnyEntered(Node2D body)
	{
		if (body is Bunny bunny && _Enemy != null && !_Enemy._IsTriggered)
			_Enemy._EnemyTriggered(bunny);
	}
}
