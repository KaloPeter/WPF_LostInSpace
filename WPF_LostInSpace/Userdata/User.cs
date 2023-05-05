﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LostInSpace.Userdata
{
    public class User
    {
        public string Username { get; set; }
        public int Money { get; set; }
        public int BestDistance { get; set; }
        public int TotalDistance { get; set; }
        public DateTime LastLogin { get; set; }

        public double MusicVolume { get; set; }
        public double EffectVolume { get; set; }

    }
}