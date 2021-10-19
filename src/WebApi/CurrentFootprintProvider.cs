using CodeCompanion.EntityFrameworkCore;

namespace Nova.Identity
{
    sealed class CurrentFootprintProvider : ICurrentFootprintProvider
    {
        public Footprint Current { get; }

        public CurrentFootprintProvider()
        {
            Current = new(0, DateTimeOffset.Now);
        }
    }
}