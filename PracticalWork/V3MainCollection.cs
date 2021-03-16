using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.CompilerServices;

namespace PracticalWork
{
    [Serializable]
    public class V3MainCollection : IEnumerable<V3Data>, ISerializable, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private bool isChanged;
        [field: NonSerialized]
        private event DataChangedEventHandler DataChanged;
        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        public List<V3Data> list;
        public V3MainCollection()
        {
            list = new List<V3Data>();
            this.IsChanged = false;
            this.CollectionChanged += this.OnCollectionChanged;
            this.DataChanged += OnDataChanged;
        }
        public V3MainCollection(SerializationInfo info, StreamingContext context)
        {
            list = (List<V3Data>)info.GetValue("List", typeof(List<V3Data>));
            this.IsChanged = false;
            this.CollectionChanged += this.OnCollectionChanged;
            this.DataChanged += OnDataChanged;
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("List", this.list, typeof(List<V3Data>));
        }
        public V3Data this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                if (list[index] != value)
                {
                    DataChanged(this, new DataChangedEventArgs(ChangeInfo.Replace, list.Count.ToString() + " " + list.Count.ToString() + "\n"));
                    CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    list[index].PropertyChanged -= this.OnPropertyChanged;
                    list[index] = value;
                    list[index].PropertyChanged += this.OnPropertyChanged;
                }
            }
        }
        public int Count
        {
            get
            {
                return list.Count;
            }
        }
        public bool IsChanged
        {
            get
            {
                return isChanged;
            }
            set
            {
                isChanged = value;
                OnIsChangedChanged("IsChanged");
            }
        }
        public int MinNumberOfCalcs
        {
            get
            {
                var V3Grid = from item in list where item.GetType() == typeof(V3DataOnGrid) select item;
                var V3Collection = from item in list where item.GetType() == typeof(V3DataCollection) select item;
                var V3GridCount = from V3DataOnGrid item in V3Grid select item.Field.Length;
                var V3CollectionCount = from V3DataCollection item in V3Collection select item.List.Count;
                var V3GridCountMin = V3GridCount.Min();
                var V3GridCollectionMin = V3CollectionCount.Min();
                return (V3GridCountMin < V3GridCollectionMin) ? (V3GridCountMin) : (V3GridCollectionMin);
            }
        }
        public float MaxDistance
        {
            get
            {
                var V3Grid = from item in list where item.GetType() == typeof(V3DataOnGrid) select item;
                var V3Collection = from item in list where item.GetType() == typeof(V3DataCollection) select item;
                var V3GridMaxDistances = from V3DataOnGrid item in V3Grid select Vector2.Distance(new Vector2(item.Ox.Step * (item.Ox.Count - 1), item.Oy.Step * (item.Oy.Count - 1)), new Vector2(0, 0));
                var V3CollectionDistances = from V3DataCollection item in V3Collection from elem1 in item.List from elem2 in item.List select Vector2.Distance(new Vector2(elem1.Coord.X, elem1.Coord.Y), new Vector2(elem2.Coord.X, elem2.Coord.Y));
                float V3GridMaxDistance = V3GridMaxDistances.Max();
                float V3CollectionMaxDistance = V3CollectionDistances.Max();
                return (V3GridMaxDistance > V3CollectionMaxDistance) ? V3GridMaxDistance : V3CollectionMaxDistance;
            }
        }
        public IEnumerable<DataItem> GetMultiplePoints
        {
            get
            {
                var V3Grid = from item in list where item.GetType() == typeof(V3DataOnGrid) select item;
                var V3Collection = from item in list where item.GetType() == typeof(V3DataCollection) select item;
                var V3GridCollection = from V3DataOnGrid item in V3Grid select (V3DataCollection)item;

                var V3CollectionLists = from V3DataCollection item in V3Collection select item.List;
                var V3GridCollectionLists = from V3DataCollection item in V3GridCollection select item.List;

                var V3CollectionDataItems = from List<DataItem> item in V3CollectionLists from elem in item select elem;
                var V3GridCollectionDataItems = from List<DataItem> item in V3GridCollectionLists from elem in item select elem;

                var dataItems = V3CollectionDataItems.Concat(V3GridCollectionDataItems);

                var res = from item1 in dataItems from item2 in dataItems where item1.Coord == item2.Coord where item1.Module != item2.Module select item1;

                return res;
            }
        }
        public void Add(V3Data item)
        {
            list.Add(item);
            item.PropertyChanged += this.OnPropertyChanged;
            DataChanged(this, new DataChangedEventArgs(ChangeInfo.Add, (list.Count - 1).ToString() + " " + list.Count.ToString() + "\n"));
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        public void Save(string filename)
        {
            FileStream file = null; 
            BinaryFormatter fr;
            try
            {
                file = new FileStream(filename, FileMode.Create);
                fr = new BinaryFormatter();
                fr.Serialize(file, this.list);
                this.IsChanged = false;
            } catch {
            } finally
            {
                file.Close();
            }
        }
        public void Load(string filename)
        {
            FileStream file = null;
            BinaryFormatter fr;
            try
            {
                file = new FileStream(filename, FileMode.Open, FileAccess.Read);
                fr = new BinaryFormatter();
                List<V3Data> temp = (List<V3Data>)fr.Deserialize(file);
                this.list = temp;
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            } catch
            {
                throw new Exception();
            } finally
            {
                file.Close();
            }
        }
        public IEnumerator<V3Data> GetEnumerator()
        {
            for (int i = 0; i < Count; ++i)
            {
                yield return list[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
        public bool Remove(string id, DateTime date)
        {
            List<V3Data> temp = list.FindAll(x => id == x.Data && date == x.Time);
            foreach (V3Data item in temp)
            {
                item.PropertyChanged -= this.OnPropertyChanged;
            }
            int PrevCount = list.Count;
            if (list.RemoveAll(x => temp.Contains(x)) > 0)
            {
                DataChanged(this, new DataChangedEventArgs(ChangeInfo.Remove, PrevCount.ToString() + " " + list.Count.ToString() + "\n"));
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                return true;
            }
            return false;
        }
        public void AddDefaults()
        {
            V3DataOnGrid item =
                new V3DataOnGrid("Uniform grid; Axis X: step = 0.1, count = 11; Axis Y: step = 0.1; count = 11", DateTime.Now, new Grid1D(0.1f, 11), new Grid1D(0.1f, 11));
            item.InitRandom(0f, 10f);
            this.Add(item);

            V3DataOnGrid item1 =
                new V3DataOnGrid("Uniform grid; Axis X: step = 0.1, count = 11; Axis Y: step = 0.1; count = 11", DateTime.Now, new Grid1D(0.1f, 0), new Grid1D(0.1f, 0));
            item1.InitRandom(0f, 10f);
            this.Add(item1);

            V3DataCollection item3 = new V3DataCollection("Data", DateTime.Now);
            item3.InitRandom(0, 0.1f, 0.1f, 0.1, 0.1);
            this.Add(item3);

            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        public override string ToString()
        {
            string res = "";
            foreach (V3Data item in list)
            {
                res += item.ToLongString();
            }
            return res;
        }
        public string ToString(string format)
        {
            string res = "";
            foreach (V3Data item in list)
            {
                res += item.ToLongString(format);
            }
            return res;
        }
        public string Nearest(Vector2 vec)
        {
            string res = "";
            foreach (V3Data item in list)
            {
                Vector2[] near = item.Nearest(vec);
                foreach (Vector2 v in near)
                {
                    res += "The Nearest point is (" + v.X.ToString() + " " + v.Y.ToString() + ")\n";
                }
            }
            return res;
        }
        public void OnPropertyChanged(object source, PropertyChangedEventArgs args)
        {
            DataChanged(this, new DataChangedEventArgs(ChangeInfo.ItemChanged, list.Count.ToString() + " " + list.Count.ToString() + "\n"));
        }
        public void OnCollectionChanged(object source, NotifyCollectionChangedEventArgs args)
        {
            IsChanged = true;
        }
        private static void OnDataChanged(object source, DataChangedEventArgs args)
        {
            Console.WriteLine(args.ToString());
        }
        public void OnIsChangedChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
