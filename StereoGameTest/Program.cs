
using StereoGame.Framework;
using StereoGameTest;

using TestGame game = new();
using var gdm = new GraphicsDeviceManager(game);
gdm.PreferredBackBufferWidth = 1024;
gdm.PreferredBackBufferHeight = 720;
game.Components.Add(new RainingBoxes(game));
game.Run();