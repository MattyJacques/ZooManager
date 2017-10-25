// Sifaka Game Studios (C) 2017

using NUnit.Framework;

namespace Assets.Editor.UnitTests
{
    [TestFixture]
    public class DefaultTestFixture
    {
        [Test]
        public void WhenTestFails_FailsBuild()
        {
            Assert.IsTrue(false);
        }
    }

}

