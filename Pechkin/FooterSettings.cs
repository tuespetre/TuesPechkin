using System;

namespace Pechkin
{
    [Serializable]
    public class FooterSettings : HeaderSettings
    {
        protected readonly override string settingPrefix = "footer";
    }
}