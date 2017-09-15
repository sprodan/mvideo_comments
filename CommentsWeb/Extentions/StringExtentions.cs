using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentsWeb.Extentions
{
    public static class StringExtentions
    {
        public static Dictionary<string, string> DeserializeAjaxString(this string str)
        {
            return str.Split('&')
					.Select(value => value.Split('='))
					.ToDictionary(pair => pair[0], pair => pair[1]);
        }
    }
}
