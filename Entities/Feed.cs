using System;

namespace Suma.Social.Entities
{
    public class Feed : BaseEntities
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public DateTimeOffset Created { get; set; }

        public string PrivacyLevel { get; set; }

    }
}