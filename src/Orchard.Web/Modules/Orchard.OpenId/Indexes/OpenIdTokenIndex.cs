﻿using Orchard.DependencyInjection;
using Orchard.OpenId.Models;
using YesSql.Core.Indexes;

namespace Orchard.OpenId.Indexes
{
    public class OpenIdTokenIndex : MapIndex
    {
        public int UserId { get; set; }   
        public int AppId { get; set; }
    }

    public class OpenIdTokenIndexProvider : IndexProvider<OpenIdToken>, IDependency
    {
        public override void Describe(DescribeContext<OpenIdToken> context)
        {
            context.For<OpenIdTokenIndex>()
                .Map(openIdToken =>
                {
                    return new OpenIdTokenIndex
                    {
                        UserId = openIdToken.UserId,
                        AppId = openIdToken.AppId
                    };
                });
        }
    }
}