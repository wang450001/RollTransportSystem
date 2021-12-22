using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Utils
{
    public class DataMapping
    {
        public static T GetObjectFromDataTable<T>(DataTable dt)
        {
            if (null == dt || null == dt.Rows || dt.Rows.Count < 1) return default(T);

            //反射类的对象
            object objRet = Activator.CreateInstance(typeof(T));
            //获取类的公共属性
            PropertyInfo[] pArray = objRet.GetType().GetProperties();

            foreach (PropertyInfo property in pArray)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.ColumnName.ToLower().Equals(property.Name.ToLower()))
                    {
                        if (!(dt.Rows[0][dc.ColumnName] is System.DBNull))
                            property.SetValue(objRet, dt.Rows[0][dc.ColumnName]);
                        break;
                    }
                }
            }

            return (T)objRet;
        }

        public static T GetObjectFromDataRow<T>(DataColumnCollection dcc, DataRow[] drs)
        {
            if (dcc == null || drs == null || dcc.Count < 1 || drs.Length < 1) return default(T);
            
            //反射类的对象
            object objRet = Activator.CreateInstance(typeof(T));
            //获取类的公共属性
            PropertyInfo[] pArray = objRet.GetType().GetProperties();

            foreach (PropertyInfo property in pArray)
            {
                foreach (DataColumn dc in dcc)
                {
                    if (dc.ColumnName.ToLower().Equals(property.Name.ToLower()))
                    {
                        if (!(drs[0][dc.ColumnName] is System.DBNull))
                            property.SetValue(objRet, drs[0][dc.ColumnName]);
                        break;
                    }
                }
            }

            return (T)objRet;
        }

        public static List<T> GetListFromDataTable<T>(DataTable dt)
        {
            if (null == dt || null == dt.Rows || dt.Rows.Count < 1) return null;

            List<T> listRet = new List<T>();

            foreach (DataRow dr in dt.Rows)
            {
                //反射类的对象
                object objRet = Activator.CreateInstance(typeof(T));
                //获取类的公共属性
                PropertyInfo[] pArray = objRet.GetType().GetProperties();
                foreach (PropertyInfo property in pArray)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.ColumnName.ToLower().Equals(property.Name.ToLower()))
                        {
                            if (!(dr[dc.ColumnName] is System.DBNull))
                                property.SetValue(objRet, dr[dc.ColumnName]);
                            break;
                        }
                    }
                }
                listRet.Add((T)objRet);
            }

            return listRet;
        }

        public static List<T> GetListFromDataRow<T>(DataColumnCollection dcc, DataRow[] drs)
        {
            if (dcc == null || drs == null || dcc.Count < 1 || drs.Length < 1) return null;

            List<T> listRet = new List<T>();

            foreach (DataRow dr in drs)
            {
                //反射类的对象
                object objRet = Activator.CreateInstance(typeof(T));
                //获取类的公共属性
                PropertyInfo[] pArray = objRet.GetType().GetProperties();
                foreach (PropertyInfo property in pArray)
                {
                    foreach (DataColumn dc in dcc)
                    {
                        if (dc.ColumnName.ToLower().Equals(property.Name.ToLower()))
                        {
                            if (!(dr[dc.ColumnName] is System.DBNull))
                                property.SetValue(objRet, dr[dc.ColumnName]);
                            break;
                        }
                    }
                }
                listRet.Add((T)objRet);
            }

            return listRet;
        }
    }
}
