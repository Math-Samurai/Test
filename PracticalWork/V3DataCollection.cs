using System;
using System.IO;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PracticalWork
{
    [Serializable]
    public class V3DataCollection : V3Data, IEnumerable<DataItem>
    {
        public V3DataCollection(string data, DateTime time) : base(data, time)
        {
            List = new List<DataItem>();
        }
        public V3DataCollection(string filename) : base(filename)
        {
            StreamReader sr = null;
            try
            {
                string temp;
                sr = new StreamReader(filename);
                List = new List<DataItem>();
                sr.ReadLine();
                while ((temp = sr.ReadLine()) != null)
                {
                    string[] nums = temp.Split((char)Consts.separator);
                    DataItem item = new DataItem();
                    item.Coord = new Vector2(float.Parse(nums[0]), float.Parse(nums[1]));
                    item.Module = float.Parse(nums[2]);
                    List.Add(item);
                }
            }
            catch
            {
                if (List != null)
                {
                    List.Clear();
                }
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
        }
        public V3DataCollection(SerializationInfo info, StreamingContext context): base(info, context)
        {
            List = (List<DataItem>)info.GetValue("List", typeof(List<DataItem>));
        }
        public List<DataItem> List
        {
            get; set;
        }
        public void Add(DataItem item)
        {
            List.Add(item);
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("List", List);
        }
        public void InitRandom(int nItems, float xmax, float ymax, double minValue, double maxValue)
        {
            Random rand = new Random();
            for (int i = 0; i < nItems; ++i)
            {
                DataItem item = new DataItem(new Vector2((float)rand.NextDouble() * xmax,
                                             (float)rand.NextDouble() * ymax),
                                             minValue + rand.NextDouble() * (maxValue - minValue));
                List.Add(item);
            }
        }
        public override Vector2[] Nearest(Vector2 v)
        {
            List<Vector2> points = new List<Vector2>();
            double min_distance = Double.MaxValue;
            foreach (DataItem item in List)
            {
                Vector2 point = new Vector2(item.Coord.X, item.Coord.Y);
                double distance = Vector2.Distance(v, point);
                int res = distance.CompareTo(min_distance);
                if (res < 0)
                {
                    min_distance = distance;
                    points.Clear();
                    points.Add(point);
                }
                else if (res == 0)
                {
                    points.Add(point);
                }
            }
            return points.ToArray();
        }
        public override string ToString()
        {
            return "V3DataCollection\n" + base.ToString() + "\nLength = " + List.Count.ToString() +
                   "\n";
        }
        public override string ToLongString()
        {
            string res = this.ToString();
            for (int i = 0; i < List.Count; ++i)
            {
                res += "Field(" + List[i].Coord.X.ToString() + " " + List[i].Coord.Y.ToString() +
                       ") = " + List[i].Module + "\n";
            }
            return res + "\n";
        }
        public override string ToLongString(string format)
        {
            string res = this.ToString();
            for (int i = 0; i < List.Count; ++i)
            {
                res += "Field(" + List[i].Coord.X.ToString(format) + " " + List[i].Coord.Y.ToString(format) +
                       ") = " + List[i].Module.ToString(format) + "\n";
            }
            return res + "\n";
        }
        public IEnumerator<DataItem> GetEnumerator()
        {
            for (int i = 0; i < List.Count; ++i)
            {
                yield return List[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
