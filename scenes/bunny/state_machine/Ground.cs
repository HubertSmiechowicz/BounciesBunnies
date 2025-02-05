using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BounciesBunnies.scenes.bunny
{
	public partial class Ground : State
	{
		[Export]
		[ExportGroup("Properties")]
		public Air _AirState;

		internal override void _StateInput(InputEvent @event)
		{
			if (@event.IsActionPressed("jump"))
				Jump();

			if (@event.IsActionPressed("swing")) 
				Swing();
		}

		public override void _OnEnter()
		{
			_AnimationTree.Set("parameters/conditions/move", true);
		}

		public override void _OnExit()
		{
			_AnimationTree.Set("parameters/conditions/move", false);
		}

		private void Jump()
		{
			_Bunny.Velocity = new Vector2(_Bunny.Velocity.X, -_Bunny._JumpImpulse);
			_NextState = _AirState;
		}

		private void Swing()
		{
			_Bunny._IsSwinging = true;
		}
	}
}
