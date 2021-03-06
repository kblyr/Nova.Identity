namespace Nova.Identity.Entities
{
    record Role
    {
        public int Id { get; init; }
        public string Name { get; init; } = "";
        public string? LookupKey { get; init; }
        public short? BoundaryId { get; init; }

        public Boundary? Boundary { get; init; }
    }
}