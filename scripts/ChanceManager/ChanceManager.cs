using Godot;
using System;

public partial class ChanceManager : Node
{
	private float chanceRegular = 0.4f;
    private float chanceFire = 0.3f;
    private float chanceOil = 0.3f;
	private Random random = new Random();


	public string GetNextBallType()
    {
        float randomValue = (float)random.NextDouble();

        if (randomValue < chanceRegular)
        {
            UpdateChances("regular");
            return "Regular";
        }
        else if (randomValue < chanceRegular + chanceFire)
        {
            UpdateChances("fire");
            return "Fire";
        }
        else
        {
            UpdateChances("oil");
            return "Oil";
        }
    }

	 private void UpdateChances(string ballType)
    {
        float decreaseFactor = 0.1f; 

        if (ballType == "regular")
        {
            chanceRegular = Mathf.Max(0.1f, chanceRegular - decreaseFactor); 
            chanceFire = Mathf.Min(0.4f, chanceFire + decreaseFactor);     
            chanceOil = Mathf.Min(0.4f, chanceOil + decreaseFactor);        
        }
        else if (ballType == "fire")
        {
            chanceFire = Mathf.Max(0.1f, chanceFire - decreaseFactor);
            chanceRegular = Mathf.Min(0.4f, chanceRegular + decreaseFactor);
            chanceOil = Mathf.Min(0.4f, chanceOil + decreaseFactor);
        }
        else if (ballType == "oil")
        {
            chanceOil = Mathf.Max(0.1f, chanceOil - decreaseFactor);
            chanceRegular = Mathf.Min(0.4f, chanceRegular + decreaseFactor);
            chanceFire = Mathf.Min(0.4f, chanceFire + decreaseFactor);
        }
		NormalizeChances();
    }
	private void NormalizeChances()
    {
        float total = chanceRegular + chanceFire + chanceOil;
        chanceRegular /= total;
        chanceFire /= total;
        chanceOil /= total;
    }

 	public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed){
            	string ballType = GetNextBallType();
            	// GD.Print("The next ball type is: " + ballType);
				// GD.Print("The chance Regular is: "+ chanceRegular);
				// GD.Print("The chance Fire is: "+ chanceFire);
				// GD.Print("The chance Oil is: "+ chanceOil);
			}
        }
    }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
