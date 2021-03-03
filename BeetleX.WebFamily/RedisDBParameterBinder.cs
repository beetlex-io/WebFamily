using BeetleX.FastHttpApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.WebFamily
{
    public class RedisDBParameterBinder<T> : ParameterBinder
        where T : Redis.RedisDB, new()
    {

        public override bool DataParameter => false;

        public T RedisDB { get; set; } = new T();

        public override object DefaultValue()
        {
            return null;
        }

        public override object GetValue(IHttpContext context)
        {
            return RedisDB;
        }
    }
}
