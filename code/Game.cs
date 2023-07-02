
using Sandbox;
using System.Linq;
using Facepunch.Voxels;

namespace Terry_excavation;

public partial class Terry_excavation : Sandbox.GameManager
{
	public Terry_excavation()
	{
		if ( Game.IsClient )
		{
			Game.RootPanel = new Hud();
		}

		new EnvironmentLightEntity() //praise the sun
		{
			Enabled = true,
			Rotation = new Rotation(0,0,0,0)
		};

		var mat = Material.Load( "materials/skybox/skybox_day_cloudy_field_a.vmat" );
		//var mod = Model.Load( "models/rust_props/wooden_pallets/pallet_stacks_a.vmdl" );
		//var pos = new Vector3( 0, 50, 0 );
		//new Prop() { Static = true, Model = mod, Position = pos};

		new SceneSkyBox( Game.SceneWorld, mat ); //add a skybox because its weird when its pitch black




	}

	/*[GameEvent.Tick.Server]
	private void Tick()
	{
		var clients = Game.Clients.ToList();

		foreach ( var client in clients )
		{
			Log.Info( client.Pawn.Position );
		}
	}*/
	public override void ClientJoined( IClient client )
	{
		base.ClientJoined( client );

		
		// Create a pawn for this client to play with
		var pawn = new Pawn();
		client.Pawn = pawn;
		pawn.Respawn();
		pawn.DressFromClient( client );
		pawn.OnMapLoaded();

		pawn.Transform = new Transform( 50, new Rotation( 0, 0, 0, 0 ), 0 ); //put the pawn roughly at 0,0,0

		VoxelWorld.Current.AddViewer( client ); //voxel stuff based on core-wars-game.cs and procgen game.cs

		if ( VoxelWorld.Current.Initialized )
		{
			SendMapToClient( client );
		}
	}

	public override void PostLevelLoaded()
	{
		StartLoadMapTask();
	}

	private async void StartLoadMapTask()
	{
		var world = VoxelWorld.Create( 1337 );

		world.OnInitialized += OnMapInitialized;
		world.SetMaterials( "materials/procgen/voxel.vmat", "materials/procgen/voxel_translucent.vmat" );
		world.SetChunkRenderDistance( 4 );
		world.SetChunkUnloadDistance( 8 );
		world.SetChunkSize( 32, 32, 32 );
		world.SetSeaLevel( 48 );
		world.SetMaxSize( 256, 256, 128 );
		world.LoadBlockAtlas( "textures/blocks/blocks_color.atlas.json" );
		world.AddAllBlockTypes();
		world.SetMinimumLoadedChunks( 8 );

		world.SetChunkGenerator<PerlinChunkGenerator>();
		world.AddBiome<PlainsBiome>();

		var startChunkSize = 4;

		for ( var x = 0; x < startChunkSize; x++ )
		{
			for ( var y = 0; y < startChunkSize; y++ )
			{
				await GameTask.Delay( 100 );

				var chunk = world.GetOrCreateChunk(
					x * world.ChunkSize.x,
					y * world.ChunkSize.y,
					0
				);

				_ = chunk.Initialize();
			}
		}

		await GameTask.Delay( 500 );

		world.Initialize();
	}

	private void OnMapInitialized()
	{
		var clients = Game.Clients.ToList();

		foreach ( var client in clients )
		{
			if ( client.IsValid() && client.Pawn.IsValid() )
			{
				SendMapToClient( client );
			}
		}
	}

	private void SendMapToClient( IClient client )
	{
		VoxelWorld.Current.Send( client );
	}
}

