using BounciesBunnies.scenes.bunny.state_machine;
using BounciesBunnies.scenes.enemies;
using Godot;
using System;
using static Godot.TextServer;

public partial class Bunny : CharacterBody2D
{

	#region _Properties

	private AnimationTree _AnimationTree;
	private StaticPlatform _Platform;
	private AnimationPlayer _Player;
	private Timer _Timer;
	private Node2D _HandEquip;
	private CollisionShape2D _HandEquipCollisionShape;
	private BunnyStateMachine _BunnyStateMachine;

	[Export]
	[ExportGroup("Movement")]
	private float _Speed = 400.0f;

	[Export]
	[ExportGroup("Movement")]
	public float _JumpImpulse = 400.0f;

	[Export]
	[ExportGroup("Movement")]
	private float _Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	[Export]
	[ExportGroup("Statistics")]
	private int _Lifes = 3;

	[Export]
	[ExportGroup("Bunny Animations")]
	private float _HurtTime = 1f;

	[Export]
	[ExportGroup("Bunny Animations")]
	public bool _IsHurt = false;

	[Export]
	[ExportGroup("Bunny Animations")]
	private float _HurtImpulse = 50f;

	[Export]
	[ExportGroup("Bunny Animations")]
	public bool _IsSwinging = false;

	[Export]
	[ExportGroup("Bunny Animations")]
	private float _SwingingTime = 1f;

	#endregion

	#region _Ready()

	public override void _Ready()
	{
		_AnimationTree = GetNode<AnimationTree>("AnimationTree");
		_Player = GetNode<AnimationPlayer>("AnimationPlayer");
		_Timer = GetNode<Timer>("Timer");
		_HandEquip = GetNode<Node2D>("HandEquip");
		_HandEquipCollisionShape = GetNode<CollisionShape2D>("HandEquip/EquipSprite/Area2D/CollisionShape2D");
		_BunnyStateMachine = GetNode<BunnyStateMachine>("BunnyStateMachine");

		_HandEquipCollisionShape.Disabled = true;
		_HandEquip.Visible = false;

		_AnimationTree.Active = true;
		_AnimationTree.Set("parameters/conditions/move", true);
	}

	#endregion

	#region _PhysicsProcess()

	public override void _PhysicsProcess(double delta)
	{
		_Move(delta);

		MoveAndSlide();
	}

	#endregion

	#region _Move()

	private void _Move(double delta)
	{
		Vector2 velocity = Velocity;
		var direction = 0f;

		if (!_IsHurt)
		{
			direction = Input.GetAxis("move_left", "move_right");

			if (direction != 0f && _BunnyStateMachine._CanMove())
				velocity.X = direction * _Speed;
			else
				velocity.X = 0;

			velocity.Y += _Gravity * (float)delta;


			_AnimationTree.Set("parameters/Move/blend_position", direction);
		}
		else
		{
			velocity.Y += _Gravity * (float)delta;
		}

		Velocity = velocity;
	}

	#endregion

	#region _OnBunnyEnterPlatform()

	public void _OnBunnyEnterPlatform(StaticPlatform platform)
	{
		GetParent().CallDeferred("remove_child", this);
		platform.CallDeferred("add_child", this);
	}

	#endregion

	#region _OnBunnyExitPlatform()

	public void _OnBunnyExitPlatform(StaticPlatform platform)
	{
		_Platform = null;
	}

	#endregion

	#region _OnTimerTimeout()

	public void _OnTimerTimeout()
	{
		if (_IsHurt)
		{
			_IsHurt = false;
			if (_BunnyStateMachine._PreviousState is Swing swing)
				_BunnyStateMachine._SwitchStates(swing._Ground);
			else
				_BunnyStateMachine._SwitchStates(_BunnyStateMachine._PreviousState);
			_BunnyStateMachine._PreviousState = null;
		}
		else if (_IsSwinging)
		{
			_IsSwinging = false;
			if (_BunnyStateMachine._CurrentState is Swing swing)
				swing._NextState = swing._Ground;
		}
	}

	#endregion

	#region _Hurt()

	public void _Hurt(float direction)
	{
		if (_Lifes > 0)
		{
			_Lifes -= 1;
			_BunnyStateMachine._PreviousState = _BunnyStateMachine._CurrentState;
			_IsSwinging = false;
			_IsHurt = true;
			Velocity = new Vector2(direction * _HurtImpulse, -_HurtImpulse);
			_Timer.Start(_HurtTime);
		}
	}

	#endregion

	#region _Swing()

	public void _Swing(float direction)
	{
		_HandEquipCollisionShape.Disabled = false;
		_HandEquip.Visible = true;

		_AnimationTree.Set("parameters/Swing/blend_position", direction);
		_Timer.Start(_SwingingTime);
	}

	#endregion

	#region _EndSwing()

	public void _EndSwing()
	{
		_HandEquipCollisionShape.Disabled = true;
		_HandEquip.Visible = false;
	}

	#endregion

	#region _OnEnemyBodyEntered()

	public void _OnEnemyBodyEntered(Node2D body)
	{
		if (body is Enemy enemy)
		{
			var direction = 0f;
			if (enemy.Position.X < Position.X)
				direction = -1f;
			else
				direction = 1f;

			enemy._Hurt(direction);
		}
	}

	#endregion
}
