// Sifaka Game Studios (C) 2017

using System.Collections;
using Assets.Scripts.Behaviours.Animal;
using Assets.Scripts.Behaviours.Base;
using Assets.Scripts.Blackboards;
using Assets.Scripts.Components.Enclosure;
using Assets.Scripts.Components.Needs;
using Assets.Scripts.Tests.Components.Enclosure;
using Assets.Scripts.Tests.Components.Needs;
using Assets.Scripts.Tests.Components.Pathfinding;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Scripts.Tests.Behaviours.Animal
{
    [TestFixture]
    public class AnimalBehavioursTestFixture
    {
        private bool _actualConditionResult;
        private ReturnCode _actualReturnCode;

        private MockNeedsComponent _needsComponent;
        private MockEnclosureComponent _enclosureComponent;
        private MockPathfindingComponent _pathfindingComponent;
        private Blackboard _actualBlackboard;

        [SetUp]
        public void BeforeTest()
        {
            _actualConditionResult = false;
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

        #region TryFindNeedToImprove
        [UnityTest]
        public IEnumerator TryFindNeedToImprove_NoNeeds_ReturnCodeFalse()
        {
            _actualConditionResult = true;

            yield return CoroutineSys.Instance.StartCoroutine(AnimalBehaviours.TryFindNeedToImprove(_actualBlackboard, TestUpdateCondition));

            Assert.IsFalse(_actualConditionResult);
        }

        [UnityTest]
        public IEnumerator TryFindNeedToImprove_NeedValuesTooHigh_ReturnCodeFalse()
        {
            _actualConditionResult = true;

            _needsComponent.Needs.Add(new Need(new NeedParams { MaxValue = AnimalBehaviours.MinNeedThreshold + 1, AssignedNeedType = NeedType.Fun }));

            yield return CoroutineSys.Instance.StartCoroutine(AnimalBehaviours.TryFindNeedToImprove(_actualBlackboard, TestUpdateCondition));

            Assert.IsFalse(_actualConditionResult);
        }

        [UnityTest]
        public IEnumerator TryFindNeedToImprove_NoNeedItemFound_ReturnCodeFalse()
        {
            _actualConditionResult = true;

            _enclosureComponent.ClosestInteriorItemTransformResult = null;

            _needsComponent.Needs.Add(new Need(new NeedParams { MaxValue = AnimalBehaviours.MinNeedThreshold - 1, AssignedNeedType = NeedType.Fun }));

            yield return CoroutineSys.Instance.StartCoroutine(AnimalBehaviours.TryFindNeedToImprove(_actualBlackboard, TestUpdateCondition));

            Assert.IsFalse(_actualConditionResult);
        }

        [UnityTest]
        public IEnumerator TryFindNeedToImprove_NeedValuesInRange_ReturnCodeTrue()
        {
            _needsComponent.Needs.Add(new Need(new NeedParams { MaxValue = AnimalBehaviours.MinNeedThreshold - 1, AssignedNeedType = NeedType.Fun }));

            yield return CoroutineSys.Instance.StartCoroutine(AnimalBehaviours.TryFindNeedToImprove(_actualBlackboard, TestUpdateCondition));

            Assert.IsTrue(_actualConditionResult);
        }

        [UnityTest]
        public IEnumerator TryFindNeedToImprove_NeedValuesInRange_UpdatesBlackboardWithNeed()
        {
            _needsComponent.Needs.Add(new Need(new NeedParams { MaxValue = AnimalBehaviours.MinNeedThreshold - 1, AssignedNeedType = NeedType.Fun }));

            yield return CoroutineSys.Instance.StartCoroutine(AnimalBehaviours.TryFindNeedToImprove(_actualBlackboard, TestUpdateCondition));

            Assert.IsTrue(_needsComponent.Needs.Contains(_actualBlackboard.InstanceBlackboard[AnimalBehaviours.PathfindingTargetTypeKey].GetCurrentItem<Need>()));
        }

        [UnityTest]
        public IEnumerator TryFindNeedToImprove_NeedValuesInRange_UpdatesBlackboardWithPosition()
        {
            _needsComponent.Needs.Add(new Need(new NeedParams { MaxValue = AnimalBehaviours.MinNeedThreshold - 1, AssignedNeedType = NeedType.Fun }));

            yield return CoroutineSys.Instance.StartCoroutine(AnimalBehaviours.TryFindNeedToImprove(_actualBlackboard, TestUpdateCondition));

            Assert.AreEqual(_enclosureComponent.ClosestInteriorItemTransformResult.position, _actualBlackboard.InstanceBlackboard[AnimalBehaviours.PathfindingTargetLocationKey].GetCurrentItem<Vector3>());
        }
        #endregion

        #region ImproveNeed
        [UnityTest]
        public IEnumerator ImproveNeed_RemovesNeedFromBlackboard()
        {
            _actualBlackboard.InstanceBlackboard.Add
            (
                AnimalBehaviours.PathfindingTargetTypeKey, 
                new BlackboardItem(new Need( new NeedParams{AssignedNeedType = NeedType.Hunger}))
            );

            yield return CoroutineSys.Instance.StartCoroutine(AnimalBehaviours.ImproveNeed(_actualBlackboard,
                TestUpdateCondition));

            Assert.IsFalse(_actualBlackboard.InstanceBlackboard.ContainsKey(AnimalBehaviours.PathfindingTargetTypeKey));
        }

        [UnityTest]
        public IEnumerator ImproveNeed_ReturnsSuccess()
        {
            _actualBlackboard.InstanceBlackboard.Add
            (
                AnimalBehaviours.PathfindingTargetTypeKey,
                new BlackboardItem(new Need(new NeedParams { AssignedNeedType = NeedType.Hunger }))
            );

            yield return CoroutineSys.Instance.StartCoroutine(AnimalBehaviours.ImproveNeed(_actualBlackboard,
                TestUpdateCondition));

            Assert.AreEqual(ReturnCode.Success, _actualReturnCode);
        }

        [UnityTest]
        public IEnumerator ImproveNeed_ImprovesExpectedNeedByNeedImproveAmount()
        {
            _needsComponent.Needs.Add(new Need(new NeedParams { MaxValue = AnimalBehaviours.NeedImproveAmount, AssignedNeedType = NeedType.Fun }));
            _needsComponent.Needs[0].AdjustNeed(-AnimalBehaviours.NeedImproveAmount);

            _actualBlackboard.InstanceBlackboard.Add
            (
                AnimalBehaviours.PathfindingTargetTypeKey,
                new BlackboardItem(_needsComponent.Needs[0])
            );

            yield return CoroutineSys.Instance.StartCoroutine(AnimalBehaviours.ImproveNeed(_actualBlackboard,
                TestUpdateCondition));

            Assert.AreEqual(AnimalBehaviours.NeedImproveAmount, _needsComponent.Needs[0].CurrentValue);
        }

        [UnityTest]
        public IEnumerator ImproveNeed_Hunger_UnregistersClosestHungerInteriorItem()
        {
            _actualBlackboard.InstanceBlackboard.Add
            (
                AnimalBehaviours.PathfindingTargetTypeKey,
                new BlackboardItem(new Need(new NeedParams { AssignedNeedType = NeedType.Hunger }))
            );

            yield return CoroutineSys.Instance.StartCoroutine(AnimalBehaviours.ImproveNeed(_actualBlackboard,
                TestUpdateCondition));

            Assert.AreEqual(_enclosureComponent.ClosestInteriorItemTransformResult.gameObject, _enclosureComponent.UnregisteredInteriorItem);
        }
        #endregion

        #region MoveToTarget
        [UnityTest]
        public IEnumerator MoveToTarget_RemovesTargetFromBlackboard()
        {
            _actualBlackboard.InstanceBlackboard.Add
            (
                AnimalBehaviours.PathfindingTargetLocationKey,
                new BlackboardItem(new Vector3())
            );

            yield return CoroutineSys.Instance.StartCoroutine(AnimalBehaviours.MoveToTarget(_actualBlackboard,
                TestUpdateCondition));

            Assert.IsFalse(_actualBlackboard.InstanceBlackboard.ContainsKey(AnimalBehaviours.PathfindingTargetLocationKey));
        }

        [UnityTest]
        public IEnumerator MoveToTarget_ReturnsSuccess()
        {
            _actualBlackboard.InstanceBlackboard.Add
            (
                AnimalBehaviours.PathfindingTargetLocationKey,
                new BlackboardItem(new Vector3())
            );

            yield return CoroutineSys.Instance.StartCoroutine(AnimalBehaviours.MoveToTarget(_actualBlackboard,
                TestUpdateCondition));

            Assert.AreEqual(ReturnCode.Success, _actualReturnCode);
        }

        [UnityTest]
        public IEnumerator MoveToTarget_StartPathfindingCalledWithBlackboardVector()
        {
            var expectedPosition = new Vector3(1, 3, 4);

            _actualBlackboard.InstanceBlackboard.Add
            (
                AnimalBehaviours.PathfindingTargetLocationKey,
                new BlackboardItem(expectedPosition)
            );

            yield return CoroutineSys.Instance.StartCoroutine(AnimalBehaviours.MoveToTarget(_actualBlackboard,
                TestUpdateCondition));

            Assert.IsTrue(_pathfindingComponent.StartPathfindingCalled);

            Assert.AreEqual(expectedPosition, _pathfindingComponent.StartPathfindingTargetVector);
        }
        #endregion

        #region GetRandomPointInsideEnclosure
        [UnityTest]
        public IEnumerator GetRandomPointInsideEnclosure_ReturnSuccess()
        {
            yield return CoroutineSys.Instance.StartCoroutine(AnimalBehaviours.GetRandomPointInsideEnclosure(_actualBlackboard,
                TestUpdateCondition));

            Assert.AreEqual(ReturnCode.Success, _actualReturnCode);
        }

        [UnityTest]
        public IEnumerator GetRandomPointInsideEnclosure_GetRandomPointOnTheGroundResultStoredInBT()
        {
            _enclosureComponent.GetRandomPointOnTheGroundResult = new Vector3(3, 4, 5);

            yield return CoroutineSys.Instance.StartCoroutine(AnimalBehaviours.GetRandomPointInsideEnclosure(_actualBlackboard,
                TestUpdateCondition));

            Assert.AreEqual(_enclosureComponent.GetRandomPointOnTheGroundResult, _actualBlackboard.InstanceBlackboard[AnimalBehaviours.PathfindingTargetLocationKey].GetCurrentItem<Vector3>());
        }
        #endregion

        private void TestUpdateCondition(bool inCondition)
        {
            _actualConditionResult = inCondition;
        }

        private void TestUpdateCondition(ReturnCode returnCode)
        {
            _actualReturnCode = returnCode;
        }
    }
}
