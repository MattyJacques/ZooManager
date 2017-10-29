// Sifaka Game Studios (C) 2017

using Assets.Scripts.Blackboards;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.Blackboards
{
    [TestFixture]
    public class BlackboardItemTestFixture
    {
        [Test]
        public void GetCurrentItem_WrongType_LogsError()
        {
            var blackBoardItem = new BlackboardItem("hello");

            LogAssert.Expect(LogType.Error, "Stored Item type does not match expected ItemType!");

            blackBoardItem.GetCurrentItem<MonoBehaviour>();
        }

        [Test]
        public void GetCurrentItem_RightType_ReturnsItem()
        {
            const string expectedString = "hello";
            var blackBoardItem = new BlackboardItem(expectedString);

            Assert.AreEqual(expectedString, blackBoardItem.GetCurrentItem<string>());
        }
    }
}
