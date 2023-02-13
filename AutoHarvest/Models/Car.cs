﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoHarvest.Models
{
    public enum ExtraInfo
    {
        Body,
        Transmission,
        Engine
    }

    public enum Source
    {
        Carsales,
        FbMarketplace,
        Gumtree
    }

    public class Car
    {
        public string Name { get; private set; }
        public string Link { get; private set; }
        public string ImgUrl { get; private set; }
        public int Price { get; private set; }
        public int KMs { get; private set; }
        public string[] ExtraInfo { get; private set; }
        public Source Source { get; private set; }

        // init
        public Car(string name, string link, string imgurl, int price, int kms, Source source, string[] extrainfo = null)
        {
            Name = name;
            Link = link;
            ImgUrl = imgurl;
            Price = price;
            KMs = kms;
            ExtraInfo = extrainfo;
            Source = source;
        }
    }
}
