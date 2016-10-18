using AutoMapper;
using MANOR.Core.Entities;
using MANOR.Infrastructure.DTOs;
using System;
using System.Globalization;

namespace MANOR.Infrastructure.Utilities
{
    public static class MapperConfig
    {
        public static void RegisterMapping()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Car, CarDto>().ReverseMap();
                cfg.CreateMap<Lap, LapDto>()
                    .ForMember(dest => dest.Timespan, opt => opt.MapFrom(src => Mapperx(src.Time)));

                cfg.CreateMap<LapDto, Lap>()
                    .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Timespan.ToString()));

                //.ReverseMap().ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Timespan.ToString()));
                cfg.CreateMap<Telemetry, TelemetryDto>().ReverseMap();

            });
        }

        private static TimeSpan Mapperx(string time)
        {
            TimeSpan timespan;
            TimeSpan.TryParseExact(time, @"mm\:ss\.f", CultureInfo.InvariantCulture, TimeSpanStyles.None, out timespan);

            //return timespan;
            return new TimeSpan(timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
            //return new TimeSpan(0, 0, timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
        }
    }
}
