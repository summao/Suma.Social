using System;

namespace Suma.Social.Entities
{
    public class Comment : BaseEntity 
    {   
        public string Text { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}