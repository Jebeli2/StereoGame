using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework.Content
{
    public interface IAssetReader
    {
        string Name { get; }
        ContentManager? ContentManager { get; set; }
        object? ReadAsset(string name, byte[]? data, object? parameter);
    }

    public interface IAssetReader<T> : IAssetReader
    {
        new T? ReadAsset(string name, byte[]? data, object? parameter = null);
    }


}
