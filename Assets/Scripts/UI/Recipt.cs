public class Recipt
{
  public enum Type {Product,Paid,Task,Payday,NA};
  Type _type = Type.Product;
  float _transAmount = 0;
  string _for = "";
  public Recipt(Type type, float amount, string forWhat)
  {
    _type = type;
    _transAmount = amount;
    _for = forWhat;
  }
  
  public string Print()
  {
    return("Type: " + _type + " Amount: " + _transAmount + " For: " + _for);
  }
  
}