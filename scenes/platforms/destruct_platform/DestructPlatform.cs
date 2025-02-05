using Godot;
using System;

public partial class DestructPlatform : StaticBody2D
{
    [Export]
    [ExportGroup("Properties")]
    public Texture2D _SpriteTexture;

    [Export]
    [ExportGroup("Properties")]
    public Texture2D _ParticlesSpriteTexture;

    private int _Time = 1;

	private Sprite2D _Sprite;
	private Timer _Timer;
	private GpuParticles2D _Particles;
	private CollisionShape2D _CollisionShape2D;
	private Area2D _Area;	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_Sprite = GetNode<Sprite2D>("Sprite2D");
		_Timer = GetNode<Timer>("Timer");
		_Particles = GetNode<GpuParticles2D>("GPUParticles2D");
		_CollisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
		_Area = GetNode<Area2D>("Area2D");

        if (_SpriteTexture != null && _Sprite != null)
        {
            _Sprite.Texture = _SpriteTexture;
        }

        if (_ParticlesSpriteTexture != null && _Particles != null)
        {
            _Particles.Texture = _ParticlesSpriteTexture;
        }

        SetProcess(false);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_Time += 1;
		_Sprite.Position += new Vector2(0, (float)Math.Sin(_Time) * 2);
	}

	public void _OnBodyEntered(Node2D body)
	{
		if (body is Bunny)
		{
			SetProcess(true);
			_Timer.Start(0.7);
		}
	}

	public void _OnTimerTimeout()
	{
		if (IsProcessing())
		{
			SetProcess(false);
			_Particles.Emitting = true;
			_Area.QueueFree();
			_CollisionShape2D.QueueFree();
			_Sprite.QueueFree();
			_Timer.Start(0.8);
		}
		else
		{
			QueueFree();
		}
	}
}
