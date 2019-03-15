using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApp.Models
{
    /// <summary>
    /// Model that represents list of to-do items
    /// </summary>
    public class TodoViewModel
    {
        public TodoItem[] Items { get; set; }
    }
}
