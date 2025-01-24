using Godot;
using System;

public partial class ChanceManager : Node
{
	private static float chanceRegular = 0.4f;
    private static float chanceFire = 0.3f;
    private static float chanceOil = 0.3f;
	private static Random random = new Random();

	public static BubbleType GetNextBubbleType()
    {
        float randomValue = (float)random.NextDouble();
        BubbleType nextType = BubbleType.Neutral;

        if (randomValue > chanceRegular){
            if (randomValue > chanceRegular + chanceFire){
                nextType = BubbleType.Oil;
            } else {
                nextType = BubbleType.Fire;
            }
        }

        UpdateChances(nextType);
        PrintChosenType(nextType);
        return nextType;
    }

	 private static void UpdateChances(BubbleType bubbleType)
    {
        float decreaseFactor = 0.1f; 

        switch (bubbleType){
            case BubbleType.Neutral:
                chanceRegular = Mathf.Max(0.1f, chanceRegular - decreaseFactor); 
                chanceFire = Mathf.Min(0.4f, chanceFire + decreaseFactor);     
                chanceOil = Mathf.Min(0.4f, chanceOil + decreaseFactor);        
                break;
            case BubbleType.Fire:
                chanceFire = Mathf.Max(0.1f, chanceFire - decreaseFactor);
                chanceRegular = Mathf.Min(0.4f, chanceRegular + decreaseFactor);
                chanceOil = Mathf.Min(0.4f, chanceOil + decreaseFactor);
                break;
            case BubbleType.Oil:
                chanceOil = Mathf.Max(0.1f, chanceOil - decreaseFactor);
                chanceRegular = Mathf.Min(0.4f, chanceRegular + decreaseFactor);
                chanceFire = Mathf.Min(0.4f, chanceFire + decreaseFactor);
                break;
        }
		NormalizeChances();
    }

	private static void NormalizeChances()
    {
        float total = chanceRegular + chanceFire + chanceOil;
        chanceRegular /= total;
        chanceFire /= total;
        chanceOil /= total;
    }

    private static void PrintChosenType(BubbleType bubbleType){
        DebugPrinter.Print("The next ball type is: " + bubbleType, LogCategory.ChanceManager);
        DebugPrinter.Print("The chances are: " + 
            chanceRegular + ", " + 
            chanceFire + ", " + 
            chanceOil, 
        LogCategory.ChanceManager);
    }
}
