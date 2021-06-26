using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnumLib
{
    public enum itemType { Weapon, Potion, Shield, Money};
    public enum textType { Greeting, Buy, Sell, DepartAngry, DepartHappy}
    public enum conditionals { None, Item, ItemInFront, Money}
    public enum resutls { None, Sell, Buy, Item}
}
