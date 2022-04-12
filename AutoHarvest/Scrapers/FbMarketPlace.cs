﻿using AutoHarvest.HelperFunctions;
using AutoHarvest.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoHarvest.Singleton;

namespace AutoHarvest.Scrapers
{
    // the class that webscrapes facebook.com/marketplace
    public static class FbMarketplace
    {
        // the urls for scraping
        private const string site = "https://www.facebook.com/marketplace/";
        private static readonly string[] trans = { "", "&transmissionType=manual", "&transmissionType=automatic" };
        private static readonly string[] sort = { "price_ascend", "price_descend", "best_match", "best_match", "best_match" };

        // webscrape FbMarketplace for all the listings
        public async static Task<List<Car>> ScrapeFbMarketplace(Func<string, Task<string>> GetHtmlAsync, FilterOptions filterOptions, int page)
        {
            // requres to save the page per user and activate js
            if (page > 1)
                return new List<Car>();
            
            // Initializing the html doc
            HtmlDocument htmlDocument = new HtmlDocument();

            // format the price range input field
            string minPrice = filterOptions.PriceMin == "" ? filterOptions.PriceMin : $"&minPrice={filterOptions.PriceMin}";
            string maxPrice = filterOptions.PriceMax == "" ? filterOptions.PriceMax : $"&maxPrice={filterOptions.PriceMax}";

            // get the HTML doc of website
            string url = $"{site}sydney/search?sortBy={sort[filterOptions.SortType]}{minPrice}{maxPrice}{trans[filterOptions.TransType]}&query={filterOptions.SearchTerm}&category_id=vehicles&exact=false";
            string html = await GetHtmlAsync(url);

            // Load HTML doc
            htmlDocument.LoadHtml(html);

            // gets the longest string in the list
            string script = htmlDocument.DocumentNode.Descendants("script").Aggregate("", (max, cur) => max.Length > cur.InnerText.Length ? max : cur.InnerText);

            // arrays to find and store the data, 0: url, 1: imgUrl, 2: price, 3: title, 4: kms
            string[] keyWords = { "GroupCommerceProductItem\",\"id\":", "image\":{\"uri\":", "amount\":\"", "listing_title\":", "subtitle\":" };
            string[] texts = { "", "", "", "", "" };

            var carItems = new List<Car>();

            // exstract the usefull text from the script
            int idx = 0;
            while (idx != -1)
            {
                for (int i = 0; i < keyWords.Length; i++)
                {
                    // find the next sub string in the string
                    idx = script.IndexOf(keyWords[i], idx);

                    // if reach the end of the string return
                    if (idx == -1)
                        return carItems;

                    // walk forwards through the string to construct the text
                    idx += keyWords[i].Length;
                    string text = "";
                    while (script[idx + 1] != '"')
                    {
                        if (script[++idx] != '\\')
                            text += script[idx];
                    }

                    // add the text to the array
                    texts[i] = text;
                }

                // add them all to the list
                carItems.Add(new Car(texts[3], site + "item/" + texts[0], texts[1], texts[2].ToInt(), texts[4].ToInt() * 1000, new string[0], "FbMarketplace"));
            }

            return carItems;
        }
    }
}
