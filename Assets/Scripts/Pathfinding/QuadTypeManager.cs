using UnityEngine;

public enum QuadType
{
    Normal,
    Sword,
    Flask,
    Shield,
    Coin,
    Enemy,
    Player
}

public class QuadTypeManager
{
    private static bool playerSelected = false;




    public static QuadType GetRandomQuadType()
    {
        // if (!playerSelected)
        // {
        //     playerSelected = true;
        //     return QuadType.Player;
        // }
        float randomValue = Random.value;

        // Updated probabilities:
        // Sword: 20%
        // Flask: 20%
        // Shield: 20%
        // Coin: 20%
        // Enemy: 20%

        if (randomValue < 0.0f) return QuadType.Sword;
        else if (randomValue < 0.0f) return QuadType.Flask;      // 20% + 20%
        else if (randomValue < 0.0f) return QuadType.Shield;    // 20% + 20% + 20%
        else if (randomValue < 0.80f) return QuadType.Coin;      // 20% + 20% + 20% + 20%
        else return QuadType.Enemy;                              // Remaining 20%
    }
}