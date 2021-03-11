using System;
using System.Collections.Generic;
using Suma.Social.Utils;

namespace Suma.Social.Entities
{
    public class BaseEntity
    {
        public string Id { get; set; }
        public Dictionary<string, object> AsDictionary()
        {
            var properties = this.GetType().GetProperties();
            var dics = new Dictionary<string, object>();
            Array.ForEach(properties, a =>
                dics.Add(StringUtil.ToLowerInvariantFirstLetter(a.Name), a.GetValue(this, null))
            );
            return dics;
        }
    }
}