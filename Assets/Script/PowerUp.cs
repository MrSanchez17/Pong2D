using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { ShrinkOpponent, GrowSelf, SpeedUp, SlowOpponent, MultiBall }
    public PowerUpType powerUpType;

    private void Start()
    {
        Destroy(gameObject, 5f); 
    }
}
