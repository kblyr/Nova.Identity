using System.Reflection;

namespace Nova.Identity
{
    public static class CoreAssemblyMarker
    {
        public static readonly Assembly Assembly = typeof(CoreAssemblyMarker).Assembly; 
    }
}