using System;

namespace EasyPeasy.Models.ViewModels
{
    public class ReportesViewModel
    {
        public string text{get;set;}
        public int value{get;set;}
        public string color{get;set;}
        public int total { get; set; }
        public int totalPorTransportista { get; set; }
    }
}