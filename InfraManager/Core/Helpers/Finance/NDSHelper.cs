using System;
using InfraManager.Core.Extensions;

namespace InfraManager.Core.Helpers.Finance
{
    public sealed class NDSValues
    {
        private NDSValues() { }

        public decimal CostWithoutNDS { get; private set; } // Стоимость без НДС
        public decimal CostWithNDS { get; private set; } // Стоимость с учетом НДС
        public decimal SumNDS { get; private set; } // Сумма НДС
        public decimal PriceWithoutNDS { get; private set; } // Цена без НДС
        public decimal PriceWithNDS { get; private set; } // Цена с учетом НДС

        public static NDSValues Create(decimal costWithoutNDS, decimal costWithNDS, decimal sumNDS, decimal priceWithoutNDS, decimal priceWithNDS)
        {
            var retval = new NDSValues();
            //
            retval.CostWithoutNDS = costWithoutNDS;
            retval.CostWithNDS = costWithNDS;
            retval.SumNDS = sumNDS;
            retval.PriceWithNDS = priceWithNDS;
            retval.PriceWithoutNDS = priceWithoutNDS;
            //
            return retval;
        }
    }

    public static class NDSHelper
    {
        public static NDSValues CalculatePriceWithNDS(decimal price, int count, NDSType type, NDSPercent ndsPercent, decimal? ndsCustomValue)
        {
            decimal costWithoutNDS = 0; // Стоимость без НДС
            decimal sumNDS = 0; // Сумма НДС
            decimal costWithNDS = 0; // Стоимость с учетом НДС
            //
            decimal priceWithoutNDS = 0; // Цена за единицу без НДС 
            decimal ndsPrice = 0; // Сумма НДС за единицу
            decimal priceWithNDS = 0; // Цена за единицу с учетом НДС
            //
            switch (type)
            {
                case NDSType.AlreadyIncluded:
                    if (ndsPercent == NDSPercent.Custom)
                    {
                        var value = ndsCustomValue.HasValue ? ndsCustomValue.Value : 0;
                        //
                        priceWithoutNDS = (price - value).To2digits();
                        ndsPrice = value.To2digits();
                        priceWithNDS = price.To2digits();
                    }
                    else
                    {
                        var percent = GetNdsPercent(ndsPercent);
                        //
                        priceWithoutNDS = (price / (1 + percent)).To2digits();
                        ndsPrice = (price - priceWithoutNDS).To2digits();
                        priceWithNDS = price.To2digits();
                    }
                    break;
                case NDSType.AddToPrice:
                    if (ndsPercent == NDSPercent.Custom)
                    {
                        var value = ndsCustomValue.HasValue ? ndsCustomValue.Value : 0;
                        //
                        ndsPrice = value.To2digits();
                        priceWithNDS = (price + ndsPrice).To2digits();
                        priceWithoutNDS = price.To2digits();
                    }
                    else
                    {
                        var percent = GetNdsPercent(ndsPercent);
                        //
                        ndsPrice = (price * percent).To2digits();
                        priceWithNDS = (price + ndsPrice).To2digits();
                        priceWithoutNDS = price.To2digits();
                    }
                    break;
                case NDSType.NotNeeded:
                    ndsPrice = 0;
                    priceWithoutNDS = price.To2digits();
                    priceWithNDS = price.To2digits();
                    break;
                default: throw new ArgumentOutOfRangeException(@"NDSType enum");
            }
            //
            costWithoutNDS = (priceWithoutNDS * count).To2digits();
            sumNDS = (ndsPrice * count).To2digits();
            costWithNDS = (priceWithNDS * count).To2digits();
            //
            return NDSValues.Create(costWithoutNDS, costWithNDS, sumNDS, priceWithoutNDS, priceWithNDS);
        }

        public static decimal GetNdsPercent(NDSPercent ndsPercent)
        {
            switch (ndsPercent)
            {
                case NDSPercent.Custom:
                    return 0;
                case NDSPercent.Seven:
                    return new decimal(0.07);
                case NDSPercent.Ten:
                    return new decimal(0.1);
                case NDSPercent.Eighteen:
                    return new decimal(0.18);
                case NDSPercent.Twenty:
                    return new decimal(0.20);
                default: throw new ArgumentOutOfRangeException(@"NDSPercent enum");
            }
        }
    }
}
