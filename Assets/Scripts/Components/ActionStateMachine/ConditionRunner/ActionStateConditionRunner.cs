// Sifaka Game Studios (C) 2017

using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Components.ActionStateMachine.ConditionRunner
{
    public class ActionStateConditionRunner
    {
        private List<List<ActionStateCondition>> ConditionTracks { get; set; }

        public ActionStateConditionRunner()
        {
            ConditionTracks = new List<List<ActionStateCondition>> {new List<ActionStateCondition>()};
        }

        public void AddCondition(ActionStateCondition inCondition)
        {
            if (ConditionTracks.Count == 1)
            {
                inCondition.Start();
            }
            ConditionTracks[ConditionTracks.Count-1].Add(inCondition);
        }

        public void PushNewTrack()
        {
            ConditionTracks.Add(new List<ActionStateCondition>());
        }

        public void Update(float deltaTime)
        {
            if (!IsComplete())
            {
                var trackComplete = true;
                var completedConditions = new List<ActionStateCondition>();

                foreach (var condition in ConditionTracks.First())
                {
                    condition.Update(deltaTime);
                    if (condition.Complete)
                    {
                        condition.End();
                        completedConditions.Add(condition);
                    }
                    else
                    {
                        trackComplete = false;
                    }
                }

                ConditionTracks.First().RemoveAll(condition => completedConditions.Contains(condition));

                if (trackComplete)
                {
                    ConditionTracks.RemoveAt(0);
                    if (ConditionTracks.Count >= 1)
                    {
                        foreach (var condition in ConditionTracks[0])
                        {
                            condition.Start();
                        }
                    }
                }
            }
        }

        public bool IsComplete()
        {
            return ConditionTracks.Count == 0;
        }
    }
}
