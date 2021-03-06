﻿/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(GraphAPIForEmail.Startup))]

namespace GraphAPIForEmail
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
