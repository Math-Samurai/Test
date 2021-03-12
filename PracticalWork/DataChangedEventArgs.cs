using System;
using System.Collections.Generic;
using System.Text;

namespace PracticalWork
{
    class DataChangedEventArgs
    {
        public DataChangedEventArgs(ChangeInfo change, string s)
        {
            cInfo = change;
            str = s;
        }
        public ChangeInfo cInfo
        {
            get; set;
        }
        public string str
        {
            get; set;
        }
        public override string ToString()
        {
            return cInfo.ToString() + " " + str;
        }
    }
}
