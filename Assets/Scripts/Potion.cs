using UnityEngine;


public class Potion
{
    public string Name; 
    public int healAmount; 
    public int speedChangeAmount;
    public int Price; 

    public Potion(string name, int healAmount, int speedChangeAmount, int price)
    {
        this.Name = name;
        this.healAmount = healAmount;
        this.speedChangeAmount = speedChangeAmount;
        this.Price = price;
    }
}
