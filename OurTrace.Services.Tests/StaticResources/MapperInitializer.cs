using AutoMapper;
using OurTrace.App.Automapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace OurTrace.Services.Tests.StaticResources
{
    public static class MapperInitializer
    {
        private static MapperConfiguration MapperConfiguration = new MapperConfiguration(cfg =>{cfg.AddProfile<MapperProfile>();});
        public static IMapper CreateMapper()
        {
            return MapperConfiguration.CreateMapper();
        }
    }
}
