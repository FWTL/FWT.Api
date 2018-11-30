using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FWT.Api.Controllers.Message
{
    public class GetDialogMessages
    {
        internal class Query
        {
            public Query()
            {
            }

            public string PhoneHashId { get; set; }
        }
    }
}
