﻿using Fritz.StreamLib.Core;
using Fritz.StreamTools.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TwitchLib.Events.Services.FollowerService;
using Xunit;
using FRITZ = Fritz.StreamTools.Services;

namespace Test.Services.TwitchService
{

	public class OnNewFollowers
	{

		[Theory(Skip ="not now")]
		[AutoMoqData]

		public void ShouldSetCurrentFollowerCount(
			IConfiguration configuration,
			ILoggerFactory loggerFactory,
			int initialFollowers,
			OnNewFollowersDetectedArgs args)
		{

			// arrange

			// act
			var sut = new FRITZ.TwitchService(configuration, loggerFactory, null, null)
			{
				CurrentFollowerCount = initialFollowers
			};

			sut.Service_OnNewFollowersDetected(null, args);

			// assert
			Assert.Equal(args.NewFollowers.Count + initialFollowers, sut.CurrentFollowerCount);

		}


		[Theory(Skip ="not now")]
		[AutoMoqData]
		public void ShouldRaiseEventProperly(
			IConfiguration configuration,
			ILoggerFactory loggerFactory,
			int initialFollowers,
			OnNewFollowersDetectedArgs args)
		{

			// arrange
			var sut = new FRITZ.TwitchService(configuration, loggerFactory, null, null)
			{
				CurrentFollowerCount = initialFollowers
			};

			// assert
			var evt = Assert.Raises<ServiceUpdatedEventArgs>(
				h => sut.Updated += h,
				h => sut.Updated -= h,
				() => sut.Service_OnNewFollowersDetected(null, args)
			);

			Assert.Equal(initialFollowers + args.NewFollowers.Count, evt.Arguments.NewFollowers);
			Assert.Null(evt.Arguments.NewViewers);

		}

	}

}
