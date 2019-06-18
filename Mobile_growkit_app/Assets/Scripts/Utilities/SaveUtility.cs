using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utilities
{
    class SaveUtility
    {
        public static bool TryGetAuthToken(out string token)
        {
            token = PlayerPrefs.GetString("authToken");
            return string.IsNullOrEmpty(token);
        }
    }
}
