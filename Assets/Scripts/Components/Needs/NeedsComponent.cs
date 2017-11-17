// Sifaka Game Studios (C) 2017

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Components.Health;
using UnityEngine;

namespace Assets.Scripts.Components.Needs
{
    [RequireComponent(typeof(IHealthInterface))]
    public class NeedsComponent
        : MonoBehaviour
        , INeedsInterface
    {
        // Array to optimise for updating (map makes more sense, but would be slower for iteration)
        private IList<NeedEntry> NeedEntries { get; set; }

        private IHealthInterface HealthInterface { get; set; }

        public class NeedEntry
        {
            public float TimePassed { get; set; }
            private readonly Need _need;

            public NeedEntry(Need inNeed)
            {
                TimePassed = 0f;
                _need = inNeed;
            }

            public Need GetNeed()
            {
                return _need;
            }
        }

        protected void Start ()
        {
		    NeedEntries = new List<NeedEntry>();

            HealthInterface = gameObject.GetComponent<IHealthInterface>();
        }
	
        protected void Update ()
        {
            var deltaTime = GetDeltaTime();

            foreach (var needEntry in NeedEntries)
            {
                needEntry.TimePassed += deltaTime;

                if (needEntry.TimePassed >= needEntry.GetNeed().GetUpdateFrequency())
                {
                    needEntry.GetNeed().Decay();

                    if (HealthInterface != null)
                    {
                        HealthInterface.AdjustHealth(needEntry.GetNeed().GetHealthAdjustment());
                    }

                    needEntry.TimePassed = 0.0f;
                }
            }
        }

        protected virtual float GetDeltaTime()
        {
            return Time.deltaTime;
        }

        public void AddNeed(Need inNeed)
        {
            if (!ContainsNeedOfType(inNeed.GetNeedType()))
            {
                NeedEntries.Add(new NeedEntry(inNeed));
            }
            else
            {
                Debug.LogError("Tried to add NeedType that already existed!");
            }
        }

        public IEnumerable<Need> GetNeeds()
        {
            return NeedEntries.Select(needEntry => needEntry.GetNeed());
        }

        private bool ContainsNeedOfType(NeedType inNeedType)
        {
            return NeedEntries.Any(needEntry => needEntry.GetNeed().GetNeedType() == inNeedType);
        }
    }
}
