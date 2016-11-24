using UnityEngine;
using System.Collections;

namespace ZooManager
{
    public class Animal
    {
        public string id { get; set; } // no_spaces string id
        public string name { get; set; }
        public string description { get; set; }
        public int cost { get; set; } // animal purchase cost cost
    }
}