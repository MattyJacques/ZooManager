// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using System;
using Assets.Scripts.Core;
using NUnit.Framework;

namespace Assets.Editor.UnitTests.Core
{
    [TestFixture]
    public class OptionalTestFixture
    {
        [Test]
        public void DefaultConstructor_IsSet_False()
        {
            var testOptional = new Optional<int>();
            Assert.IsFalse(testOptional.IsSet());
        }

        [Test]
        public void DefaultConstructor_Get_ThrowsException()
        {
            var testOptional = new Optional<int>();
            Assert.Throws<NullReferenceException>(() => testOptional.Get());
        }

        [Test]
        public void ConstructWithNullObject_IsSet_False()
        {
            object testObject = null;
            var testOptional = new Optional<object>(testObject);
            Assert.IsFalse(testOptional.IsSet());
        }

        [Test]
        public void ConstructWithNullObject_Get_ThrowsException()
        {
            object testObject = null;
            var testOptional = new Optional<object>(testObject);
            Assert.Throws<NullReferenceException>(() => testOptional.Get());
        }

        [Test]
        public void ConstructWithObject_IsSet_True()
        {
            var testObject = new object();
            var testOptional = new Optional<object>(testObject);
            Assert.IsTrue(testOptional.IsSet());
        }

        [Test]
        public void ConstructWithObject_Get_ReturnsObject()
        {
            var testObject = new object();
            var testOptional = new Optional<object>(testObject);
            Assert.AreSame(testObject, testOptional.Get());
        }
    }
}

#endif
