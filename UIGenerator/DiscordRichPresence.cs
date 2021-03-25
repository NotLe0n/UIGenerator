using DiscordRPC;
using DiscordRPC.Logging;

namespace UIGenerator
{
    public class DiscordRichPresence
    {
        public DiscordRpcClient client;

        public DiscordRichPresence()
        {
            client = new DiscordRpcClient("822869370543013888")
            {
                Logger = new ConsoleLogger() { Level = LogLevel.Warning }
            };

            //Connect to the RPC
            client.Initialize();

            client.SetPresence(new RichPresence()
            {
                Details = "Editing UIState0.cs", // instead of "UIState0.cs" replace it with the current selected File in the future
                Timestamps = Timestamps.Now,
                Assets = new Assets() { LargeImageKey = "rpcimg" }
            });
        }
    }
}
