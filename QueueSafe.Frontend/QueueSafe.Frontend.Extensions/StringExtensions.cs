namespace QueueSafe.Frontend.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsAll(this string s, string b)
        {
            bool ContainsAll = true;

            foreach(var word in b.Trim().Split(" "))
            {
                if(!s.Contains(word)) ContainsAll = false;
            }

            return ContainsAll;
        } 
    }
}