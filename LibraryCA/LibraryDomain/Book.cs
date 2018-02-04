using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDomain
{
	public class Book
	{
		public int BookID { get; set; }
		public string Name { get; set; }
		public string Author { get; set; }
		public int NumberAvailableBooks { get; set; }
	}
}
