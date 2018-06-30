using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace DAL.Tools
{
    public class JsonHelper
    {
        #region 把对象序列化 JSON 字符串
        public static string ModelConvertoJson<T>(T model)
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                json.WriteObject(ms, model);
                string szJson = Encoding.UTF8.GetString(ms.ToArray());
                return szJson;
            }
        }
        #endregion

        #region 把JSON字符串还原为对象
        public static T JsonConvertoModel<T>(string szJson)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
            {
                DataContractJsonSerializer dcj = new DataContractJsonSerializer(typeof(T));
                return (T)dcj.ReadObject(ms);
            }
        }
        #endregion

        #region 表转换成Jason
        public static string DataTableToJSON(System.Data.DataTable table)
        {
            StringBuilder json = new StringBuilder();
            json.Append("[");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];

                // json object
                json.Append("{");
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    string columnName = table.Columns[j].ColumnName.ToLower();
                    string columnType = table.Columns[j].DataType.Name;

                    // json field
                    if (columnType == "Int32" || columnType == "Int16" || columnType == "Decimal")
                    {
                        // don't surround numbers with quotes
                        json.AppendFormat("\"{0}\":\"{1}\"", columnName, row.IsNull(columnName) ? "" : row[columnName]);
                    }
                    else if (columnType == "Boolean")
                    {
                        // make the bool value lowercase
                        json.AppendFormat("\"{0}\":{1}", columnName.ToLower(), row.IsNull(columnName) ? "" : row[columnName].ToString().ToLower());
                    }
                    else
                    {
                        // everything else gets quotes around the data
                        json.AppendFormat("\"{0}\":\"{1}\"", columnName, row[columnName]);
                    }

                    if (j < table.Columns.Count - 1) json.Append(","); // add comma if not last column
                }
                json.Append("}");

                if (i < table.Rows.Count - 1) json.Append(","); // add comma if not last row
            }
            json.Append("]");
            return json.ToString().Replace("\r","\\r").Replace("\n","\\n");
        }
        #endregion

        #region 表转换成Jason
        public static string DataViewToJSON(DataView view)
        {
            StringBuilder json = new StringBuilder();
            json.Append("[");
            for (int i = 0; i < view.Count; i++)
            {
                DataRowView row = view[i];

                // json object
                json.Append("{");
                for (int j = 0; j < view.Table.Columns.Count; j++)
                {
                    string columnName = view.Table.Columns[j].ColumnName.ToLower();
                    string columnType = view.Table.Columns[j].DataType.Name;

                    // json field
                    if (columnType == "Int32" || columnType == "Int16" || columnType == "Decimal")
                    {
                        // don't surround numbers with quotes
                        json.AppendFormat("\"{0}\":\"{1}\"", columnName, row[columnName]);
                    }
                    else if (columnType == "Boolean")
                    {
                        // make the bool value lowercase
                        json.AppendFormat("\"{0}\":{1}", columnName.ToLower(), row[columnName].ToString().ToLower());
                    }
                    else
                    {
                        // everything else gets quotes around the data
                        json.AppendFormat("\"{0}\":\"{1}\"", columnName, row[columnName]);
                    }

                    if (j < view.Table.Columns.Count - 1) json.Append(","); // add comma if not last column
                }
                json.Append("}");

                if (i < view.Count - 1) json.Append(","); // add comma if not last row
            }
            json.Append("]");
            return json.ToString();
        }
        #endregion

        #region Table to Obj
        public static List<T> ConvertTableToObj<T>(DataTable dt) where T : new()
        {
            Type type = typeof(T);
            List<T> objs = new List<T>();
            var properties = type.GetProperties();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                T o = new T();
                foreach (var p in properties)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].ColumnName == p.Name)
                        {
                            if (!string.IsNullOrEmpty(dt.Rows[i][j].ToString()))
                            {
                                p.SetValue(o, dt.Rows[i][j]);
                            }
                            break;
                        }
                    }
                }
                objs.Add(o);
            }
            return objs;
        }
        #endregion
    }
}
