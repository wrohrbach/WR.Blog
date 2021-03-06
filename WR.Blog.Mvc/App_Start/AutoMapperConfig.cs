﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using WR.Blog.Data.Models;
using WR.Blog.Mvc.Areas.SiteAdmin.Models;

using AutoMapper;
using WR.Blog.Mvc.Models;
using WR.Blog.Mvc.Config;

namespace WR.Blog.Mvc
{
    public class AutoMapperConfig
    {
        public static void CreateMaps()
        {
            // Blog version / post domain model mappings
            Mapper.CreateMap<BlogVersionDto, BlogPostDto>().ForMember(b => b.Id, opt => opt.Ignore());
            Mapper.CreateMap<BlogPostDto, BlogVersionDto>().ForMember(b => b.Id, opt => opt.Ignore());

            // Blog post domain / view model mappings
            Mapper.CreateMap<BlogPostDto, BlogPost>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.DisplayName))
                .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Comments.Where(c => c.IsApproved && !c.IsDeleted).Count()));
            Mapper.CreateMap<BlogPost, BlogPostDto>();

            // Blog version domain / view model mappings
            Mapper.CreateMap<BlogVersionDto, BlogVersion>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.DisplayName))
                .ForMember(dest => dest.VersionOfId, opt => opt.MapFrom(src => src.VersionOf.Id));
            Mapper.CreateMap<BlogVersion, BlogVersionDto>();

            // Blog settings domain / view model mappings
            Mapper.CreateMap<BlogSettingsDto, BlogSettingsDto>();
            Mapper.CreateMap<BlogSettingsDto, BlogSettings>();
            Mapper.CreateMap<BlogSettings, BlogSettingsDto>();
            Mapper.CreateMap<BlogSettingsDto, Settings>();
            Mapper.CreateMap<Settings, BlogSettings>();
            Mapper.CreateMap<BlogSettings, Settings>();

            // Blog comment domain / view model mappings
            Mapper.CreateMap<BlogCommentDto, BlogComment>()
                .ForMember(dest => dest.BlogPostId, opt => opt.MapFrom(src => src.BlogPost.Id))
                .ForMember(dest => dest.BlogPostTitle, opt => opt.MapFrom(src => src.BlogPost.Title));
            Mapper.CreateMap<BlogComment, BlogCommentDto>()
                .ForMember(dest => dest.Homepage, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Homepage) ? src.Homepage : ""));
        }
    }
}