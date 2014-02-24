using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlexAddin.NUnit
{
    internal static class PermutationExtensions
    {
        public static bool IsPermutable(this object o)
        {
            Type t = o as Type;
            if (t != null)
            {
                return t == typeof(bool) || t.IsEnum;
            }

            return (o is Array);
        }
    }

    internal class PermutationProvider
    {
        public static List<List<object>> CreatePermutations(object[] arguments)
        {
            List<List<object>> permutations = new List<List<object>>();
            
            for(int i=0; i<arguments.Length; i++)
            {
                var set = PermuteArg(arguments[i]);

                if (i == 0)
                {
                    foreach (var arg in set)
                        permutations.Add(new List<object>() { arg });
                }
                else
                    AddPermutations(permutations, set);                
            }

            return permutations;
        }

        private static void AddPermutations(List<List<object>> permutations, object[] set)
        {
            int count = permutations.Count;
            for (int i = 0; i < count;)
            {
                var toCopy = permutations[i].ToList();

                for (int j=0; j<set.Length; j++)
                {
                    var newPerm = toCopy.ToList();
                    newPerm.Add(set[j]);

                    if (j == 0)
                        permutations[i] = newPerm;
                    else
                    {
                        permutations.Insert(i + j, newPerm);
                        count++;
                    }
                }

                i += set.Length;
            }
        }

        private static object[] PermuteArg(object arg)
        {
            List<object> permutations = new List<object>();

            Type typeToPermutate = arg as Type;
            Array arrayPermutations = arg as Array;

            if (typeToPermutate != null)
            {
                if (typeToPermutate.IsEnum)
                {
                    foreach (object val in Enum.GetValues(typeToPermutate))
                        permutations.Add(val);
                }
                else if (typeToPermutate == typeof(bool))
                {
                    permutations.Add(false);
                    permutations.Add(true);
                }
                else
                    permutations.Add(arg);
            }
            else if (!(arg is string) && arrayPermutations != null)
            {
                foreach (var obj in arrayPermutations)
                    permutations.Add(obj);
            }
            else
                permutations.Add(arg);

            return permutations.ToArray();
        }
    }
}
