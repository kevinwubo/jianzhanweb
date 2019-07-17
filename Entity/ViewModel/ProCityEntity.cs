﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.ViewModel
{


    public class TelephoneJsonInfo
    {
        public string resultcode { get; set; }

        public string reason { get; set; }

        public resultinfo result { get; set; }

        public int error_code { get; set; }
    }

    public class resultinfo
    {
        public string Province { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Isp { get; set; }

    }
    public class ProCityEntity
    {
        public string IpAddress { get; set; }
        public string province { get; set; }
        public string city { get; set; }
    }

    public class TelephoneJson
    {
        public string resultcode { get; set; }

        public string reason { get; set; }

        public result result { get; set; }

        public int error_code { get; set; }
    }

    public class result
    {
        public string province { get; set; }

        public string city { get; set; }

        public string areacode { get; set; }

        public string zip { get; set; }

        public string company { get; set; }

        public string card { get; set; }

    }
}
