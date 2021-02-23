using System;
using Neo4j.Driver;

namespace Suma.Social.Entities
{
    public class Feed
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}