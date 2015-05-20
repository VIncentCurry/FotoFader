using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoTaggerOM
{
    public class FacebookTimeOutException : Exception
    {
        public override string Message
        {
            get
            {
                return "There was an error trying to access Facebook data.";
            }
        }
    }
}
