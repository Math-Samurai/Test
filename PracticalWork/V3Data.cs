using System;
using System.Numerics;
using System.ComponentModel;
using System.IO;
using System.Globalization;
using System.Runtime.Serialization;

namespace PracticalWork
{
    [Serializable]
    public abstract class V3Data : INotifyPropertyChanged, ISerializable
    {
        string data;
        DateTime time;
        public event PropertyChangedEventHandler PropertyChanged;
        public V3Data(string data, DateTime time)
        {
            Data = data;
            Time = time;
        }
        public V3Data(string filename)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filename);
                string temp;
                temp = sr.ReadLine();
                if (temp != null)
                {
                    string[] bases = temp.Split();
                    Data = bases[0];
                    Time = DateTime.ParseExact(bases[1], "dd-MM-yyyy", new CultureInfo("ru-ru"));
                }
            }
            catch
            {
                Console.WriteLine("Error.\n");
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
        }
        public V3Data(SerializationInfo info, StreamingContext context)
        {
            data = (string)info.GetValue("Data", typeof(string));
            time = (DateTime)info.GetValue("Time", typeof(DateTime));
        }
        public string Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Data"));
                }

            }
        }
        public DateTime Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Time"));
                }
            }
        }
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Data", data);
            info.AddValue("Time", time);
        }
        public abstract Vector2[] Nearest(Vector2 v);
        public abstract string ToLongString();
        public abstract string ToLongString(string format);
        public override string ToString()
        {
            return Data + "\n" + Time.ToString();
        }
    }
}
