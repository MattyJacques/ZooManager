using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ZooManager
{
    public class Building
    {
        public string id { get; set; } // no_spaces string id
        public string name { get; set; }
        public bool enclosure { get; set; } // is this an animal enclosure?
        public bool shop { get; set; } // is this a shop?
        public int cost { get; set; } // building cost
        public int maintenance { get; set; } // maintenance cost (recurring)
    }
}