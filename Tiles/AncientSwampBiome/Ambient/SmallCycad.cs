using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

using ABMod.Common;

namespace ABMod.Tiles.AncientSwampBiome.Ambient
{
    public class SmallCycad : ModTile
    {
        //Texture
        private Asset<Texture2D> PlantTexture;
        
        public override void SetStaticDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.ReplaceTileBreakUp[Type] = true;
            TileID.Sets.BreakableWhenPlacing[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.newTile.AnchorValidTiles = new int[]
            {
                ModContent.TileType<PrehistoricMoss>(),
            };
            TileObjectData.addTile(Type);
            HitSound = SoundID.Dig;
            DustType = DustID.GreenMoss;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1: 3;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            PlantTexture ??= ModContent.Request<Texture2D>(Texture + "_Full");

            //Get the tile, color and offsets
			Color col = Lighting.GetColor(i, j);

            Vector2 pos = TileGlobal.TileCustomPosition(i, j);
            Vector2 offset = new Vector2(34, 62);

            Main.spriteBatch.Draw(PlantTexture.Value, pos, new Rectangle(0, 0, 84, 76), new Color(col.R, col.G, col.B, 255), 0f, offset, 1f, SpriteEffects.None, 0f);
            
            return base.PreDraw(i, j, spriteBatch);
        }
	}
}