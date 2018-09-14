using Cwc.Common;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CWC.AutoTests.Helpers.Transport
{
    public class VisitMonitorHelper
    {
        DataBaseParams dbParams;
        public VisitMonitorHelper()
        {
            dbParams = new DataBaseParams();
        }
        public List<Dictionary<string, object>> GetResult(QueryResult query)
        {
            var parameters = DataBaseHelper.ConvertDictionaryParamsToSQLParamsWithSqlDbTypeCheck(query.Parameters);

            var res =  DataBaseHelper.Select(query.QueryText, parameters, dbParams);

            return ConvertToDictionary(res);
        }


        private SqlParameter[] ConvertParameters(Dictionary<string, object> input)
        {
            SqlParameter[] convertedParameters = new SqlParameter[input.Count()];

            for (int i = 0; i < convertedParameters.Length; i++)
            {
                var el = input.ElementAt(i);

                convertedParameters[i] = new SqlParameter(el.Key, el.Value);
            }

            return convertedParameters;
        }

        private List<Dictionary<string, object>> ConvertToDictionary(DataTable dt)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in dt.Rows)
            {
                var dict = new Dictionary<string, object>();

                foreach (DataColumn col in dt.Columns)
                {
                    dict.Add(col.ColumnName, row[col]);
                }

                list.Add(dict);
            }

            return list;
        }
    }
}
