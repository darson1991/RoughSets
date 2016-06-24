using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Helpers
{
    public class StringValueToNumberMapper
    {
        private readonly Dictionary<string, int> _stringToNumberMapper;

        public int ColumnIndex { get; private set; }
        public int HighestMapperNumber => _stringToNumberMapper.Count > 0 
                                                ? _stringToNumberMapper.Max(n => n.Value)
                                                : -1;

        public StringValueToNumberMapper(int columnIndex)
        {
            ColumnIndex = columnIndex;
            _stringToNumberMapper = new Dictionary<string, int>();
        }

        public bool ContainsDictionaryKey(string key)
        {
            return _stringToNumberMapper.ContainsKey(key);
        }

        public int GetValue(string key)
        {
            if(ContainsDictionaryKey(key))
                return _stringToNumberMapper[key];

            throw new Exception("String to number mapper does not contain key: " + key);
        }

        public void AddValue(string key, int value)
        {
            _stringToNumberMapper.Add(key, value);
        }
    }
}
