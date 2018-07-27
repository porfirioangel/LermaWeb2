﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cuenca_conagua.src.Entidades;
using LumenWorks.Framework.IO.Csv;

namespace Cuenca_conagua.src.Utilidades
{
    public class CsvDataReader
    {
        public static List<Dictionary<string, string>> ReadCsv(string filename)
        {
            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();

            using (CsvReader csv = new CsvReader(new StreamReader(filename), true))
            {
                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();

                Logger.AddToLog("Csv filename: " + filename, true);
                Logger.AddToLog("Csv fields: " + fieldCount, true);
                Logger.AddToLog("Csv headers: " + string.Join(", ", headers), true);

                while (csv.ReadNextRecord())
                {
                    string[] record = new string[fieldCount];
                    csv.CopyCurrentRecordTo(record);
                    Logger.AddToLog("Csv row: " + string.Join(", ", record), true);

                    data.Add(new Dictionary<string, string>());

                    for (int i = 0; i < fieldCount; i++)
                        data[data.Count - 1].Add(headers[i], record[i]);
                }
            }

            return data;
        }

        public static List<PrecipitacionMedia> ReadPrecipitacionMediaCsv(string filename)
        {
            List<Dictionary<string, string>> data = ReadCsv(filename);
            Logger.AddToLog(DictListToJsonString(data), true);

            List<PrecipitacionMedia> precs = new List<PrecipitacionMedia>();

            foreach (Dictionary<string, string> prec in data)
            {
                PrecipitacionMedia p = new PrecipitacionMedia();
                p.Ciclo = prec["ciclo"];
                p.Nov = double.Parse(prec["nov"]);
                p.Dic = double.Parse(prec["dic"]);
                p.Ene = double.Parse(prec["ene"]);
                p.Feb = double.Parse(prec["feb"]);
                p.Mar = double.Parse(prec["mar"]);
                p.Abr = double.Parse(prec["abr"]);
                p.May = double.Parse(prec["may"]);
                p.Jun = double.Parse(prec["jun"]);
                p.Jul = double.Parse(prec["jul"]);
                p.Ago = double.Parse(prec["ago"]);
                p.Sep = double.Parse(prec["sep"]);
                p.Oct = double.Parse(prec["oct"]);
                precs.Add(p);
            }

            return precs;
        }

        private static string DictListToJsonString(List<Dictionary<string, string>> obj)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\n[\n    ");

            foreach (Dictionary<string, string> o in obj)
            {
                IEnumerable<string> enumerable = o.Select(
                    d => string.Format("\"{0}\": {1}", d.Key, d.Value));
                string oJson = "{\n        " + string.Join(",\n        ", 
                    enumerable) + "\n    }";
                sb.Append(oJson).Append(",\n    ");
            }

            sb.Remove(sb.Length - 6, 6);
            sb.Append("\n]");
            return sb.ToString();
        }
    }
}