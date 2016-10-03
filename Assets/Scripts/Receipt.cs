// Title        : Receipt.cs
// Purpose      : Object in a log of transactions
// Author       : Jacob Miller
// Date         : 22/09/2016

public class Receipt
{
  //public enum Type {Product = 0,Paid = 1,Task = 2,Payday = 3,NA = 4};
  public enum Type {Product,Paid,Task,Payday,NA};           //What the money is being used for task wise
  public Type _type = Type.Product;                         //The type of transactions
  float _transAmount = 0;                                   //How much is being withdrawn
  float _previousAmount = 0;                                //How much money you had before the transactions
  string _for = "";                                         //What the money is being used for
  
  public Receipt(Type type, float totalBefore, float amount, string forWhat)
  {//Builds the Receipt
    _type = type;
    _transAmount = amount;
    _for = forWhat;
    _previousAmount = totalBefore;
  }
  
  public string Print()
  {//Return all availible info
    if (_transAmount >= 0)
      return("Type: " + _type + " Amount: +" + _transAmount + "\n" + " For: " + _for);
    return("Type: " + _type + " Amount: " + _transAmount + "\n" + " For: " + _for);
  }
  
}