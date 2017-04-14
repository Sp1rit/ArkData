# ARK Player & Tribe file parser
I created this library to use for my new version of the ARK Player & Tribe Viewer. It is an easy way to parse player & tribe files from ARK: Survival Evolved to extract information of you users that RCON tools can't provide.

While I plan to add more fields to be read out, there are some things that can't be parsed easily. An example are the tamed dinos which are saved in the world file.

## Usage
Create a new container
```csharp
var playerParser = new PlayerFileParser();
var tribeParser = new TribeFileParser();
var steamApi = new SteamApi();
var steamServer = new SteamServer();

var container = new Container(playerParser, tribeParser, steamApi, steamServer);
```

To load player & tribe files you just need the following line.
The players and tribes will be loaded and interlinked so tribes are linked to owners/members.
```csharp
container.LoadDirectory("C:\\ArkDataFiles");
```

You can also load additional Steam information for the user like bans and its avatar.
In addition to that the Steam username is refreshed if it has changed since ARK only saves the user name when the player first joins the server.
```csharp
container.LoadSteamData("SteamAPIKey");
```

To see if a player is online the have to request the online players from your server.
You can do that with a simple call.
```csharp
var ip = IPAddress.Parse("127.0.0.1");
var server = new IPEndPoint(ip, 27015);

container.LoadOnlinePlayers(server);
```

All methods can also be accessed asynchronous using the async pattern.
```csharp
var container = new Container(playerParser, tribeParser, steamApi, steamServer);

await container.LoadDirectoryAsync("C:\\ArkDataFiles");
await container.LoadSteamData("SteamAPIKey");
await container.LoadOnlinePlayers(ip, 27015);
```
