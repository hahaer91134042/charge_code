using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DevLibs
{
    public static class AttrExt
    {
        public static T copyRowFrom<T>(this T ori,object from)
        {
            try
            {
                var fromProps = from.getAttrProperty<DbRowKey>();
                var oriProps = ori.getAttrProperty<DbRowKey>();
                foreach(var prop in oriProps)
                {
                    var oriAttr = prop.getAttribute<DbRowKey>();
                    var formProp = fromProps.FirstOrDefault(p
                        => p.getAttribute<DbRowKey>().Key == oriAttr.Key);

                    //沒有相對應的Row 跳過
                    if (formProp == null) continue;

                    var value = formProp.GetValue(from);
                    prop.SetValue(ori, value);
                }

                return ori;
            }catch(Exception e)
            {
                Log.e("copyRowFrom error", e);
            }
            return default(T);
        }

        #region MapString
        public static string toSymbolString(this MapString.Root root)
        {
            var symbol = root.mapSymbol();
            var builder = new StringBuilder();

            foreach (var prop in root.GetType().GetProperties())
            {
                var attr = prop.getAttribute<MapString>();
                if (attr == null)
                    continue;

                var valueStr = $"{attr.key}{attr.subSymbol}{prop.GetValue(root)}".Trim();

                builder.Append($"{valueStr}{symbol}");
            }
            builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }
        public static T toMapObj<T>(this string input) where T : MapString.Root
        {
            var obj = Activator.CreateInstance<T>();
            var dataPair = input.Split(obj.mapSymbol().ToCharArray());

            //Log.d($"str->{input} data->{dataPair.toJsonString()}");

            foreach (var prop in obj.GetType().GetProperties())
            {
                var attr = prop.getAttribute<MapString>();
                if (attr == null)
                    continue;
                var key = attr.key;
                var subSymbol = attr.subSymbol.ToCharArray();


                if (subSymbol.Length < 1)
                {
                    var value = dataPair[attr.index];
                    //Log.d($"arr value->{value}");
                    prop.SetValue(obj, Convert.ChangeType(value, prop.PropertyType));
                }
                else
                {
                    var digest = (from sub in dataPair.Select(s => new { key = s.Split(subSymbol)[0], value = s.Split(subSymbol)[1] })
                                  where sub.key == key
                                  select sub).FirstOrDefault();
                    //Log.d($"digest data digest->{digest.toJsonString()}");
                    if (digest == null)
                        continue;

                    prop.SetValue(obj, Convert.ChangeType(digest.value, prop.PropertyType));
                }
            }

            return obj;
        }
        #endregion
    }
}