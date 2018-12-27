# Fritz.StreamTools
Handy tools for managing my a live video stream and outputting video widgets that can be used directly in OBS or other streaming tools.

## Features

The project is intended to be built as a Docker container and configured with a series of environment variables.  It is intended to support a single-user, and not run on the public-facing internet.

The following features are supported by this project:

*  A checklist rundown of segments of the show (/rundown) that is updated from another page (/admin)
*  A followers count API that reports the total number of followers: GET /api/Followers
*  A followers count page that can be easily styled and formatted at /Followers/Count
*  A followers goal meter that can be sized and have its goal caption and value set, complete with configuration screen at /Followers/Goal/Configuration
![Follower Goal Sample](docs/images/FollowerGoalSample.PNG)

## Services Supported

The project supports reading stream metrics from the following services:

*  Mixer
*  Twitch

## Contributing

This application was built with ASP.NET Core 2.0 and can be built on Mac, Linux, and Windows.  Download the [.NET SDK](https://dot.net) and grab a copy of [Visual Studio Code](https://code.visualstudio.com) to get started on any platform

### Configuration

#### Google Fonts Api

To use your own Google Fonts Api key without modifying the `Fritz.StreamTools\appSettings.json` you can place your key in the `secrets.json` file in the [path appropriate for your OS](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.1&tabs=visual-studio#how-the-secret-manager-tool-works).

The *userSecretsId* is **78c713a0-80e0-4e16-956a-33cf16f08a02** and can be found in `Fritz.StreamTools\Fritz.StreamTools.csproj`.

If you are using Visual Studio you can use the [integrated User Secrets management UI](https://blogs.msdn.microsoft.com/mihansen/2017/09/10/managing-secrets-in-net-core-2-0-apps/)

The `secrets.json` file should look like this


    {
      "GoogleFontsApi": {
        "Key": "<YOUR API KEY>"
      }
    }


### Naming guideline for unit tests:
*  Create a folder for each "logical class"
*  Create a test class for each feature to test - end with "Should"
*  Test Methods should describe what they are inspecting and what they're given, if anything.. ending with "\_Given..."
Example: StreamService.CurrentFollowerCountShould.MatchCurrentFollowerCount_GivenOneService
