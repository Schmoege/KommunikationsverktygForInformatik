﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Kommunikationsverktyg_för_informatik.Startup))]
namespace Kommunikationsverktyg_för_informatik
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
