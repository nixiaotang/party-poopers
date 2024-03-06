using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public enum Type
    {
        Fire,
        Ice,
        Earth,
        Elec,
        Light,
        Dark
    }

    [System.Serializable]
    public class CardInfo
    {
        public string name;
        public Type type;
        public int cost;
        public int damage;
    }


    [SerializeField] private CardInfo _cardInfo;


    void Start()
    {
        

    }

}
