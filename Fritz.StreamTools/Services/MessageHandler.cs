using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.CognitiveServices.ContentModerator;
using Microsoft.Extensions.Configuration;

namespace Fritz.StreamTools.Services
{
	public class MessageHandler
	{

		public MessageHandler(IConfiguration configuration)
		{
			this.Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		private string AzureCogServUrl { get { return Configuration["AzureServices:CognitiveServicesUrl"]; } }
		private string ContentModeratorSubsKey { get { return Configuration["AzureServices:ContentModeratorSubKey"]; } }

		public ContentModeratorClient NewClient()
		{
			var client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(ContentModeratorSubsKey));

			client.BaseUrl = AzureCogServUrl;
			return client;
		}


	}
}