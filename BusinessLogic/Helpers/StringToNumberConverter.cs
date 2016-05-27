using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using BusinessLogic.Exceptions;

namespace BusinessLogic.Helpers
{
    [SuppressMessage("ReSharper", "LoopCanBePartlyConvertedToQuery")]
    public class StringToNumberConverter
    {
        private List<StringValueToNumberMapper> _stringToNumberMappersList;

        public StringToNumberConverter()
        {
            _stringToNumberMappersList = new List<StringValueToNumberMapper>();
        }

        public void ConvertStringsToNumbers(string[] lines)
        {
            try
            {
                InitializeStringToNumberMappersList(lines[0]);
                FillStringToNumbereMappersDictionaries(lines);

                for (var i = 0; i < lines.Length; i++)
                {
                    var argumentsValuesCollection = lines[i].Split(',');
                    foreach (var mapper in _stringToNumberMappersList)
                    {
                        argumentsValuesCollection[mapper.ColumnIndex] =
                            mapper.GetValue(argumentsValuesCollection[mapper.ColumnIndex]).ToString();
                    }
                    lines[i] = string.Join(",", argumentsValuesCollection);
                }
            }
            catch (Exception exception)
            {
                throw new ConvertStringToNumberException(exception);
            }
        }

        private void InitializeStringToNumberMappersList(string line)
        {
            _stringToNumberMappersList = new List<StringValueToNumberMapper>();
            var dataCollection = line.Split(',');
            for (var i = 0; i < dataCollection.Length; i++)
            {
                double number;
                if (!double.TryParse(dataCollection[i].Replace('.', ','), out number))
                    _stringToNumberMappersList.Add(new StringValueToNumberMapper(i));
            }
        }

        private void FillStringToNumbereMappersDictionaries(string[] lines)
        {
            foreach (var mapper in _stringToNumberMappersList)
            {
                var index = mapper.ColumnIndex;
                foreach (var line in lines)
                {
                    var argumentsValuesCollection = line.Split(',');
                    if (!mapper.ContainsDictionaryKey(argumentsValuesCollection[index].Trim()))
                        mapper.AddValue(argumentsValuesCollection[index], mapper.HighestMapperNumber + 1);
                }
            }
        }
    }
}
