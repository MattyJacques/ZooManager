// Sifaka Game Studios (C) 2017

using System;
using UnityEngine;

namespace Assets.Scripts.Blackboards
{
    public class BlackboardItem
    {
        private readonly Type _itemType;
        private readonly object _currentItem;

        public BlackboardItem(object inCurrentItem)
        {
            _itemType = inCurrentItem.GetType();
            _currentItem = inCurrentItem;
        }

        public TReturnType GetCurrentItem<TReturnType>()
            where TReturnType : class
        {
            if (typeof(TReturnType) == _itemType)
            {
                return (TReturnType)_currentItem;
            }

            Debug.LogError("Stored Item type does not match expected ItemType!");

            return null;
        }
    }
}
