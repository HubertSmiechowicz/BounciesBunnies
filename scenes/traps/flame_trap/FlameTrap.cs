using Godot;
using System;

public partial class FlameTrap : Area2D
{
	[Export]
	[ExportGroup("Timer")]
	private float _Cycle = 2f;


	private Sprite2D _Flame;
	private Timer _Timer;


	private int _Time = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_Flame = GetNode<Sprite2D>("Flame");
		_Timer = GetNode<Timer>("Timer");

		_Timer.Start(_Cycle);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_Time += 1;
		_Flame.Position += new Vector2((float)Math.Sin(_Time) * 2, (float)Math.Sin(_Time) * 2);
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

	public void _OnTimerTimeout()
	{
		if (_Flame.Visible)
		{
			_Flame.Visible = false;
			Monitoring = false;
			SetProcess(false);
		}
		else
		{
			_Flame.Visible = true;
			Monitoring = true;
			SetProcess(true);
		}

		_Timer.Start(_Cycle);
	}
}
