using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facepunch.Voxels;

namespace Terry_excavation.Blocks
{
	[Library]
	public class GrassBlock : BlockType
	{
		public override string DefaultTexture => "dirt_grass";
		public override string FriendlyName => "Dirt";

		public override byte GetTextureId( BlockFace face, Chunk chunk, int x, int y, int z )
		{
			var position = new IntVector3( x, y, z );

			if ( face == BlockFace.Top )
				return World.BlockAtlas.GetTextureId( "grass" );
			else if ( face == BlockFace.Bottom )
				return World.BlockAtlas.GetTextureId( "dirt" );

			var adjacentBlockId = World.GetAdjacentBlock( chunk.Offset + position, (int)BlockFace.Top );
			var adjacentBlock = World.GetBlockType( adjacentBlockId );

			if ( adjacentBlock.IsTranslucent )
				return World.BlockAtlas.GetTextureId( DefaultTexture );

			return World.BlockAtlas.GetTextureId( "dirt" );
		}
	}
}
