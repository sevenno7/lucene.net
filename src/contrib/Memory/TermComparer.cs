/*
 *
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 *
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lucene.Net.Index.Memory
{
    internal class TermComparer
    {
        /// <summary>
        /// Sorts term entries into ascending order; also works for
        /// Arrays.binarySearch() and Arrays.sort()
        /// </summary>
        public static int KeyComparer<TKey, TValue>(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
            where TKey : class, IComparable<TKey>
        {
            if (x.Key == y.Key) return 0;
            return typeof (TKey) == typeof (string)
                       ? string.Compare(x.Key as string, y.Key as string, StringComparison.Ordinal)
                       : x.Key.CompareTo(y.Key);
        }
    }

    sealed class TermComparer<T> : TermComparer, IComparer<KeyValuePair<string, T>>, IComparer
    {
        public int Compare(KeyValuePair<string, T> x, KeyValuePair<string, T> y)
        {
            return KeyComparer(x, y);
        }

        int IComparer.Compare(object x, object y)
        {
            return KeyComparer((KeyValuePair<string, T>)x, (KeyValuePair<string, T>)y);
        }
    }
}
