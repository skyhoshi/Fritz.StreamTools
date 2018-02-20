using Microsoft.CognitiveServices.ContentModerator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TwitchLib;
using TwitchLib.Events.Client;
using TwitchLib.Models.Client;

namespace Fritz.StreamTools.Services
{
	public class BotService : IHostedService
	{
		private TwitchClient _Client;

		public BotService(IConfiguration config, ILoggerFactory loggerFactory, MessageHandler messageHandler)
		{
			this.Configuration = config;
			this.Logger = loggerFactory.CreateLogger("BotService");
			this.MessageHandler = messageHandler;

		}

		public IConfiguration Configuration { get; }
		public ILogger Logger { get; }
		public MessageHandler MessageHandler { get; }

		private string Channel { get { return Configuration["StreamServices:Twitch:Channel"]; } }

		private string ChannelId { get { return Configuration["StreamServices:Twitch:UserId"]; } }

		private string ChatToken { get { return Configuration["StreamServices:Twitch:ChatToken"]; } }

		public Task StartAsync(CancellationToken cancellationToken)
		{

			ConnectToTwitch();
			return Task.CompletedTask;

		}

		private void ConnectToTwitch()
		{

			var credentials = new ConnectionCredentials(Channel, ChatToken);

			_Client = new TwitchClient(credentials, Channel);
			// _Client.OnJoinedChannel += onJoinedChannel;
			_Client.OnMessageReceived += onMessageReceived;
			_Client.OnWhisperReceived += onWhisperReceived;
			_Client.OnConnected += _Client_OnConnected;
			_Client.OnConnectionError += _Client_OnConnectionError;

			_Client.Connect();

			Logger.LogInformation($"Client is connected: {_Client.IsConnected}");


		}

		private void _Client_OnConnectionError(object sender, OnConnectionErrorArgs e)
		{
			this.Logger.LogError($"Problem connecting to Twitch for the Bot: {e.Error}");
		}

		private void _Client_OnConnected(object sender, OnConnectedArgs e)
		{
			this.Logger.LogInformation("We're connected to Twitch");
		}

		private void onWhisperReceived(object sender, OnWhisperReceivedArgs e)
		{
			// throw new NotImplementedException();
		}

		private void onMessageReceived(object sender, OnMessageReceivedArgs e)
		{

			var stopwatch = System.Diagnostics.Stopwatch.StartNew();

			// Don't moderate these folks
			if (e.ChatMessage.IsMe || e.ChatMessage.IsBroadcaster) return;

			using (var client = MessageHandler.NewClient())
			{
				var screenedValue = client.TextModeration.ScreenText("eng", "text/plain",
					e.ChatMessage.Message, classify: true);
				this.Logger.LogInformation($"Message: '{e.ChatMessage.Message}' Offensive: {screenedValue.Classification.OffensiveScore}");
			}

			this.Logger.LogInformation($"Completed moderating in {stopwatch.ElapsedMilliseconds}ms");

		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_Client.Disconnect();
			_Client = null;
			return Task.CompletedTask;
		}
	}
}
