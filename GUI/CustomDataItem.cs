using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using PracticalWork;

namespace GUI
{
    class CustomDataItem: IDataErrorInfo, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        double module;
        double xCoord;
        double yCoord;
        public V3DataCollection col;
        public CustomDataItem(V3DataCollection col)
        {
            this.col = col;
        }
        public double Module
        {
            get
            {
                return module;
            }
            set
            {
                module = Convert.ToDouble(value);
                PropertyChanged(this, new PropertyChangedEventArgs("Module"));
            }
        }
        public double XCoord
        {
            get
            {
                return xCoord;
            }
            set
            {
                xCoord = value;
                PropertyChanged(this, new PropertyChangedEventArgs("XCoord"));
            }
        }
        public double YCoord
        {
            get
            {
                return yCoord;
            }
            set
            {
                yCoord = value;
                PropertyChanged(this, new PropertyChangedEventArgs("yCoord"));
            }
        }
        public string Error
        {
            get
            {
                return "Wrong data.";
            }
        }
        public string this[string property]
        {
            get
            {
                string msg = String.Empty;
                if (property == "Module")
                {
                    if (module < 0)
                    {
                        msg = "Module should be more than 0.";
                    }
                }
                else if (property == "XCoord" || property == "YCoord")
                {
                    if (inCollection(xCoord, yCoord))
                    {
                        msg = "In collection";
                    }
                }
                return msg;
            }
        }
        private bool inCollection(double x, double y)
        {
            foreach(DataItem item in col)
            {
                if (item.Coord.X == x && item.Coord.Y == y)
                {
                    return true;
                }
            }
            return false;
        }
        public void AddCustom()
        {
            DataItem item = new DataItem(new System.Numerics.Vector2((float)xCoord, (float)yCoord), Module);
            col.Add(item);
        }
    }
}
