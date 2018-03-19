﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWT.TL.Infrastructure.Telegram
{
    public class AppUserSessionManager
    {
        private static readonly Lazy<AppUserSessionManager> LazyAppUserSessionManager = new Lazy<AppUserSessionManager>(() => new AppUserSessionManager());

        private static readonly Lazy<UserSessionManager> LazyUserSessionManager = new Lazy<UserSessionManager>(() =>
        {
            return new UserSessionManager();
        });

        private AppUserSessionManager()
        {
        }

        public static AppUserSessionManager Instance
        {
            get
            {
                return LazyAppUserSessionManager.Value;
            }
        }

        public UserSessionManager UserSessionManager
        {
            get { return LazyUserSessionManager.Value; }
        }
    }
}
