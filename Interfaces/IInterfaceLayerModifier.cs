using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TModder.Interfaces
{
    public interface IInterfaceLayerModifier
    {
        void ModifyInterfaceLayers(List<GameInterfaceLayer> layers);
    }
}
