using Godot;
using System;

public partial class Portal : Area2D
{
	[Export]
	[ExportGroup("Properties")]
	private Node2D _Target;

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
		if (body is Bunny bunny && _Target != null)
		{
			if (bunny.Position.X > Position.X)
				bunny.Position = new Vector2(_Target.Position.X - 100, _Target.Position.Y);

			else
				bunny.Position = new Vector2(_Target.Position.X + 100, _Target.Position.Y);
		}
	}
}
