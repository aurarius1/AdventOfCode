using System.Diagnostics;
using System;


namespace _2025._03
{
    public sealed class Day03 : Base
    {

        private Dictionary<(string, int), string> cache = new();
        private int _length;
        public Day03(bool example) : base(example)
        {
            Day = "03";
        }

        private string GetMaxJoltage(string bank, int remaining)
        {

            var key = (bank, remaining);
            if(cache.TryGetValue(key, out string value)){
                return value;
            }
            if (remaining == 0)
            {
                return "";
            }

            if(bank.Length == remaining){
                cache[key] = bank;
                return bank;
            }

            string remainingBank = bank[1..];
            string active1 = bank[0] + GetMaxJoltage(remainingBank, remaining-1);
            string active2 = GetMaxJoltage(remainingBank, remaining);

            if(ulong.Parse(active1) >= ulong.Parse(active2))
            {
                cache[key] = active1;
                return active1;
            }

            cache[key] = active2;
            return active2;
        }


        public override object PartOne()
        {
            string[] input = ReadInput();
            ulong joltage = 0;
            foreach(string bank in input)
            {
                cache.Clear();
                joltage += ulong.Parse(GetMaxJoltage(bank, 2));
            }
            return joltage.ToString();
        }

        public override object PartTwo()
        {
            string[] input = ReadInput();
            ulong joltage = 0;
            
            foreach(string bank in input)
            {
                cache.Clear();
                joltage += ulong.Parse(GetMaxJoltage(bank, 12));
            }
            return joltage.ToString();
        }
    }
}
