using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.ModLoader.UI;

namespace TModder.Interfaces
{
    public interface IInterfaceLayerModifier
    {
        // Method to modify interface layers
        void ModifyInterfaceLayers(List<GameInterfaceLayer> layers);

        // Method to initialize interface layers
        void InitializeInterfaceLayers();

        // Method to update interface layers
        void UpdateInterfaceLayers(GameTime gameTime);

        // Method to draw interface layers
        void DrawInterfaceLayers(SpriteBatch spriteBatch);

        // Property to get the priority of the interface layer modifier
        int Priority { get; }
    }
}
