#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;

#endregion

// username: jeffs
// created:  8/10/2020 7:49:50 PM

namespace SharedCode
{
	public class ShViewport
	{
	#region private fields

	#endregion

	#region ctor

	#endregion

	#region public properties

		public Viewport Viewport { get; set; }

		public bool Selected { get; set; } = true;

		public bool IsCopyable { get; set; }

		public string NewViewName { get; set; }

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

	#endregion

	#region event processing

	#endregion

	#region event handeling

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ShViewports";
		}

	#endregion
	}
}