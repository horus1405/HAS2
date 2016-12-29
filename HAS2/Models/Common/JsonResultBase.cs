using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HAS2.Models.Common
{
    public class JsonResultBase
    {
        public bool result { get; set; }
        public string errormessage { get; set; }
        public string targeturl { get; set; }
        public object data { get; set; }

        public JsonResultBase()
        {
            result = true;
        }

        public JsonResultBase(object data, bool result, string errormessage, string targeturl)
        {
            this.data = data;
            this.result = result;
            this.errormessage = errormessage;
            this.targeturl = targeturl;
        }

    }

    public class JsonResultBase<T> : JsonResultBase
    {
        public T data { get; set; }

        public JsonResultBase()
        {
            result = true;
        }

        public JsonResultBase(T data, bool result, string errormessage)
        {
            this.data = data;
            this.result = result;
            this.errormessage = errormessage;            
        }

        public JsonResultBase(T data, bool result, string errormessage, string targeturl)
        {
            this.data = data;
            this.result = result;
            this.errormessage = errormessage;
            this.targeturl = targeturl;
        }

    }
}