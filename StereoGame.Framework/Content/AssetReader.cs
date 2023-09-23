using StereoGame.Framework.Audio;
using StereoGame.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework.Content
{
    public abstract class AssetReader<T> : IAssetReader<T>
    {
        private readonly string name;
        private ContentManager? contentManager;
        protected AssetReader(string name)
        {
            this.name = name;
        }

        public string Name => name;

        public ContentManager? ContentManager
        {
            get => contentManager;
            set => contentManager = value;
        }

        object? IAssetReader.ReadAsset(string name, byte[]? data, object? parameter)
        {
            return ReadAsset(name, data, parameter);
        }
        public abstract T? ReadAsset(string name, byte[]? data, object? parameter);

        public GamePlatform? GetPlatform()
        {
            return contentManager?.Game.Platform;
        }
        public GraphicsDevice? GetGraphicsDevice()
        {
            return contentManager?.Game.GraphicsDevice;
        }

        public AudioDevice? GetAudioDevice()
        {
            return contentManager?.Game.AudioDevice;
        }
    }
}
