using AutoCivilization.Abstractions;

namespace AutoCivilization.TechnologyResolvers
{
    public class OrdinalSuffixResolver : IOrdinalSuffixResolver
    {
        public string GetOrdinalSuffix(int num)
        {
            string number = num.ToString();
            if (number.EndsWith("11")) return "th";
            if (number.EndsWith("12")) return "th";
            if (number.EndsWith("13")) return "th";
            if (number.EndsWith("1")) return "st";
            if (number.EndsWith("2")) return "nd";
            if (number.EndsWith("3")) return "rd";
            return "th";
        }

        public string GetOrdinalSuffixWithInput(int num)
        {
            return $"{num}{GetOrdinalSuffix(num)}";
        }
    }
}
