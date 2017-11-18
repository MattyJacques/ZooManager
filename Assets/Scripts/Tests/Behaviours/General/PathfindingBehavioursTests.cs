﻿// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using System.Collections;
using Assets.Scripts.Behaviours.Base;
using Assets.Scripts.Behaviours.General;
using Assets.Scripts.Blackboards;
using Assets.Scripts.Components.Enclosure;
using Assets.Scripts.Tests.Components.Enclosure;
using Assets.Scripts.Tests.Components.Needs;
using Assets.Scripts.Tests.Components.Pathfinding;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Scripts.Tests.Behaviours.General
{
    [TestFixture]
    public class PathfindingBehavioursTestFixture
    {
        private ReturnCode _actualReturnCode;

        private MockNeedsComponent _needsComponent;
        private MockEnclosureComponent _enclosureComponent;
        private MockPathfindingComponent _pathfindingComponent;
        private Blackboard _actualBlackboard;

        [SetUp]
        public void BeforeTest()
        {
            _actualReturnCode = ReturnCode.Running;

            _needsComponent = new GameObject().AddComponent<MockNeedsComponent>();
            _enclosureComponent = _needsComponent.gameObject.AddComponent<MockEnclosureComponent>();
            _pathfindingComponent = _needsComponent.gameObject.AddComponent<MockPathfindingComponent>();

            _enclosureComponent.gameObject.AddComponent<EnclosureResidentComponent>().RegisteredEnclosure =
                _enclosureComponent;

            _enclosureComponent.ClosestInteriorItemTransformResult = _enclosureComponent.gameObject.transform;

            _actualBlackboard = new Blackboard();
            _actualBlackboard.InstanceBlackboard.Add(BehaviourTree.GameObjectKey, new BlackboardItem(_enclosureComponent.gameObject));
        }

        [TearDown]
        public void AferTest()
        {
            _actualBlackboard = null;
            _pathfindingComponent = null;
            _enclosureComponent = null;
            _needsComponent = null;
        }

        #region MoveToTarget
        [UnityTest]
        public IEnumerator MoveToTarget_RemovesTargetFromBlackboard()
        {
            _actualBlackboard.InstanceBlackboard.Add
            (
                PathfindingBehaviours.PathfindingTargetLocationKey,
                new BlackboardItem(new Vector3())
            );

            yield return CoroutineSys.Instance.StartCoroutine(PathfindingBehaviours.MoveToTarget(_actualBlackboard,
                TestUpdateCondition));

            Assert.IsFalse(_actualBlackboard.InstanceBlackboard.ContainsKey(PathfindingBehaviours.PathfindingTargetLocationKey));
        }

        [UnityTest]
        public IEnumerator MoveToTarget_ReturnsSuccess()
        {
            _actualBlackboard.InstanceBlackboard.Add
            (
                PathfindingBehaviours.PathfindingTargetLocationKey,
                new BlackboardItem(new Vector3())
            );

            yield return CoroutineSys.Instance.StartCoroutine(PathfindingBehaviours.MoveToTarget(_actualBlackboard,
                TestUpdateCondition));

            Assert.AreEqual(ReturnCode.Success, _actualReturnCode);
        }

        [UnityTest]
        public IEnumerator MoveToTarget_StartPathfindingCalledWithBlackboardVector()
        {
            var expectedPosition = new Vector3(1, 3, 4);

            _actualBlackboard.InstanceBlackboard.Add
            (
                PathfindingBehaviours.PathfindingTargetLocationKey,
                new BlackboardItem(expectedPosition)
            );

            yield return CoroutineSys.Instance.StartCoroutine(PathfindingBehaviours.MoveToTarget(_actualBlackboard,
                TestUpdateCondition));

            Assert.IsTrue(_pathfindingComponent.StartPathfindingCalled);

            Assert.AreEqual(expectedPosition, _pathfindingComponent.StartPathfindingTargetVector);
        }
        #endregion

        #region GetRandomPointInsideEnclosure
        [UnityTest]
        public IEnumerator GetRandomPointInsideEnclosure_ReturnSuccess()
        {
            yield return CoroutineSys.Instance.StartCoroutine(PathfindingBehaviours.GetRandomPointInsideEnclosure(_actualBlackboard,
                TestUpdateCondition));

            Assert.AreEqual(ReturnCode.Success, _actualReturnCode);
        }

        [UnityTest]
        public IEnumerator GetRandomPointInsideEnclosure_GetRandomPointOnTheGroundResultStoredInBT()
        {
            _enclosureComponent.GetRandomPointOnTheGroundResult = new Vector3(3, 4, 5);

            yield return CoroutineSys.Instance.StartCoroutine(PathfindingBehaviours.GetRandomPointInsideEnclosure(_actualBlackboard,
                TestUpdateCondition));

            Assert.AreEqual(_enclosureComponent.GetRandomPointOnTheGroundResult, _actualBlackboard.InstanceBlackboard[PathfindingBehaviours.PathfindingTargetLocationKey].GetCurrentItem<Vector3>());
        }
        #endregion

        private void TestUpdateCondition(ReturnCode returnCode)
        {
            _actualReturnCode = returnCode;
        }
    }
}

#endif // UNITY_EDITOR
