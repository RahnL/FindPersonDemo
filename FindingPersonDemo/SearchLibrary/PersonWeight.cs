/* 
 * Finding a Person demonstration project
 * 
 * Released under the MIT license
 * Copyright (c) 2013 Rahn Lieberman, http://RahnsWorld.com
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchLibrary
{
    class PersonWeight
    {
        private Dictionary<int, int> list = new Dictionary<int, int>();
        internal int ValidSearch = 0;
        internal int InvalidSearch = 0;

        internal void Clear()
        {
            list.Clear();
        }

        /// <summary>
        /// Add a new Person ID to the list
        /// </summary>
        /// <param name="PersonID"></param>
        internal string Add(int PersonID)
        {
            if (PersonID > 0)
            {
                if (!list.ContainsKey(PersonID))
                {
                    list.Add(PersonID, 1);
                }
                else
                {
                    list[PersonID]++;   //Increase count by one
                }

                return PersonID.ToString();
            }
            return "not found";
        }

        internal int Weigh()
        {
            int maxValue = 0;
            int maxPerson = -1;

            foreach (KeyValuePair<int, int> kvp in list)
            {
                if (kvp.Value > maxValue)
                {
                    maxValue = kvp.Value;
                    maxPerson = kvp.Key;
                }
            }
            if (maxValue > 1)   //ensure more than a singe match, to avoid a single true being counted as correct.
                return maxPerson;
            else
                return -1;
        }

        private int GetFirstPerson(PersonFunctions ps)
        {
            //int result = ps.First.PersonID;
            //if (ps.First.Name.ToLower().Contains("duplicate"))
            //{
            //    foreach (PersonSearchEDI p in ps)
            //    {
            //        if (!ps.First.Name.ToLower().Contains("duplicate"))
            //        {
            //            result = p.PersonID;
            //            break;
            //        }
            //    }
            //}
            return 1;
        }
    }
}
