using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.NetworkingManager
{
    public interface INetworkingManager
    {
        public NetworkInfo NetworkInfo { get; set; }
        public abstract void init();

        public abstract void Update();
        public abstract void MenuUpdate();
        public abstract string ConnectionInfo();
        public abstract string CopyInfo();
    }
}
