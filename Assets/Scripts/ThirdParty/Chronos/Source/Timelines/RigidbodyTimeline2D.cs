using UnityEngine;

namespace Chronos
{
	public class RigidbodyTimeline2D : RigidbodyTimeline<Rigidbody2D, RigidbodyTimeline2D.Snapshot>
	{
		// Scale is disabled by default because it usually doesn't change and 
		// would otherwise take more memory. Feel free to uncomment the lines
		// below if you need to record it.

		public struct Snapshot
		{
			public Vector3 position;
			public Quaternion rotation;
			// public Vector3 scale;
			public Vector2 velocity;
			public float angularVelocity;
			public float lastPositiveTimeScale;

			public static Snapshot Lerp(Snapshot from, Snapshot to, float t)
			{
				return new Snapshot()
				{
					position = Vector3.Lerp(from.position, to.position, t),
					rotation = Quaternion.Lerp(from.rotation, to.rotation, t),
					// scale = Vector3.Lerp(from.scale, to.scale, t),
					velocity = Vector2.Lerp(from.velocity, to.velocity, t),
					angularVelocity = Mathf.Lerp(from.angularVelocity, to.angularVelocity, t),
					lastPositiveTimeScale = Mathf.Lerp(from.lastPositiveTimeScale, to.lastPositiveTimeScale, t),
				};
			}
		}

		public RigidbodyTimeline2D(Timeline timeline, Rigidbody2D component) : base(timeline, component) { }

		public override void CopyProperties(Rigidbody2D source)
		{
			isKinematic = source.isKinematic;
			gravityScale = source.gravityScale;
			source.gravityScale = 0;
		}

		public override void FixedUpdate()
		{
			if (gravityScale > 0 && !component.isKinematic && timeline.timeScale > 0)
			{
				velocity += (Physics2D.gravity * gravityScale * timeline.fixedDeltaTime);
			}
		}

		#region Snapshots

		protected override Snapshot LerpSnapshots(Snapshot from, Snapshot to, float t)
		{
			return Snapshot.Lerp(from, to, t);
		}

		protected override Snapshot CopySnapshot()
		{
			return new Snapshot()
			{
				position = component.transform.position,
				rotation = component.transform.rotation,
				// scale = component.transform.localScale,
				velocity = component.velocity,
				angularVelocity = component.angularVelocity,
				lastPositiveTimeScale = lastPositiveTimeScale
			};
		}

		protected override void ApplySnapshot(Snapshot snapshot)
		{
			component.transform.position = snapshot.position;
			component.transform.rotation = snapshot.rotation;
			// component.transform.localScale = snapshot.scale;

			if (timeline.timeScale > 0)
			{
				component.velocity = snapshot.velocity;
				component.angularVelocity = snapshot.angularVelocity;
			}

			lastPositiveTimeScale = snapshot.lastPositiveTimeScale;
		}

		#endregion

		#region Components

		protected float bodyGravityScale
		{
			get { return component.gravityScale; }
			set { component.gravityScale = value; }
		}

		protected override bool bodyIsKinematic
		{
			get { return component.isKinematic; }
			set { component.isKinematic = value; }
		}

		protected override float bodyMass
		{
			get { return component.mass; }
			set { component.mass = value; }
		}

		protected override Vector3 bodyVelocity
		{
			get { return component.velocity; }
			set { component.velocity = value; }
		}

		protected override Vector3 bodyAngularVelocity
		{
			get { return component.angularVelocity * Vector3.one; }
			set { component.angularVelocity = value.x; }
		}

		protected override float bodyDrag
		{
			get { return component.drag; }
			set { component.drag = value; }
		}

		protected override float bodyAngularDrag
		{
			get { return component.angularDrag; }
			set { component.angularDrag = value; }
		}

		protected override bool IsSleeping()
		{
			return component.IsSleeping();
		}

		protected override void WakeUp()
		{
			component.WakeUp();
		}

		/// <summary>
		/// The gravity scale of the rigidbody. Use this property instead of Rigidbody2D.gravityScale, which will be overwritten by the physics timer at runtime.
		/// </summary>
		public float gravityScale { get; set; }

		/// <summary>
		/// The velocity of the rigidbody before time effects. Use this property instead of Rigidbody2D.velocity, which will be overwritten by the physics timer at runtime. 
		/// </summary>
		public Vector2 velocity
		{
			get { return bodyVelocity / timeline.timeScale; }
			set { if (AssertForwardProperty("velocity", Severity.Ignore)) bodyVelocity = value * timeline.timeScale; }
		}

		/// <summary>
		/// The angular velocity of the rigidbody before time effects. Use this property instead of Rigidbody2D.angularVelocity, which will be overwritten by the physics timer at runtime. 
		/// </summary>
		public float angularVelocity
		{
			get { return bodyAngularVelocity.x / timeline.timeScale; }
			set
			{
				if (AssertForwardProperty("angularVelocity", Severity.Ignore))
					bodyAngularVelocity = value * Vector3.one * timeline.timeScale;
			}
		}

		/// <summary>
		/// The equivalent of Rigidbody2D.AddForce adjusted for time effects.
		/// </summary>
		public void AddForce(Vector2 force, ForceMode2D mode = ForceMode2D.Force)
		{
			if (AssertForwardForce(Severity.Ignore))
				component.AddForce(AdjustForce(force), mode);
		}

		/// <summary>
		/// The equivalent of Rigidbody2D.AddRelativeForce adjusted for time effects.
		/// </summary>
		public void AddRelativeForce(Vector2 force, ForceMode2D mode = ForceMode2D.Force)
		{
			if (AssertForwardForce(Severity.Ignore))
				component.AddRelativeForce(AdjustForce(force), mode);
		}

		/// <summary>
		/// The equivalent of Rigidbody2D.AddForceAtPosition adjusted for time effects.
		/// </summary>
		public void AddForceAtPosition(Vector2 force, Vector2 position, ForceMode2D mode = ForceMode2D.Force)
		{
			if (AssertForwardForce(Severity.Ignore))
				component.AddForceAtPosition(AdjustForce(force), position, mode);
		}

		/// <summary>
		/// The equivalent of Rigidbody2D.AddTorque adjusted for time effects.
		/// </summary>
		public void AddTorque(float torque, ForceMode2D mode = ForceMode2D.Force)
		{
			if (AssertForwardForce(Severity.Ignore))
				component.AddTorque(AdjustForce(torque), mode);
		}

		#endregion
	}
}
