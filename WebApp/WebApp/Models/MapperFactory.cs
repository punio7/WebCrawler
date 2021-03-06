﻿using System;
using System.Collections.Generic;
using AutoMapper;
using WebCrawler.WebApp.DbModel.Models;
using WebCrawler.WebApp.WebApp.Models.Apps;

namespace WebCrawler.WebApp.WebApp.Models
{
    public static class MapperFactory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Parametr potrzebny do delegaty rejestracji serwisów")]
        public static IMapper CreateMapper(IServiceProvider serviceProvider)
        {
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.CreateMap<AppDefinition, AppViewModel>();
                config.CreateMap<ProcessSession, SessionViewModel>();
                config.CreateMap<ProcessSession, SessionRoomViewModel>();

                config.CreateMap<IEnumerable<AppDefinition>, ListAppsViewModel>()
                    .ForMember(vm => vm.Apps, options => options.MapFrom(dbm => dbm));
                config.CreateMap<IEnumerable<ProcessSession>, ListSessionsViewModel>()
                    .ForMember(vm => vm.Sessions, options => options.MapFrom(dbm => dbm));

            });

            return mapperConfiguration.CreateMapper();
        }
    }
}
