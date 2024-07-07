using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Sandbox;

public sealed class WebSocketUtility : Component
{
	[Property] public List<WebsocketTools> websocketToolsList { get; set; }
	protected override void OnAwake()
	{
		foreach ( var websocketTools in websocketToolsList )
		{
			if ( websocketTools.url is null )
			{
				Log.Error( "WebsocketTools URL is null" );
				return;
			}
			websocketTools.webSocket = new WebSocket();
			ConnectToSocket( websocketTools.webSocket, websocketTools.url );
			websocketTools.isConnected = true;
			websocketTools.webSocket.OnMessageReceived += websocketTools.OnMessageReceivedMethod;
			websocketTools.isSubscribed = true;
		}
	}

	protected override void OnUpdate()
	{
		SendMessageFromList( WebsocketTools.Fetch.OnUpdate );
	}
	protected override void OnFixedUpdate()
	{
		SendMessageFromList( WebsocketTools.Fetch.OnFixedUpdate );
	}
	protected override void OnStart()
	{
		SendMessageFromList( WebsocketTools.Fetch.OnStart );
	}

	private async void SendMessageFromList( WebsocketTools.Fetch fetch )
	{
		foreach ( var websocketTools in websocketToolsList )
		{
			if ( websocketTools.fetch == fetch )
			{
				if ( websocketTools.message.UseJsonTags )
				{
					var jsonStrings = websocketTools.message.jsonTags.Select( tag => Json.Serialize( tag ) );

					var bigString = string.Join( "", jsonStrings );

					var finalJsonString = Json.Serialize( bigString );

					await websocketTools.webSocket.Send( finalJsonString );
				}
				else
				{
					var messageBytes = Encoding.UTF8.GetBytes( websocketTools.message.message );
					await websocketTools.webSocket.Send( messageBytes );
				}
			}
		}
	}
	[Description( "Sends a message over a websocket connection" )]
	public static async void Send( WebsocketTools websocketTools )
	{
		if ( websocketTools.webSocket is null )
		{
			websocketTools.webSocket = new WebSocket();
		}
		if ( !websocketTools.isConnected )
		{
			await websocketTools.webSocket.Connect( websocketTools.url );
			websocketTools.isConnected = true;
		}
		await websocketTools.webSocket.Send( websocketTools.message.message );
		if ( !websocketTools.isSubscribed )
		{
			websocketTools.webSocket.OnMessageReceived += websocketTools.OnMessageReceivedMethod;
			websocketTools.isSubscribed = true;
		}
	}


	private async void ConnectToSocket( WebSocket webSocket, string url )
	{
		await webSocket.Connect( url );
	}

	[ActionGraphNode( "new websocket tools" ), Pure]
	public static WebsocketTools NewWebsocketTools()
	{
		return new WebsocketTools();
	}
}

public class WebsocketTools
{
	public delegate void OnMessageReceived( string message );
	public OnMessageReceived onMessageReceived { get; set; }
	public WebSocket webSocket { get; set; }
	public string url { get; set; }
	public WebsocketMessage message { get; set; }
	public bool isConnected { get; set; }
	public bool isSubscribed { get; set; }
	public string returnMessage { get; set; }
	public enum Fetch
	{
		OnUpdate,
		OnFixedUpdate,
		OnStart,
	}
	public Fetch fetch { get; set; }

	public void OnMessageReceivedMethod( string message )
	{
		onMessageReceived?.Invoke( message );
		returnMessage = message;
	}

	public WebsocketTools()
	{
		url = null;
		fetch = Fetch.OnUpdate;
		onMessageReceived = null;
		message = null;
	}

	public WebsocketTools( string url, OnMessageReceived onMessageReceived, WebsocketMessage message, Fetch fetch = Fetch.OnUpdate )
	{
		this.url = url;
		this.fetch = fetch;
		this.onMessageReceived = onMessageReceived;
		this.message = message;
	}

}
[GameResource( "Message", "message", "A message to be sent over a websocket connection", Icon = "chat_bubble" )]
public class WebsocketMessage : GameResource
{
	public bool UseJsonTags { get; set; }
	[ShowIf( "UseJsonTags", false )] public string message { get; set; }
	[ShowIf( "UseJsonTags", true )] public List<JsonTags> jsonTags { get; set; }
}

public class JsonTags
{
	public string tag { get; set; }
	public string value { get; set; }

}
