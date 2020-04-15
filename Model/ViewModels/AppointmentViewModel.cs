﻿using System;

namespace api.reserveerme.nu.ViewModels
{
    public class AppointmentViewModel
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Location { get; set; }
    }
}