using System;

namespace Creatives.Gozilla
{
    public interface IGodzillaShark
    {
        public event Action<IGodzillaShark> OnDead;
        public void Activate(bool now);
    }
}