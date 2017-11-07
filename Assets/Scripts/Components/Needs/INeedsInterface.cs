// Sifaka Game Studios (C) 2017

using System.Collections.Generic;

namespace Assets.Scripts.Components.Needs
{
    public interface INeedsInterface
    {
        void AddNeed(Need inNeed);
        IEnumerable<Need> GetNeeds();
    }
}
