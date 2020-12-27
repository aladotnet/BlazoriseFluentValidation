using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazoriseFluentValidation.WASM.Sample.Client.Components
{
    public class PersonViewModel
    {        
        public string FirstName
        { get; set; }
        public string LastName { get; set; }

        public int Age { get; set; }
    }
}
