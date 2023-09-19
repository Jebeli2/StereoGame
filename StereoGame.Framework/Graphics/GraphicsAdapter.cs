using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework.Graphics
{
    public sealed class GraphicsAdapter : IDisposable
    {
        public enum DriverType
        {
            Hardware,
            Reference,
            FastSoftware
        }
        private static ReadOnlyCollection<GraphicsAdapter> adapters;

        public void Dispose()
        {
        }

        public static GraphicsAdapter DefaultAdapter
        {
            get { return Adapters[0]; }
        }

        public static ReadOnlyCollection<GraphicsAdapter> Adapters
        {
            get
            {
                if (adapters == null)
                {
                    adapters = new ReadOnlyCollection<GraphicsAdapter>(new[] { new GraphicsAdapter() });
                }
                return adapters;
            }
        }

        public static bool UseReferenceDevice
        {
            get { return UseDriverType == DriverType.Reference; }
            set { UseDriverType = value ? DriverType.Reference : DriverType.Hardware; }
        }

        public static DriverType UseDriverType { get; set; }

        public bool IsProfileSupported(GraphicsProfile graphicsProfile)
        {
            if (UseReferenceDevice)
                return true;

            switch (graphicsProfile)
            {
                case GraphicsProfile.Reach:
                    return true;
                case GraphicsProfile.HiDef:
                    bool result = true;
                    // TODO: check adapter capabilities...
                    return result;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
