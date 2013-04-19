using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using WR.Blog.Data.Models;
using WR.Blog.Mvc.Areas.SiteAdmin.Models;

using AutoMapper;

namespace WR.Blog.Mvc
{
    public class AutoMapperConfig
    {
        public static void CreateMaps()
        {
            Mapper.CreateMap<BlogVersionDto, BlogPostDto>().ForMember(b => b.Id, opt => opt.Ignore());
            Mapper.CreateMap<BlogPostDto, BlogVersionDto>().ForMember(b => b.Id, opt => opt.Ignore());

            Mapper.CreateMap<BlogPostDto, BlogPost>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.DisplayName));
            Mapper.CreateMap<BlogPost, BlogPostDto>();

            Mapper.CreateMap<BlogVersionDto, BlogVersion>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.DisplayName))
                .ForMember(dest => dest.VersionOfId, opt => opt.MapFrom(src => src.VersionOf.Id));
            Mapper.CreateMap<BlogVersion, BlogVersionDto>();

            Mapper.CreateMap<SiteSettingsDto, SiteSettingsDto>();
        }
    }
}