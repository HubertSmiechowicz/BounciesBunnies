using Godot;
using System;

public partial class FallingSpikes : Path2D
{
	[Export]
	[ExportGroup("Movement")]
	private float _SpeedScale = 1.0f;

	[Export]
	[ExportGroup("Timer")]
	private float _TimeToFall = 2f;

	private int _Time = 1;
	private bool _Triggered = false;

	private Area2D _Damage;
	private Area2D _Trigger;
	private AnimationPlayer _Player;
	private Sprite2D _Sprite;
	private Timer _Timer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_Damage = GetNode<Area2D>("AnimatableBody2D/Damage");
		_Trigger = GetNode<Area2D>("AnimatableBody2D/Trigger");
		_Player = GetNode<AnimationPlayer>("AnimationPlayer");
		_Sprite = GetNode<Sprite2D>("AnimatableBody2D/Sprite2D");
		_Timer = GetNode<Timer>("Timer");

		SetProcess(false);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_Time += 1;
		_Sprite.Position += new Vector2(0, (float)Math.Sin(_Time) * 2);
	}

	public void _OnDamageEntered(Node2D body)
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

	public void _OnTriggerEntered(Node2D body)
	{
		if (body is Bunny bunny && !_Triggered)
		{
			SetProcess(true);
			_Timer.Start(_TimeToFall);
			_Triggered = true;
		}
	}

	public void _OnTimerTimeout()
	{
		if (IsProcessing())
		{
			_Timer.Stop();
			SetProcess(false);
			_Player.Play("fall");
			_Player.SpeedScale = _SpeedScale;
			_Timer.Start(1);
		}
		else
		{
			QueueFree();
		}
	}
}
