﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoHarvest.Models
{
    public enum SortTypes
    {
        PriceLowtoHigh,
        PriceHightoLow,
        Recent,
        KilometresLowtoHigh,
        KilometresHightoLow
    }

    public enum TransTypes
    {
        All,
        Manuel,
        Automatic
    }

    public struct FilterOptions
    {
        public string SearchTerm { get; private set; }
        public int SortType { get; private set; }
        public int TransType { get; private set; }
        public string PriceMin { get; private set; }
        public string PriceMax { get; private set; }

        public FilterOptions(string searchterm, int sorttype, int transtype, string pricemin, string pricemax)
        {
            SearchTerm = searchterm;
            SortType = sorttype;
            TransType = transtype;
            PriceMin = pricemin;
            PriceMax = pricemax;
        }
    }

    public struct Toggles
    {
        public bool ToggleCarsales { get; set; }
        public bool ToggleFBMarketplace { get; set; }
        public bool ToggleGumtree { get; set; }

        public Toggles(bool togglecarsales, bool toggleFBmarketplace, bool togglegumtree)
        {
            ToggleCarsales = togglecarsales;
            ToggleFBMarketplace = toggleFBmarketplace;
            ToggleGumtree = togglegumtree;
        }
    }
}
