// Title        : metaManager.cs
// Purpose      : To store game instance information
// Author       : Jeremy Mann
// Date         : 03/01/2017

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Variable Declaration START ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

	public float _currentFunds;

	public float _secondsPerEconomyCycle;
	public int _economyCycleNum;

	public float _visitorsNewNum;
	public float _visitorsOldNum; // Difference between old and new vistors is that old ones paid in previous cycles, new ones paid this cycle
	public float _admissionPerVisitor;

	public float _totalAttractionValue;
	public float _visitorSpawnRate; // Number that interacts with TotalAttractionValue too determine how many new visitors per cycle. Potential uses, difficulty levels and ingame events.
	public float _animalAttractionValue;
	public float _retentionValue; //percentage, you keep this percentage of Old vistors per cycle, others leave.

	private float _totalExpenses;
	private float _totalRevenue;


	public float _costPerAnimal;
	public float _costPerBuilding;

	public int _animalNum;
	public int _buildingNum;

	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Variable Declaration END ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Function Declaration START ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

	// Use this for initialization
	void Start () 
	{
		StartCoroutine(EconomyCycler());
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	IEnumerator EconomyCycler () // The economy works in cycles or ticks. Every X seconds revenue/expenses/attraction is calculated and added.
	{
		yield return new WaitForSeconds (_secondsPerEconomyCycle);
		Expenses();
		Revenue();
		Attraction();
		StartCoroutine(EconomyCycler());
		_economyCycleNum = (_economyCycleNum + 1);
	}

	public void Revenue () // Calculate the total incoming money for this economy cycle and then add it too current funds
	{
		_totalRevenue = (_visitorsNewNum * _admissionPerVisitor);
		_currentFunds = (_currentFunds + _totalRevenue);
	}

	public void Expenses () // Calculate the expenses for this Economy cycle and subtract it from current funds
	{
		_totalExpenses = ((_animalNum * _costPerAnimal) + (_buildingNum * _costPerBuilding));

		_currentFunds = (_currentFunds - _totalExpenses);
	}

	public void Attraction () // Calculate the attraction value of the park and draw in appropriate viistors
	{
		_visitorsOldNum = Mathf.Round((_visitorsOldNum/100)*_retentionValue);
		_visitorsOldNum = (_visitorsOldNum + _visitorsNewNum);
		_visitorsNewNum = 0;

		_totalAttractionValue = (_animalNum * _animalAttractionValue);

		_visitorsNewNum = (_totalAttractionValue * _visitorSpawnRate);

	}
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Function Declaration END ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~