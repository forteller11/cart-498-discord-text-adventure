using System;
using System.Collections.Generic;
using chext.Math;

namespace DiscordTextAdventure
{
    public static class Common
    {
        /// <summary>
        /// takes all members of type T in static class and returns as array
        /// </summary>
        /// <param name="encompassingClass"> type of class to check in</param>
        /// <param name="parentObject"> leave null if static class</param>
        /// <typeparam name="TFieldToFind"> to return to array</typeparam>
        /// <returns></returns>
        public static TFieldToFind[] ClassMembersToArray<TFieldToFind>(Type encompassingClass, object parentObject)
        {
            List<TFieldToFind> members = new List<TFieldToFind>();
            var fieldInfos =encompassingClass.GetFields();
            for (int i = 0; i < fieldInfos.Length; i++)
            {
                var m = fieldInfos[i];
                if (fieldInfos[i].FieldType == typeof(TFieldToFind))
                {
                    var value = fieldInfos[i].GetValue(parentObject);
                    members.Add((TFieldToFind) value);
                }
            }

            return members.ToArray();
        }

    }
}