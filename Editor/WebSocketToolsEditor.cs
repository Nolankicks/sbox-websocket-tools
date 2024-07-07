using System.Collections.Generic;
using System.Linq;
using Editor;
using Editor.ActionGraphs;
using Editor.Widgets;
using Microsoft.VisualBasic;
using Sandbox;

[CustomEditor( typeof( WebsocketTools ) )]
public class WebsocketToolsControlWidget : ControlWidget
{
	public WebsocketToolsControlWidget( SerializedProperty property ) : base( property )
	{
		Layout = Layout.Column();
		if ( property.IsNull )
		{
			property.SetValue( new WebsocketTools() );
		}

		var serializedObject = property.GetValue<WebsocketTools>()?.GetSerialized();
		if ( serializedObject is null ) return;

		serializedObject.TryGetProperty( nameof( WebsocketTools.url ), out var url );
		serializedObject.TryGetProperty( nameof( WebsocketTools.fetch ), out var fetch );
		serializedObject.TryGetProperty( nameof( WebsocketTools.onMessageReceived ), out var onMessageReceived );
		serializedObject.TryGetProperty( nameof( WebsocketTools.message ), out var message );

		var controlSheet = new ControlSheet();
		controlSheet.AddRow( url );
		controlSheet.AddRow( fetch );
		controlSheet.AddRow( onMessageReceived );
		controlSheet.AddRow( message );
		Layout.Add( controlSheet );
	}
}

[CustomEditor( typeof( JsonTags ) )]
public class JsonTagsControlWidget : ControlWidget
{
	public JsonTagsControlWidget( SerializedProperty property ) : base( property )
	{
		Layout = Layout.Column();
		PaintBackground = false;
		if ( property.IsNull )
		{
			property.SetValue( new JsonTags() );
		}

		var serializedObject = property.GetValue<JsonTags>()?.GetSerialized();
		if ( serializedObject is null ) return;

		serializedObject.TryGetProperty( nameof( JsonTags.value ), out var value );
		serializedObject.TryGetProperty( nameof( JsonTags.tag ), out var tag );

		var controlSheet = new ControlSheet();
		controlSheet.AddRow( tag );
		controlSheet.AddRow( value );
		Layout.Add( controlSheet );
	}
}
/*
[EditorForAssetType( "message" )]
public class WebsocketMessageEditor : DockWindow, IAssetEditor
{
	public WebsocketMessage Message;
	public Asset _asset;
	public WebsocketMessageEditor()
	{
		WindowTitle = "Websocket Message Editor";
		MinimumSize = new Vector2( 100, 100 );
		Size = new Vector2( 500, 500 );
		Log.Info( All );
	}

	public void AssetOpen( Asset asset )
	{
		Show();
		Log.Info( asset.AssetType );

		Open( asset.AbsolutePath, asset );
	}


	public void SelectMember( string memberName )
	{
		// Implement the logic to select a member here
	}

	public bool CanOpenMultipleAssets => true; // Or false, depending on your requirements

	protected override void RestoreDefaultDockLayout()
	{

	}

	public void Open( string path, Asset asset = null )
	{
		if ( !string.IsNullOrEmpty( path ) )
		{
			asset ??= AssetSystem.FindByPath( path );
		}
		if ( asset is null ) return;
		if ( asset == _asset )
		{
			Focus();
			return;
		}

		var message = asset.LoadResource<WebsocketMessage>();
		if ( message is null ) return;
		_asset = asset;
		Message = message;
		var mainWidget = new MainWidget( this );
		DockManager.AddDock( null, mainWidget, DockArea.Left, DockManager.DockProperty.HideOnClose );
	}

	public void Save()
	{
		if ( _asset is null ) return;
		Log.Info( "Saving" );
		_asset ??= AssetSystem.RegisterFile( _asset.AbsolutePath );
		_asset.SaveToDisk( Message );
	}
}

public class MainWidget : Widget
{
	public WebsocketMessageEditor Editor { get; set; }
	public List<Dictionary<string, string>> dictionaries = new();
	public List<DictionaryProperty<string, string>> dictionaryProperties = new();
	public int Tags { get; set; } = 0;
	public MainWidget( WebsocketMessageEditor editor ) : base( null )
	{
		Editor = editor;
		Name = "Message Editor";
		WindowTitle = "Message Editor";
		Layout = Layout.Column();
		MinimumWidth = 450f;

		var serializedObject = Editor.Message?.GetSerialized();
		if ( serializedObject is null ) return;
		serializedObject.TryGetProperty( nameof( WebsocketMessage.message ), out var message );

		var controlSheet = new ControlSheet();
		controlSheet.AddRow( message );
		Layout.Add( controlSheet );

		Layout.Add( new Label( "JSON Tags" ) );
		var jsonTagButton = new Button( "Add new JSON Tag" );
		jsonTagButton.Clicked += () =>
		{
			Tags++;

			var dictionary = new Dictionary<string, string>();
			var property = new DictionaryProperty<string, string>( this );

			property.Value = dictionary;
			property.Height = 30;
			property.Width = 300;

			property.SetProperty( "Key", "Tag" + Tags );

			dictionaryProperties.Add( property );
			dictionaries.Add( dictionary );

			Layout.Add( property );
		};

		Layout.Add( jsonTagButton );


		var saveButton = new Button( "Save" );
		saveButton.Clicked += () => Save();
		Layout.Add( saveButton );


	}
	private void Save()
	{
		Editor.Message.jsonTags = dictionaries;
		Editor.Save();
	}
}*/




