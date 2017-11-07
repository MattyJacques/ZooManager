// Sifaka Game Studios (C) 2017

using System;

namespace Assets.Scripts.Core
{
    public class Optional<TOptionalType>
    {
        public Optional()
        {
            _setInitialValue = false;
        }

        public Optional(TOptionalType inOptionalValue)
        {
            _optionalValue = inOptionalValue;
            _setInitialValue = true;
        }

        public bool IsSet()
        {
            return _setInitialValue && _optionalValue != null;
        }

        public TOptionalType Get()
        {
            if (IsSet())
            {
                return _optionalValue;
            }

            throw new NullReferenceException("Underlying optional value was not set or null!");
        }

        private readonly bool _setInitialValue;
        private readonly TOptionalType _optionalValue;
    }
}
