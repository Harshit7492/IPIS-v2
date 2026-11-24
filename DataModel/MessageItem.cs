using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace DataModel
    {
        public class MessageItem
        {
            public int Id { get; set; }         // MESSAGE_ID
            public string Message { get; set; } // MESSAGE text

            public override string ToString()
            {
                return Message; // so listbox displays the message text
            }
        }
    }

