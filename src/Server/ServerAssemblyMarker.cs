using System.Reflection;

namespace Nova.Identity
{
    public static class ServerAssemblyMarker
    {
        public static readonly Assembly Assembly = typeof(ServerAssemblyMarker).Assembly;
    }
}