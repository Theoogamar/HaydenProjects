﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoHarvest.Singletons;
using AutoHarvest.HelperFunctions;
using AutoHarvest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.ComponentModel.DataAnnotations;
using AutoHarvest.Models.Json;
using Microsoft.Extensions.Options;

namespace AutoHarvest.Pages
{
    public class IndexModel : PageModel
    {
        #region Url Params

        // the make of the car (car brand)
        [Required]
        [MaxLength(25)]
        [BindProperty(SupportsGet = true)]
        public string Make { get; set; }

        // the model of the car (car brand's model)
        [MaxLength(25)]
        [BindProperty(SupportsGet = true)]
        public string Model { get; set; }

        // the search term
        [Required]
        [MaxLength(100)]
        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }

        // the way the car listings are sorted
        [Required]
        [BindProperty(SupportsGet = true)]
        public int Sort { get; set; }

        // the car transmission
        [Required]
        [BindProperty(SupportsGet = true)]
        public int Trans { get; set; }

        // the minimum price
        [MaxLength(10)]
        [BindProperty(SupportsGet = true)]
        public string Min { get; set; }

        // the maxium price
        [MaxLength(10)]
        [BindProperty(SupportsGet = true)]
        public string Max { get; set; }

        // the toggles for websites to scrape
        [Required]
        [BindProperty(SupportsGet = true)]
        public bool Carsales { get; set; } = true;

        [Required]
        [BindProperty(SupportsGet = true)]
        public bool FbMarketplace { get; set; } = true;

        [Required]
        [BindProperty(SupportsGet = true)]
        public bool Gumtree { get; set; } = true;

        // the page's number
        [BindProperty(SupportsGet = true)]
        public int PageNum { get; set; } = 1;
        #endregion

        // page number properties
        public bool ShowPrevious => PageNum > 1;
        public bool ShowNext => PageNum < 10 && Make != null && Model != null;

        // the icons for all the extra info
        public static readonly string[] Icons = new string[3] { "fa fa-car", "fa fa-cog", "fa fa-wrench" };

        // the websites title
        public string Title;

        public IEnumerable<Car> Cars { get; set; }

        public readonly CarWrapper CarWrapper;

        public readonly CarLookup CarLookup;

        public readonly CarFinder CarFinder;

        private readonly ILogger<IndexModel> Logger;

        public IndexModel(CarWrapper carwrapper, CarLookup carlookup, IOptions<CarFinder> carfinder, ILogger<IndexModel> logger, Events events)
        {
            CarWrapper = carwrapper;
            CarLookup = carlookup;
            CarFinder = carfinder.Value;
            Logger = logger;
            Title = events.GetTitle();
            Cars = new List<Car>();
        }

        public async Task OnGet()
        {
            try
            {
                // get cars on make model or search
                if (Make != null && Model != null)
                {
                    // log activity
                    Logger.LogInformation("CarFinder: {ipaddress} {url}", Request.HttpContext.Connection.RemoteIpAddress, Request.QueryString);

                    FilterOptions filterOptions = new FilterOptions(Make, Model, Search, Sort, Trans, Min, Max,
                        Carsales, FbMarketplace, Gumtree, PageNum);

                    // get the car listings
                    Cars = await CarWrapper.GetCarsAsync(filterOptions);
                }
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "CarFinder has ran into a fatally error. QueryString: {url}", Request.QueryString);
                Response.Redirect("error");
            }
        }
    }
}
