// Sifaka Game Studios (C) 2017

using BlackboardContainer = System.Collections.Generic.Dictionary<string, Assets.Scripts.Blackboards.BlackboardItem>;

namespace Assets.Scripts.Blackboards
{
    public class Blackboard
    {
        public static BlackboardContainer GlobalBlackboard { get; set; }
        public BlackboardContainer InstanceBlackboard { get; set; }
    }
}
