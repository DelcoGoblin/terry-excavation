using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace Sandbox;
/// <summary>
/// This is your game class. This is an entity that is created serverside when
/// the game starts, and is replicated to the client. 
/// 
/// You can use this to create things like HUDs and declare which player class
/// to use for spawned players.
/// </summary>
public partial class MiniEmpires : GameManager
{
	public MiniEmpires()
	{

		/*for(int i = 0; i < 5; i++ ) //praise the sun
		{
			int x;
			int y;

			x = i * 90;

			if ( x > 270 )
			{
				y = i * 90;
				new EnvironmentLightEntity() 
				{
					Enabled = true,
					Rotation = new Rotation( 0, y, 0, 0 )
				};
				continue;
			}

			new EnvironmentLightEntity()
			{
				Enabled = true,
				Rotation = new Rotation( x, 0, 0, 0 )
			};
		}

		new EnvironmentLightEntity()
		{
			Enabled = true,
			Rotation = new Rotation( 0, 0, 0, 0 ),
		};
		*/
		Color color = new Color( 1, 1, 1, 1 );

		//new SceneSunLight( Game.SceneWorld, Rotation = new Rotation( 0, 0, 0, 0 ), color);

		Game.SceneWorld.AmbientLightColor = color;

		StartBuild();

		var mat = Material.Load( "materials/skybox/skybox_day_cloudy_field_a.vmat" );
		//var mod = Model.Load( "models/rust_props/wooden_pallets/pallet_stacks_a.vmdl" );
		//var pos = new Vector3( 0, 50, 0 );
		//new Prop() { Static = true, Model = mod, Position = pos};

		new SceneSkyBox( Game.SceneWorld, mat ); //add a skybox because its weird when its pitch black
	}
	/// <summary>
	/// A client has joined the server. Make them a pawn to play with
	/// </summary>
	public override void ClientJoined( IClient client )
	{
		base.ClientJoined( client );

		// Create a pawn for this client to play with
		var pawn = new Pawn();
		client.Pawn = pawn;

		// Get all of the spawnpoints


		// chose a random one

		pawn.Position = new Vector3( 200, 200, 0 );

	}

	public void StartBuild()
	{
		List<Vector3> addpos = GenSpawn();
		for ( int i = 0; i < addpos.Count; i++ )
		{
			var position = addpos[i];
			AddBlock( position );
			//Log.Info( "Added Block" + i );
		}
	}

	public List<Vector3> GenSpawn()
	{
		List<Vector3> posi = new List<Vector3>();

		int x;
		int y;
		int z;

		for ( int i = 0; i < 1000; i++ )
		{
			x = i * 15;
			y = 0;
			z = 0;
			posi.Add( new Vector3( x, y, z ) );
			//Log.Info( "Added Block" );

			for ( int b = 0; b < 10; b++ )
			{
				x = i * 15;
				y = 0;
				z = b * 15;
				posi.Add( new Vector3( x, y, z ) );
			}
		}

		return posi;
	}

	static void AddBlock(Vector3 pos)
	{
		Vox vox = new();
		var mesh = new Mesh( Material.Load( "materials/dev/reflectivity_30.vmat" ) );
		mesh = vox.BuildMesh( mesh );
		var model = Model.Builder
				.AddMesh( mesh )
				.AddCollisionBox( 8 )
				.Create();
		_ = new SceneObject( Game.SceneWorld, model )
		{
			Position = pos
		};
	}
}
