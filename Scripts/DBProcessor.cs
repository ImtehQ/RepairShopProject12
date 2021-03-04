using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace RepairShopProject12.Scripts
{
    public class WhereTypeCase
    {
        public WhereType type;
        public string propertieName;
        public bool isFilled()
        {
            if (propertieName != null && propertieName.Length > 0)
                return true;
            return false;
        }
    }
    public enum WhereType
    {
        and,or    
    }
    public static class DBProcessor
    {
        /// <summary>
        /// Creates a table enterie of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Data"></param>
        /// <param name="DatabaseTable"></param>
        /// <param name="autoIncrease"></param>
        /// <param name="IdString"></param>
        /// <returns></returns>
        public static int Create<T>(T Data, string DatabaseTable, bool autoIncrease = true, List<string> excludeStrings = null, string IdString = "Id")
        {
            string databaseString = $"insert into dbo.{DatabaseTable} ";
            Type t = Data.GetType();
            PropertyInfo[] props = t.GetProperties();

            if (autoIncrease)
            {
                var pid = props.First(p => p.Name == IdString);

                pid.SetValue(Data, Count(Data, DatabaseTable));
            }

            databaseString += "(";
            for (int i = 0; i < props.Length; i++)
            {
                if (excludeStrings != null)
                {
                    if (excludeStrings.Contains(props[i].Name)) { continue; }
                }

                databaseString += props[i].Name;
                if (i < props.Length - 1)
                    databaseString += ", ";
            }
            if (databaseString.Substring(databaseString.Length - 2, 1) == ",")
                databaseString = databaseString.Substring(0, databaseString.Length - 2);
            databaseString += ") values (";
            for (int i = 0; i < props.Length; i++)
            {
                if (excludeStrings != null)
                {
                    if (excludeStrings.Contains(props[i].Name)) { continue; }
                }

                databaseString += $"'{props[i].GetValue(Data)}'";

                if (i < props.Length - 1)
                    databaseString += ", ";
            }

            if (databaseString.Substring(databaseString.Length - 2, 1) == ",")
                databaseString = databaseString.Substring(0, databaseString.Length - 2);
            databaseString += ")";

            return SqlDataAccess.ExecuteDataString(databaseString);
        }

        /// <summary>
        /// Update single enterie in the table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Data"></param>
        /// <param name="DatabaseTable"></param>
        /// <param name="wherePropertie"></param>
        /// <param name="excludeString"></param>
        /// <returns></returns>
        public static int Update<T>(T Data, string DatabaseTable, List<string> excludeStrings = null, string wherePropertie = "Id", bool excludeDefaultString = true)
        {
            if (excludeDefaultString)
                if (excludeStrings != null)
                    excludeStrings.Add("Id");

            string databaseString = $"update dbo.{DatabaseTable} set ";
            Type t = Data.GetType();
            PropertyInfo[] props = t.GetProperties();

            for (int i = 0; i < props.Length; i++)
            {
                if (excludeStrings != null)
                {
                    if (excludeStrings.Contains(props[i].Name)) { continue; }
                }

                if (props[i].PropertyType.Name == "String")
                {
                    if (props[i].GetValue(Data) != null && props[i].GetValue(Data).ToString().Length > 0)
                    {
                        databaseString += $"{props[i].Name}='{props[i].GetValue(Data)}'";
                    }
                    else
                    {
                        databaseString += $"{props[i].Name}=''";
                    }
                }
                else
                    databaseString += $"{props[i].Name}={props[i].GetValue(Data)}";

                if (i < props.Length - 1)
                    databaseString += ", ";
            }

            PropertyInfo whereCase = props.First(p => p.Name == wherePropertie);

            if (databaseString.Substring(databaseString.Length - 2, 1) == ",")
                databaseString = databaseString.Substring(0, databaseString.Length - 2);

            if (whereCase != null)
            {
                databaseString += $" where {wherePropertie} = {whereCase.GetValue(Data)}";
            }

            return SqlDataAccess.ExecuteDataString(databaseString);
        }

        /// <summary>
        /// Deletes from table where case
        /// </summary>
        /// <param name="DatabaseTable"></param>
        /// <param name="whereValue"></param>
        /// <param name="wherePropertie"></param>
        /// <returns></returns>
        public static int Delete(string DatabaseTable, string whereValue, string wherePropertie = "Id")
        {
            string databaseString = "delete from dbo." + DatabaseTable;
            databaseString += " where " + wherePropertie + "=" + whereValue;

            return SqlDataAccess.ExecuteDataString(databaseString);
        }

        /// <summary>
        /// Sql command formatter tool
        /// </summary>
        /// <param name="DatabaseTable"></param>
        /// <param name="wheres"></param>
        /// <param name="excludeStrings"></param>
        /// <returns></returns>
        private static string formatString<T>(T data, string DatabaseTable, List<WhereTypeCase> wheres = null, List<string> excludeStrings = null, bool checkData = true)
        {
            if(checkData){
                if (wheres.Where(w => w.isFilled() == false) != null)
                    return "Where case error!";}

            return "";
        }

        /// <summary>
        /// Returns everything out of the database from this table
        /// Include type and it will auto convert.
        /// doing it wrong and breaking it is your own fault
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="DatabaseTable"></param>
        /// <returns></returns>
        public static List<T> ListEverything<T>(string DatabaseTable)
        {
            return SqlDataAccess.LoadData<T>($"select * from dbo.{DatabaseTable}");
        }

        /// <summary>
        /// Returns list of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Data"></param>
        /// <param name="DatabaseTable"></param>
        /// <returns></returns>
        public static List<T> ListAll<T>(T Data, string DatabaseTable, List<string> excludeStrings = null)
        {
            string databaseString = "select ";
            Type t = Data.GetType();
            PropertyInfo[] props = t.GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                if(excludeStrings!= null && excludeStrings.Count > 0)
                {
                    if (excludeStrings.Contains(props[i].Name))
                        continue;
                }

                databaseString += props[i].Name;
                if (i < props.Length - 1)
                    databaseString += ", ";
            }

            if (databaseString.Substring(databaseString.Length - 2, 2) == ", ")
                databaseString = databaseString.Substring(0, databaseString.Length - 2);
            databaseString += $" from dbo.{DatabaseTable}";


            return SqlDataAccess.LoadData<T>(databaseString);
        }

        /// <summary>
        /// Counts stuffs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Data"></param>
        /// <param name="DatabaseTable"></param>
        /// <returns></returns>
        public static int Count<T>(T Data, string DatabaseTable)
        {
            return ListAll<T>(Data, DatabaseTable).Count();
        }
        public static int Count<T>(string DatabaseTable)
        {
            return SqlDataAccess.LoadData<T>($"select * from dbo.{DatabaseTable}").Count();
        }

        /// <summary>
        /// Loads 1 of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Data"></param>
        /// <param name="DatabaseTable"></param>
        /// <param name="wherePropertie"></param>
        /// <returns></returns>
        public static T Details<T>(T Data, string DatabaseTable, string wherePropertie = "Id")
        {
            string databaseString = "select ";
            Type t = Data.GetType();
            PropertyInfo[] props = t.GetProperties();

            for (int i = 0; i < props.Length; i++)
            {
                if (wherePropertie == "Id" && props[i].Name == "Id")
                    return OneById<T>((int)props[i].GetValue(Data), DatabaseTable);

                databaseString += props[i].Name;
                if (i < props.Length - 1)
                    databaseString += ", ";
            }

            databaseString += $" from dbo.{DatabaseTable}";

            PropertyInfo whereCase = props.First(p => p.Name == wherePropertie);

            if (whereCase != null)
                databaseString += $" where {wherePropertie}={whereCase.GetValue(Data)}";

            return SqlDataAccess.LoadData<T>(databaseString).FirstOrDefault();
        }

        /// <summary>
        /// Load 1 of type T by ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <param name="DatabaseTable"></param>
        /// <param name="wherePropertie"></param>
        /// <returns></returns>
        public static T OneById<T>(int Id, string DatabaseTable, string wherePropertie = "Id")
        {
            return SqlDataAccess.LoadData<T>($"select * from dbo.{DatabaseTable} where {wherePropertie}={Id}").FirstOrDefault();
        }


        /// <summary>
        /// Converts type From(F) => type To(T)
        /// </summary>
        /// <typeparam name="F"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="From"></param>
        /// <param name="To"></param>
        /// <returns></returns>
        public static void ConvertFromTo<F, T>(F From, T To)
        {
            Type Tt = To.GetType();
            PropertyInfo[] Tprops = Tt.GetProperties();

            Type Ft = To.GetType();
            PropertyInfo[] Fprops = Ft.GetProperties();

            foreach (PropertyInfo Fprop in Fprops)
            {
                foreach (PropertyInfo Tprop in Tprops)
                {
                    if (Fprop.Name == Tprop.Name)
                    {
                        Tprop.SetValue(Tt,
                            Fprop.GetValue(Ft));
                    }
                }
            }
        }
        public static T ConvertFromToReturn<F, T>(F From, T To)
        {
            Type Tt = To.GetType();
            PropertyInfo[] Tprops = Tt.GetProperties();

            Type Ft = From.GetType();
            PropertyInfo[] Fprops = Ft.GetProperties();

            foreach (PropertyInfo Fprop in Fprops)
            {
                foreach (PropertyInfo Tprop in Tprops)
                {
                    if (Fprop.Name == Tprop.Name)
                    {
                        Tprop.SetValue(Tt,
                            Fprop.GetValue(Ft));
                    }
                }
            }

            return To;
        }

        public static List<T> ConvertFromToReturn<F, T>(this List<F> From, T ToType)
        {
            List<T> ts = new List<T>();
            foreach (var item in From)
            {
                ts.Add(ConvertFromToReturn<F, T>(item, ToType));
            }
            return ts;
        }
    }
}