using Facepunch.Voxels;
using Sandbox;

namespace Terry_excavation.Blocks
{
	[Library]
	public class LeafBlock : BlockType
	{
		public override string DefaultTexture => "leaf";
		public override string FriendlyName => "Leaf";
		public override bool IsTranslucent => true;
	}
}
