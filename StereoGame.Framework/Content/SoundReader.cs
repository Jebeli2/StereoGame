namespace StereoGame.Framework.Content
{
    using StereoGame.Framework.Audio;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class SoundReader : AssetReader<Sound>
    {
        public SoundReader()
            : base("Sound Reader")
        {

        }

        public override Sound? ReadAsset(string name, byte[]? data, object? parameter)
        {
            var ad = GetAudioDevice();
            if (ad != null)
            {
                if (data != null)
                {
                    return ad.LoadSound(name, data);
                }
                else
                {
                    return ad.LoadSound(name);
                }
            }
            return null;
        }
    }
}
