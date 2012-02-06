using System;
using System.Collections.Generic;
using System.Windows.Media;
using WPF.RealTime.Data.Entities;

namespace FutureModule
{
    public class RuleEngine
    {
        private const string TwoDec = "{0:0.00}";
        private const string MaxTwoDec = "{0:0.##}";

        public static void ApplyRules(IEnumerable<Entity> entities)
        {
            ColouringRule(entities);

            //DisplayValueRule(entities);
        }

        private static void DisplayValueRule(IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                foreach (var item in entity.Values)
                {
                    if ((item.Value.Type == typeof(double)))
                    {
                        item.Value.DisplayValue = String.Format(TwoDec, item.Value.Value);
                    }
                }
            }
        }

        private static void ColouringRule(IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                foreach (var item in entity.Values)
                {
                    if ((item.Value.Type == typeof(double)) && (item.Value.Value != null))
                    {
                        if ((double)item.Value.Value > 0.5)
                        {
                            item.Value.Background = Brushes.LightBlue;
                        }
                        else
                        {
                            item.Value.Background = Brushes.White;
                        }
                    }
                }
            }
        }
    }
}
