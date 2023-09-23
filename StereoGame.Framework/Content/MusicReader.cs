namespace StereoGame.Framework.Content
{
    using StereoGame.Framework.Audio;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class MusicReader : AssetReader<Music>
    {
        public MusicReader()
            : base("Music Reader")
        {

        }

        public override Music? ReadAsset(string name, byte[]? data, object? parameter)
        {
            var ad = GetAudioDevice();
            if (ad != null)
            {
                if (data != null)
                {
                    return ad.LoadMusic(name, data);
                }
                else
                {
                    return ad.LoadMusic(name);
                }
            }
            return null;
        }
    }
}
