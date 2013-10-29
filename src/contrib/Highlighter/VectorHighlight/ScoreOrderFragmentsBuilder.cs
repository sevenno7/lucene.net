/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Lucene.Net.Support;
using System;
using System.Collections.Generic;
using System.Text;
using WeightedFragInfo = Lucene.Net.Search.VectorHighlight.FieldFragList.WeightedFragInfo;

namespace Lucene.Net.Search.VectorHighlight
{
    /*
    * An implementation of FragmentsBuilder that outputs score-order fragments.
    */
    public class ScoreOrderFragmentsBuilder : BaseFragmentsBuilder
    {
        /// <summary>
        /// a constructor.
        /// </summary>
        public ScoreOrderFragmentsBuilder()
            : base()
        {
        }

        /// <summary>
        /// a constructor.
        /// </summary>
        /// <param name="preTags">array of pre-tags for markup terms</param>
        /// <param name="postTags">array of post-tags for markup terms</param>
        public ScoreOrderFragmentsBuilder(String[] preTags, String[] postTags)
            : base(preTags, postTags)
        {
        }

        public ScoreOrderFragmentsBuilder(IBoundaryScanner bs)
            : base(bs)
        {
        }

        public ScoreOrderFragmentsBuilder(String[] preTags, String[] postTags, IBoundaryScanner bs)
            : base(preTags, postTags, bs)
        {
        }

        public override IList<WeightedFragInfo> GetWeightedFragInfoList(IList<WeightedFragInfo> src)
        {
            // .NET implementation as IList lacks .Sort
            List<WeightedFragInfo> asList = src as List<WeightedFragInfo>;

            if (asList != null)
            {
                asList.Sort(new ScoreComparator());
                return asList;
            }
            else
            {
                asList = new List<WeightedFragInfo>(src);
                asList.Sort(new ScoreComparator());
                return asList;
            }
        }

        public class ScoreComparator : IComparer<WeightedFragInfo>
        {  
            public int Compare(WeightedFragInfo o1, WeightedFragInfo o2)
            {
                if (o1.TotalBoost > o2.TotalBoost) return -1;
                else if (o1.TotalBoost < o2.TotalBoost) return 1;
                // if same score then check startOffset
                else
                {
                    if (o1.StartOffset < o2.StartOffset) return -1;
                    else if (o1.StartOffset > o2.StartOffset) return 1;
                }
                return 0;
            }
        }
    }

}
