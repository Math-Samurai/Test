using System;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;
using System.Runtime.Serialization;

namespace PracticalWork
{
    [Serializable]
    public class V3DataOnGrid : V3Data, IEnumerable<DataItem>
    {
        public V3DataOnGrid(string data, DateTime time, Grid1D ox, Grid1D oy) : base(data, time)
        {
            Ox = ox;
            Oy = oy;
        }
        public V3DataOnGrid(SerializationInfo info, StreamingContext context): base(info, context)
        {
            Ox = (Grid1D)info.GetValue("Ox", typeof(Grid1D));
            Oy = (Grid1D)info.GetValue("Oy", typeof(Grid1D));
            Field = (double[,])info.GetValue("Field", typeof(double[,]));
        }
        public Grid1D Ox
        {
            get; set;
        }
        public Grid1D Oy
        {
            get; set;
        }
        public double[,] Field
        {
            get; set;
        }
        public static explicit operator V3DataCollection(V3DataOnGrid obj)
        {
            V3DataCollection res = new V3DataCollection(obj.Data, obj.Time);
            for (int i = 0; i < obj.Ox.Count; ++i)
            {
                for (int j = 0; j < obj.Oy.Count; ++j)
                {
                    Vector2 vec = new Vector2(i * obj.Ox.Step, j * obj.Oy.Step);
                    res.Add(new DataItem(vec, obj.Field[i, j]));
                }
            }
            return res;
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Ox", Ox);
            info.AddValue("Oy", Oy);
            info.AddValue("Field", Field);
        }
        public void InitRandom(double minValue, double maxValue)
        {
            Field = new double[Ox.Count, Oy.Count];
            Random rnd = new Random();
            for (int i = 0; i < Ox.Count; ++i)
            {
                for (int j = 0; j < Oy.Count; ++j)
                {
                    Field[i, j] = minValue + rnd.NextDouble() * (maxValue - minValue);
                }
            }
        }
        public override Vector2[] Nearest(Vector2 v)
        {
            List<Vector2> points = new List<Vector2>();
            double min_distance = Double.MaxValue;
            for (int i = 0; i < Ox.Count; ++i)
            {
                for (int j = 0; j < Oy.Count; ++j)
                {
                    Vector2 point = new Vector2(i * Ox.Step, j * Oy.Step);
                    double distance = Vector2.Distance(v, point);
                    int res = distance.CompareTo(min_distance);
                    if (res == 0)
                    {
                        points.Add(point);
                    }
                    else if (res < 0)
                    {
                        min_distance = distance;
                        points.Clear();
                        points.Add(point);
                    }
                }
            }
            return points.ToArray();
        }
        public override string ToString()
        {
            return "V3DataOnGrid\n" + base.ToString() + "\nOx: " + Ox.ToString() + "Oy: " +
                   Oy.ToString();
        }
        public override string ToLongString()
        {
            string res = this.ToString();
            for (int i = 0; i < Ox.Count; ++i)
            {
                for (int j = 0; j < Oy.Count; ++j)
                {
                    res += "Field(" + (i * Ox.Step).ToString() + " " + (j * Oy.Step).ToString() +
                           ") = " + Field[i, j].ToString() + "\n";
                }
            }
            return res + "\n";
        }
        public override string ToLongString(string format)
        {
            string res = this.ToString();
            for (int i = 0; i < Ox.Count; ++i)
            {
                for (int j = 0; j < Oy.Count; ++j)
                {
                    res += "Field(" + (i * Ox.Step).ToString(format) + " " + (j * Oy.Step).ToString(format) +
                           ") = " + Field[i, j].ToString(format) + "\n";
                }
            }
            return res + "\n";
        }
        public IEnumerator<DataItem> GetEnumerator()
        {
            for (int i = 0; i < Ox.Count; ++i)
            {
                for (int j = 0; j < Oy.Count; ++j)
                {
                    DataItem item = new DataItem(new Vector2(Ox.Step * i, Oy.Step * j), Field[i, j]);
                    yield return item;
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
