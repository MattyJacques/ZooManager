// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using System.Collections;
using Assets.Scripts.Behaviours.Animal;
using Assets.Scripts.Behaviours.Base;
using Assets.Scripts.Behaviours.General;
using Assets.Scripts.Blackboards;
using Assets.Scripts.Components.Enclosure;
using Assets.Scripts.Components.Needs;
using Assets.Scripts.Tests.Components.Enclosure;
using Assets.Scripts.Tests.Components.Needs;
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
        private Blackboard _actualBlackboard;

        [SetUp]
        public void BeforeTest()
        {
            _actualConditionResult = false;
            _actualReturnCode = ReturnCode.Running;

            _needsComponent = new GameObject().AddComponent<MockNeedsComponent>();
            _enclosureComponent = _needsComponent.gameObject.AddComponent<MockEnclosureComponent>();

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

            Assert.IsTrue(_needsComponent.Needs.Contains(_actualBlackboard.InstanceBlackboard[PathfindingBehaviours.PathfindingTargetTypeKey].GetCurrentItem<Need>()));
        }

        [UnityTest]
        public IEnumerator TryFindNeedToImprove_NeedValuesInRange_UpdatesBlackboardWithPosition()
        {
            _needsComponent.Needs.Add(new Need(new NeedParams { MaxValue = AnimalBehaviours.MinNeedThreshold - 1, AssignedNeedType = NeedType.Fun }));

            yield return CoroutineSys.Instance.StartCoroutine(AnimalBehaviours.TryFindNeedToImprove(_actualBlackboard, TestUpdateCondition));

            Assert.AreEqual(_enclosureComponent.ClosestInteriorItemTransformResult.position, _actualBlackboard.InstanceBlackboard[PathfindingBehaviours.PathfindingTargetLocationKey].GetCurrentItem<Vector3>());
        }
        #endregion

        #region ImproveNeed
        [UnityTest]
        public IEnumerator ImproveNeed_RemovesNeedFromBlackboard()
        {
            _actualBlackboard.InstanceBlackboard.Add
            (
                PathfindingBehaviours.PathfindingTargetTypeKey, 
                new BlackboardItem(new Need( new NeedParams{AssignedNeedType = NeedType.Hunger}))
            );

            yield return CoroutineSys.Instance.StartCoroutine(AnimalBehaviours.ImproveNeed(_actualBlackboard,
                TestUpdateCondition));

            Assert.IsFalse(_actualBlackboard.InstanceBlackboard.ContainsKey(PathfindingBehaviours.PathfindingTargetTypeKey));
        }

        [UnityTest]
        public IEnumerator ImproveNeed_ReturnsSuccess()
        {
            _actualBlackboard.InstanceBlackboard.Add
            (
                PathfindingBehaviours.PathfindingTargetTypeKey,
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
                PathfindingBehaviours.PathfindingTargetTypeKey,
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
                PathfindingBehaviours.PathfindingTargetTypeKey,
                new BlackboardItem(new Need(new NeedParams { AssignedNeedType = NeedType.Hunger }))
            );

            yield return CoroutineSys.Instance.StartCoroutine(AnimalBehaviours.ImproveNeed(_actualBlackboard,
                TestUpdateCondition));

            Assert.AreEqual(_enclosureComponent.ClosestInteriorItemTransformResult.gameObject, _enclosureComponent.UnregisteredInteriorItem);
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

#endif // UNITY_EDITOR
