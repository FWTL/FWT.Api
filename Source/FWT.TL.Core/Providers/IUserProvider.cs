using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWT.TL.Core.Providers
{
    public interface IUserProvider
    {
        int CurrentUserId { get; }
    }
}
