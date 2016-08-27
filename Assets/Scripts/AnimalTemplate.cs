[System.Serializable]
public class AnimalTemplateCollection
{
    public AnimalTemplate[] animalTemplates;
}

[System.Serializable]
public class AnimalTemplate
{
    public int id;
    public string animalname;
    public int price;
    public string habitat;
    public string[] food;
    public string[] fun;
    public string[] tags;
    public float healthRate;
    public float hungerRate;
    public float thirstRate;
    public float lifespan;
}
