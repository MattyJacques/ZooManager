// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using Assets.Scripts.Instance;

namespace Assets.Scripts.Tests.Instance
{
    public class TestGameInstance 
        : GameInstance
    {
        public void TestAwake()
        {
            Awake();
        }
    }
}

#endif // UNITY_EDITOR
