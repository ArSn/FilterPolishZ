﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilterCore.Constants;
using FilterEconomy.Model;
using FilterEconomy.Model.ItemAspects;
using FilterPolishUtil.Collections;

namespace FilterEconomy.Request.Enrichment
{
    public class LowestPriceEnrichment : IDataEnrichment
    {
        public string DataKey => "LowestPrice";

        public void Enrich(string baseType, ItemList<NinjaItem> data)
        {
            List<NinjaItem> target = data;

            if (data.Count > 1)
            {
                var filteredData = data.Where(x => x.Aspects.All(z => !FilterConstants.IgnoredLowestPriceAspects.Contains(z.Name))).ToList();
                if (filteredData.Count >= 1)
                {
                    target = filteredData;
                }
            }

            var price = target.Min(x => x.CVal);
            data.LowestPrice = price;
        }
    }
}
