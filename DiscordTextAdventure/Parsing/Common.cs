using System;
using System.Collections.Generic;

#nullable enable
namespace DiscordTextAdventure.Parsing
{
    public static class Common
    {
        public static readonly char[] SEPERATORS = {' ', '-', '_', '\t', '\n'};
        
        /// <summary>
        /// takes all members of type T in static class and returns as array
        /// </summary>
        /// <param name="encompassingClass"> static class to check in</param>
        /// <typeparam name="TFieldToFind"> to return to array</typeparam>
        /// <returns></returns>
        public static TFieldToFind[] ClassMembersToArray<TFieldToFind>(Type encompassingClass)
        {
            List<TFieldToFind> members = new List<TFieldToFind>();
            var fieldInfos =encompassingClass.GetFields();
            for (int i = 0; i < fieldInfos.Length; i++)
            {
                var m = fieldInfos[i];
                if (fieldInfos[i].FieldType == typeof(TFieldToFind))
                {
                    var value = fieldInfos[i].GetValue(null);
                    members.Add((TFieldToFind) value);
                }
            }

            return members.ToArray();
        }
    }
    
    
}