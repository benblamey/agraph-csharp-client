using System;
using System.Data;
using System.Linq;

namespace AllegroGraphCSharpClient
{
    /// <summary>
    /// Class to return a clean datatable for the query made
    /// </summary>
    public class JSONUtility
    {
        public System.Data.DataTable ReturnCleanSemanticResults(string QueryResults)
        {
            string[] QuerySeparatorLeftBracket = { "]" };
            string[] QuerySeparatorRightBracket = { "[" };
            string[] QuerySeparatorComma = { "," };
            string[] QuerySeparatorSubString = { "[\"" };
            string[] Individuals = QueryResults.Split(QuerySeparatorLeftBracket,StringSplitOptions.None);
            string[] substrings = Individuals[0].Split(QuerySeparatorRightBracket, StringSplitOptions.None);
            DataColumn dc = new DataColumn();
            DataTable dt = new DataTable();
            // build the data columns from the json array
            string[] stringToUse = substrings[1].Split(QuerySeparatorComma,StringSplitOptions.None);
            
            string[] stringToUse2;
            for (int i2 = 0; i2 <= stringToUse.Length - 1; i2++)
            {
                string columnName = stringToUse[i2].Replace("\"", "");
                dc = new DataColumn(columnName.Replace("\"", ""), System.Type.GetType("System.String"));
                dt.Columns.Add(dc);
            }
            string[] delimiter1 = {('"' + (",\\" + '"'))};
            //  now start handling the actual values
            if ((Individuals.Length > 1))
            {
                string[] subStrings2 = Individuals[1].Split(QuerySeparatorSubString,StringSplitOptions.None);
                string[] tempstring2 = subStrings2[subStrings2.Count()-1].Split(delimiter1, 4, StringSplitOptions.RemoveEmptyEntries);
                stringToUse2 = tempstring2[tempstring2.Count() - 1].Split(QuerySeparatorComma, StringSplitOptions.RemoveEmptyEntries);
                DataRow dr;
                dr = dt.NewRow();
                for (int i3 = 1; (i3 <= stringToUse.Length); i3++)
                {
                    string tempstring;
                    tempstring = stringToUse2[(i3 - 1)].Replace("^^<http://www.w3.org/2001/XMLSchema#string>\"", "").Replace("\\\"", "").Replace("\"", "");
                    dr[(i3 - 1)] = tempstring.Replace("value:", "");
                }
                dt.Rows.Add(dr);
                /// go until you finish reading the 3rd item from the end of the json array
                /// since the last two items are useless
                for (int i = 2; (i <= (Individuals.Length - 3)); i++)
                {
                    string[] subStrings3 = Individuals[i].Split(QuerySeparatorSubString,StringSplitOptions.None);
                    string[] tempstring3 = subStrings3[subStrings3.Count() - 1].Split(delimiter1, 4, StringSplitOptions.RemoveEmptyEntries);
                    stringToUse = tempstring3[tempstring3.Count() - 1].Split(QuerySeparatorComma, 4, StringSplitOptions.RemoveEmptyEntries);
                    dr = dt.NewRow();
                    for (int i3 = 0; (i3
                                <= (dt.Columns.Count - 1)); i3++)
                    {
                        dr[i3] = stringToUse[i3].Replace("^^<http://www.w3.org/2001/XMLSchema#string>\"", "").Replace("\\\"", "").Replace("\"", "");
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }


    }
}
