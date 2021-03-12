using Microsoft.VisualBasic;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Serialization;

namespace PracticalWork
{    
    enum Consts
    {
        separator = ' '
    }
    enum ChangeInfo
    {
        ItemChanged,
        Add,
        Remove,
        Replace
    }
    delegate void DataChangedEventHandler(object source, DataChangedEventArgs args);
    [Serializable]
    public struct DataItem: ISerializable
    {
        public DataItem(Vector2 coord, double module)
        {
            Coord = coord;
            Module = module;
        }
        public Vector2 Coord
        {
            get; set;
        }
        public DataItem(SerializationInfo info, StreamingContext context)
        {
            float x = (float)info.GetValue("X", typeof(float));
            float y = (float)info.GetValue("Y", typeof(float));
            Coord = new Vector2(x, y);
            Module = (double)info.GetValue("Module", typeof(double));
        }

        public double Module
        {
            get; set;
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("X", Coord.X);
            info.AddValue("Y", Coord.Y);
            info.AddValue("Module", Module);
        }
        public override string ToString()
        {
            return "Coords = (" + Coord.X.ToString() + ", " + Coord.Y.ToString() + "); Module = " +
                   Module.ToString();
        }
        public string ToLongString(string format)
        {
            return "Coords = (" + Coord.X.ToString(format) + ", " + Coord.Y.ToString(format) + "); Module = " +
                    Module.ToString(format);
        }
    }
    [Serializable]
    public struct Grid1D: ISerializable
    {
        public Grid1D(float step, int count)
        {
            Step = step;
            Count = count;
        }
        public Grid1D(SerializationInfo info, StreamingContext context)
        {
            Step = (float)info.GetValue("Step", typeof(float));
            Count = (int)info.GetValue("Count", typeof(int));
        }
        public float Step
        {
            get; set;
        }
        public int Count
        {
            get; set;
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Step", Step);
            info.AddValue("Count", Count);
        }
        public override string ToString()
        {
            return "Step = " + Step.ToString() + "; Count = " + Count.ToString() + "\n";
        }
        public string ToString(string format)
        {
            return "Step = " + Step.ToString(format) + "; Count = " + Count.ToString(format) + "\n";
        }
    }
}