#if CHRONOS_PLAYMAKER

using HutongGames.PlayMaker;

namespace Chronos.PlayMaker
{
	[ActionCategory("Physics 2d (Chronos)")]
	[Tooltip("Tests if a Game Object's Rigid Body 2D is Kinematic.")]
	public class IsKinematic2d : ChronosComponentAction<Timeline>
	{
		[RequiredField]
		[CheckForComponent(typeof(Timeline))]
		public FsmOwnerDefault gameObject;

		public FsmEvent trueEvent;

		public FsmEvent falseEvent;

		[UIHint(UIHint.Variable)]
		public FsmBool store;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			trueEvent = null;
			falseEvent = null;
			store = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoIsKinematic();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoIsKinematic();
		}

		private void DoIsKinematic()
		{
			if (!UpdateCache(Fsm.GetOwnerDefaultTarget(gameObject))) return;

			var isKinematic = timeline.rigidbody2D.isKinematic;
			store.Value = isKinematic;

			Fsm.Event(isKinematic ? trueEvent : falseEvent);
		}
	}
}

#endif
