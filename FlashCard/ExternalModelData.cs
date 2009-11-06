using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlashCard
{
    class ExternalModelData
    {
        public Dictionary<int, ExternalModel> organModelDictionary = new Dictionary<int, ExternalModel> { };

        public ExternalModelData(List<ExternalModel> organs)
        {
            foreach (ExternalModel each in organs)
            {
                organModelDictionary.Add(each.tag, each);
            }
        
        }

        public ExternalModel GetOrganFromTag(byte tagVal)
        {
            return GetOrganFromTag((int)tagVal);
        }

        //needs a liiiitle more error checking.
        public ExternalModel GetOrganFromTag(int tagVal)
        {
            if (organModelDictionary.ContainsKey(tagVal))
            {
                return organModelDictionary[tagVal];
            }
            else
            {
                return null;
            }
        }

    }
}
